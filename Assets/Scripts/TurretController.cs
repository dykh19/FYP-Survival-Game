using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Referenced from Unity FPS Template scripts
public class TurretController : MonoBehaviour
{
    // Detection Variables
    public Transform detectionSourcePoint;
    public float detectionRange = 30f;
    public float attackRange = 30f;
    public float knownTargetTimeout = 4f;
    public GameObject knownDetectedTarget;
    public bool isTargetInDetectionRange; 
    public bool isTargetInAttackRange;
    public bool hadKnownTarget;
    protected float timeLastSeenTarget = Mathf.NegativeInfinity;

    // Turret Variables
    public enum AIState
    {
        Idle,
        Attack,
    }
    AIState aiState;
    public Transform turretPivot;
    public Transform turretAimPoint;
    public Animator animator;
    public float aimRotationSharpness = 5f;
    public float lookAtRotationSharpness = 2.5f;
    public float detectionFireDelay = 1f;
    public float aimingTransitionBlendTime = 1f;

    public ParticleSystem[] onDetectVFX;
    public AudioClip onDetectSFX;

    public RangedWeaponController turretWeapon;

    Quaternion rotationWeaponForwardToPivot;
    float timeStartedDetection;
    float timeLostDetection;
    Quaternion previousPivotAimingRotation;
    Quaternion pivotAimingRotation;

    float lastTimeShot;
    float delayBetweenShots;



    // References
    Monster_Spawner monsterSpawner;
    public AudioSource audioSource;
    public AudioClip turretShootSFX;

    private void Start()
    {
        monsterSpawner = GameObject.Find("Spawner").GetComponent<Monster_Spawner>();

        rotationWeaponForwardToPivot = Quaternion.Inverse(turretWeapon.WeaponMuzzle.rotation) * turretPivot.rotation;

        aiState = AIState.Idle;

        timeStartedDetection = Mathf.NegativeInfinity;
        previousPivotAimingRotation = turretPivot.rotation;

        delayBetweenShots = turretWeapon.delayBetweenShots;
    }

    private void Update()
    {
        HandleTargetDetection();
        UpdateCurrentAiState();
    }

    private void LateUpdate()
    {
        UpdateTurretAiming();
    }

    public void HandleTargetDetection()
    {
        // Handle known target detection timeout
        if (knownDetectedTarget && !isTargetInDetectionRange && (Time.time - timeLastSeenTarget) > knownTargetTimeout)
        {
            knownDetectedTarget = null;
        }

        // Find closest visible enemy
        float sqrDetectionRange = detectionRange * detectionRange;
        isTargetInDetectionRange = false;
        float closestSqrDistance = Mathf.Infinity;
        foreach (Transform child in monsterSpawner.transform)
        {
            //Check every enemy distance
            float sqrDistance = (child.GetChild(0).position - detectionSourcePoint.position).sqrMagnitude;
            if (sqrDistance < sqrDetectionRange && sqrDistance < closestSqrDistance)
            {
                //If no obstacles between turret and enemy, and is closest to turret, set enemy as target
                RaycastHit[] hits = Physics.RaycastAll(detectionSourcePoint.position, (child.GetChild(0).position - detectionSourcePoint.position).normalized, detectionRange, -1, QueryTriggerInteraction.Ignore);
                RaycastHit closestValidHit = new RaycastHit();
                closestValidHit.distance = Mathf.Infinity;
                bool foundValidHit = false;
                foreach (var hit in hits)
                {
                    if (hit.distance < closestValidHit.distance && hit.collider.CompareTag("Enemy"))
                    {
                        closestValidHit = hit;
                        foundValidHit = true;
                    }
                }

                if (foundValidHit)
                {
                    isTargetInDetectionRange = true;
                    closestSqrDistance = sqrDistance;
                    timeLastSeenTarget = Time.time;
                    knownDetectedTarget = child.gameObject;
                }
            }
        }

        isTargetInAttackRange = knownDetectedTarget != null && Vector3.Distance(transform.position, knownDetectedTarget.transform.position) <= attackRange;

        // Detection Events
        if (!hadKnownTarget && knownDetectedTarget != null)
        {
            TargetDetected();
        }

        if (hadKnownTarget && knownDetectedTarget == null)
        {
            TargetLost();
        }

        // Remember if target detected in previous frame
        hadKnownTarget = knownDetectedTarget != null;
    }

    public void TargetDetected()
    {
        if (aiState == AIState.Idle)
        {
            aiState = AIState.Attack;
        }

        //Play SFX and VFX
        audioSource.PlayOneShot(onDetectSFX);

        animator.SetBool("IsActive", true);
        timeStartedDetection = Time.time;
    }

    public void TargetLost()
    {
        if (aiState == AIState.Attack)
        {
            aiState = AIState.Idle;
        }

        //Stop SFX and VFX

        animator.SetBool("IsActive", false);
        timeLostDetection = Time.time;
    }

    public void UpdateTurretAiming()
    {
        switch (aiState)
        {
            case AIState.Attack:
                {
                    turretPivot.rotation = pivotAimingRotation;
                    break;
                }
            default:
                {
                    turretPivot.rotation = Quaternion.Slerp(pivotAimingRotation, turretPivot.rotation, (Time.time - timeLostDetection) / aimingTransitionBlendTime);
                    break;
                }
        }

        previousPivotAimingRotation = turretPivot.rotation;
    }

    public void UpdateCurrentAiState()
    {
        switch (aiState)
        {
            case AIState.Attack:
                {
                    bool mustShoot = Time.time > timeStartedDetection + detectionFireDelay;
                    Vector3 directionToTarget = (knownDetectedTarget.transform.GetChild(0).position - turretAimPoint.position).normalized;
                    Quaternion offsetTargetRotation = Quaternion.LookRotation(directionToTarget) * rotationWeaponForwardToPivot;
                    pivotAimingRotation = Quaternion.Slerp(previousPivotAimingRotation, offsetTargetRotation, (mustShoot ? aimRotationSharpness : lookAtRotationSharpness) * Time.deltaTime);

                    

                    if (mustShoot)
                    {
                        Vector3 correctedDirectionToTarget = (pivotAimingRotation * Quaternion.Inverse(rotationWeaponForwardToPivot)) * Vector3.forward;
                        OrientWeaponTowards(turretAimPoint.position + correctedDirectionToTarget);
                        turretWeapon.TryShoot();
                        if (lastTimeShot + delayBetweenShots < Time.time)
                        {
                            audioSource.PlayOneShot(turretShootSFX);
                            lastTimeShot = Time.time;
                        }
                        
                    }
                    break;
                }
        }
        
    }

    public void OrientWeaponTowards(Vector3 position)
    {
        Vector3 weaponForward = (position - turretWeapon.WeaponRoot.transform.position).normalized;
        turretWeapon.transform.forward = weaponForward;
    }
        
}
