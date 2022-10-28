using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeWeaponController : WeaponController
{
    [Header("Appearance Settings")]
    public Vector3 offset;

    [Header("Attack Settings")]
    public bool canBreakObjects;
    public float damage;
    public float damageModifier = 0;
    [Range(2, 10)] public float attackRange;
    [Range(1, 180)] public float attackWidth;

    [Header("Animation Settings")]
    public float attackSpeed = 1;
    [Range(45, 135)] public float swingAngle = 80;
    public bool swingSideways;

    public Transform modelObject;
    private MeleeWeaponAnimator animator;

    public void Awake()
    {
        modelObject = this.transform;
        animator = new MeleeWeaponAnimator(modelObject, attackSpeed, swingAngle);
        
        this.gameObject.SetActive(false);
    }

    public void Start()
    {
        WeaponFollow();
    }

    public void Update()
    {
        animator.UpdateAnimation(swingSideways);
    }

    public void OnUse()
    {
        if (animator.StartAnimation())
            HitTargets();
            animator.UpdateAnimation(swingSideways);
    }

    private void WeaponFollow()
    {
        var camera = Camera.main.transform;

        var offsetX = this.offset.x * camera.right;
        var offsetY = this.offset.y * camera.up;
        var offsetZ = this.offset.z * camera.forward;
        var offset = offsetX + offsetY + offsetZ;

        modelObject.localPosition = offset;
    }

    private void HitTargets()
    {
        var hits = SectorCast();

        foreach (var hit in hits)
        {
            if (canBreakObjects)
            {
                var breakable = hit.transform.GetComponentInParent<Breakable>();
                if (breakable is not null)
                {
                    breakable.OnHit(damage);
                    continue;
                }
            }

            if (hit.transform.CompareTag("Enemy"))
            {
                var health = hit.transform.GetComponentInParent<Health>();
                health.TakeDamage(damage + (damageModifier * damage));

                var player = GameObject.FindGameObjectWithTag("Player");
                var playerController = player.GetComponent<PlayerCharacterController>();
                playerController.PlayHitSound();
            }
        }
    }

    private RaycastHit[] SectorCast()
    {
        var camera = Camera.main.transform;
        var origin = camera.position;
        var halfWidth = attackWidth;

        var hitList = new List<RaycastHit>();

        for (var i = -halfWidth; i < halfWidth; i++)
        {
            var direction = Quaternion.AngleAxis(i, camera.up) * camera.forward;
            var hits = Physics.RaycastAll(origin, direction, attackRange);

            var uniqueHits = hits
                .Where(hit => !hitList
                .Any(h => h.transform.name == hit.transform.name));

            hitList.AddRange(uniqueHits);
        }

        return hitList.ToArray();
    }

    public void HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
    {
        if(inputDown)
        {
            OnUse();
        }
    }

    public void ShowWeapon(bool show)
    {
        modelObject.gameObject.SetActive(show);
    }
}

public class MeleeWeaponAnimator
{
    private readonly Transform modelObject;
    private readonly float attackSpeed;
    private readonly float swingAngle;

    private AnimationState animationState = AnimationState.None;
    private float animationFrame = 0;

    public MeleeWeaponAnimator(Transform modelObject, float attackSpeed, float swingAngle)
    {
        this.modelObject = modelObject;
        this.attackSpeed = attackSpeed;
        this.swingAngle = swingAngle;
    }

    public bool StartAnimation()
    {
        if (animationState == AnimationState.None)
        {
            animationState = AnimationState.Forward;
            return true;
        }
        return false;
    }

    public void UpdateAnimation(bool sideways)
    {
        var camera = Camera.main.transform;
        
        if (animationState == AnimationState.None)
        {
            modelObject.rotation = Quaternion.LookRotation(camera.forward);
        }
        else
        {
            IncrementAnimationFrame();

            var axis = sideways ? camera.up : camera.right;
            var targetDirection = Quaternion.AngleAxis(swingAngle, axis) * camera.forward;
            var transitionDirection = Vector3.Lerp(camera.forward, targetDirection, animationFrame);
            var rotation = Quaternion.LookRotation(transitionDirection);
            modelObject.rotation = rotation;

            CheckAnimationState();
        }
    }

    private void IncrementAnimationFrame()
    {
        if (animationState == AnimationState.Reverse)
            animationFrame -= attackSpeed * Time.deltaTime;
        else
            animationFrame += attackSpeed * Time.deltaTime;
    }

    private void CheckAnimationState()
    {
        if (animationFrame > 1)
            animationState = AnimationState.Reverse;
        else if (animationFrame < 0)
            animationState = AnimationState.None;
    }

    private enum AnimationState
    {
        None,
        Forward,
        Reverse
    }
}
