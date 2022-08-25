using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 22/08/2022.

// TODO: Play sound effect.

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Melee Weapon")]
public class MeleeWeapon : GameItem
{
    [Header("Appearance Settings")]
    public GameObject model;
    public Vector3 offset;

    [Header("Crosshair Settings")]
    public Sprite crosshair;
    public float crosshairSize = 50;
    public Color crosshairColour = Color.white;

    [Header("Attack Settings")]
    public bool canBreakObjects;
    public float damage;
    [Range(0.1f, 5)] public float attackRange;
    [Range(1, 180)]  public float attackWidth;

    [Header("Animation Settings")]
    public float attackSpeed = 1;
    [Range(45, 135)] public float swingAngle = 80;
    public bool swingSideways;

    private Transform modelObject;
    private MeleeWeaponAnimator animator;

    public override void OnHoldEnter()
    {
        modelObject = Instantiate(model).transform;
        animator = new MeleeWeaponAnimator(modelObject, attackSpeed, swingAngle);

        // Set the crosshair.
        PlayerHUD.Main.SetCrosshair(crosshair, Vector2.one * crosshairSize, crosshairColour);
    }

    public override void OnHoldStay()
    {
        if (modelObject != null)
        {
            WeaponFollow();
            animator.UpdateAnimation(swingSideways);
        }
    }

    public override void OnHoldExit()
    {
        if (modelObject != null)
            Destroy(modelObject.gameObject);

        // Unset the crosshair.
        PlayerHUD.Main.RemoveCrosshair();
    }

    public override void OnUse()
    {
        if (animator.StartAnimation())
            HitTargets();
    }

    private void WeaponFollow()
    {
        var camera = Camera.main.transform;

        var offsetX = this.offset.x * camera.right;
        var offsetY = this.offset.y * camera.up;
        var offsetZ = this.offset.z * camera.forward;
        var offset = offsetX + offsetY + offsetZ;

        modelObject.position = camera.position + offset;
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

            var strikeable = hit.transform.GetComponentInParent<Strikeable>();
            if (strikeable is not null)
            {
                strikeable.OnHit(damage);
            }
        }
    }

    private RaycastHit[] SectorCast()
    {
        var origin = GameManagerJoseph.Main.playerStatus.transform.position;
        var camera = Camera.main.transform;
        var halfWidth = attackWidth / 2;

        var hitList = new List<RaycastHit>();

        for (var i = -halfWidth; i < halfWidth; i++)
        {
            var direction = Quaternion.AngleAxis(i, camera.up) * camera.forward;
            var hits = Physics.RaycastAll(origin, direction, attackRange);

            // Debug.DrawRay(origin, direction * attackRange, Color.red, 1, false);

            var uniqueHits = hits
                .Where(hit => !hitList
                .Any(h => h.transform.name == hit.transform.name));

            hitList.AddRange(uniqueHits);
        }

        return hitList.ToArray();
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
