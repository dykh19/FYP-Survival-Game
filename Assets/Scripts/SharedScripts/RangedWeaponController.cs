﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;



[System.Serializable]
/*public struct CrosshairData
{
    [Tooltip("The image that will be used for this weapon's crosshair")]
    public Sprite CrosshairSprite;

    [Tooltip("The size of the crosshair image")]
    public int CrosshairSize;

    [Tooltip("The color of the crosshair image")]
    public Color CrosshairColor;
}*/

[RequireComponent(typeof(AudioSource))]
public class RangedWeaponController : WeaponController
{
    [Header("Information")] [Tooltip("The name that will be displayed in the UI for this weapon")]
    public string WeaponName;

    [Tooltip("The image that will be displayed in the UI for this weapon")]
    public Sprite WeaponIcon;

    //[Tooltip("Default data for the crosshair")]
    //public CrosshairData CrosshairDataDefault;

    //[Tooltip("Data for the crosshair when targeting an enemy")]
    //public CrosshairData CrosshairDataTargetInSight;

    [Header("Internal References")]
    [Tooltip("The root object for the weapon, this is what will be deactivated when the weapon isn't active")]
    public GameObject WeaponRoot;

    [Tooltip("Tip of the weapon, where the projectiles are shot")]
    public Transform WeaponMuzzle;

    [Tooltip("The projectile prefab")] public ProjectileBase ProjectilePrefab;

    [Tooltip("Minimum duration between two shots")]
    public float delayBetweenShots = 0.5f;

    [Tooltip("Angle for the cone in which the bullets will be shot randomly (0 means no spread at all)")]
    public float BulletSpreadAngle = 0f;

    [Tooltip("Amount of bullets per shot")]
    public int BulletsPerShot = 1;

    [Tooltip("Force that will push back the weapon after each shot")] [Range(0f, 2f)]
    public float RecoilForce = 1;

    [Tooltip("Ratio of the default FOV that this weapon applies while aiming")] [Range(0f, 1f)]
    public float AimZoomRatio = 1f;

    [Tooltip("Translation to apply to weapon arm when aiming with this weapon")]
    public Vector3 AimOffset;

    [Header("Ammo Parameters")]
    [Tooltip("Should the player manually reload")]
    public bool AutomaticReload = true;
    //[Tooltip("Has physical clip on the weapon and ammo shells are ejected when firing")]
    //public bool HasPhysicalBullets = false;
    [Tooltip("Number of bullets in a clip")]
    public int ClipSize = 30;
    //[Tooltip("Bullet Shell Casing")]
    //public GameObject ShellCasing;
    //[Tooltip("Weapon Ejection Port for physical ammo")]
    //public Transform EjectionPort;
    //[Tooltip("Force applied on the shell")]
    //[Range(0.0f, 5.0f)] public float ShellCasingEjectionForce = 2.0f;
    //[Tooltip("Maximum number of shell that can be spawned before reuse")]
    //[Range(1, 30)] public int ShellPoolSize = 1;
    [Tooltip("Amount of ammo reloaded per second")]
    public float AmmoReloadRate = 1f;

    [Tooltip("Delay after the last shot before starting to reload")]
    public float AmmoReloadDelay = 2f;

    [Tooltip("Should the player have infinite ammo reserve")]
    public bool InfiniteAmmo = false;

    [Tooltip("Maximum amount of ammo player can carry")]
    public int MaxAmmo = 900;

    public float damage = 30f;

    public float bulletLifeTime = 5f;

    [Header("Audio & Visual")] 
    [Tooltip("Optional weapon animator for OnShoot animations")]
    public Animator WeaponAnimator;

    [Tooltip("Prefab of the muzzle flash")]
    public GameObject MuzzleFlashPrefab;

    [Tooltip("Unparent the muzzle flash instance on spawn")]
    public bool UnparentMuzzleFlash;

    [Tooltip("sound played when shooting")]
    public AudioClip ShootSfx;

    [Tooltip("Sound played when changing to this weapon")]
    public AudioClip ChangeWeaponSfx;

    [Tooltip("Continuous Shooting Sound")] public bool UseContinuousShootSound = false;
    public AudioClip ContinuousShootStartSfx;
    public AudioClip ContinuousShootLoopSfx;
    public AudioClip ContinuousShootEndSfx;
    AudioSource m_ContinuousShootAudioSource = null;
    bool m_WantsToShoot = false;

    public AudioClip ReloadCooldownSound;

    public ParticleSystem CooldownEffect;

    public UnityAction OnShoot;
    public event Action OnShootProcessed;

    public float m_AmmoReserve;
    public float m_CurrentAmmoInClip;
    float m_LastTimeShot = Mathf.NegativeInfinity;
    //public float LastChargeTriggerTimestamp { get; private set; }
    Vector3 m_LastMuzzlePosition;
    public float CurrentAmmoRatio { get; private set; }
    public bool IsWeaponActive { get; private set; }
    public Vector3 MuzzleWorldVelocity { get; private set; }

    /*public float GetAmmoNeededToShoot() =>
        (ShootType != WeaponShootType.Charge ? 1f : Mathf.Max(1f, AmmoUsedOnStartCharge)) /
        (MaxAmmo * BulletsPerShot);*/

    //public int GetCarriedPhysicalBullets() => m_CarriedPhysicalBullets;
    //public int GetCurrentAmmo() => Mathf.FloorToInt(m_CurrentAmmoInClip);

    AudioSource m_ShootAudioSource;

    public bool IsReloading { get; private set; }

    const string k_AnimAttackParameter = "Attack";

    public bool IsTurret;

    //private Queue<Rigidbody> m_PhysicalAmmoPool;

    void Awake()
    {
        m_AmmoReserve = MaxAmmo;
        m_CurrentAmmoInClip = ClipSize;

        //m_CarriedPhysicalBullets = HasPhysicalBullets ? ClipSize : 0;
        m_LastMuzzlePosition = WeaponMuzzle.position;

        m_ShootAudioSource = GetComponent<AudioSource>();


        if (UseContinuousShootSound)
        {
            m_ContinuousShootAudioSource = gameObject.AddComponent<AudioSource>();
            m_ContinuousShootAudioSource.playOnAwake = false;
            m_ContinuousShootAudioSource.clip = ContinuousShootLoopSfx;
            //m_ContinuousShootAudioSource.outputAudioMixerGroup =
            //    AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.WeaponShoot);
            m_ContinuousShootAudioSource.loop = true;
        }

        /*if (HasPhysicalBullets)
        {
            m_PhysicalAmmoPool = new Queue<Rigidbody>(ShellPoolSize);

            for (int i = 0; i < ShellPoolSize; i++)
            {
                GameObject shell = Instantiate(ShellCasing, transform);
                shell.SetActive(false);
                m_PhysicalAmmoPool.Enqueue(shell.GetComponent<Rigidbody>());
            }
        }*/

        if (gameObject.CompareTag("Turret"))
        {
            Owner = gameObject;
            IsTurret = true;
        }

    }

    //public void AddCarriablePhysicalBullets(int count) => m_CarriedPhysicalBullets = Mathf.Max(m_CarriedPhysicalBullets + count, MaxAmmo);

    /*void ShootShell()
    {
        Rigidbody nextShell = m_PhysicalAmmoPool.Dequeue();

        nextShell.transform.position = EjectionPort.transform.position;
        nextShell.transform.rotation = EjectionPort.transform.rotation;
        nextShell.gameObject.SetActive(true);
        nextShell.transform.SetParent(null);
        nextShell.collisionDetectionMode = CollisionDetectionMode.Continuous;
        nextShell.AddForce(nextShell.transform.up * ShellCasingEjectionForce, ForceMode.Impulse);

        m_PhysicalAmmoPool.Enqueue(nextShell);
    }*/

    //void PlaySFX(AudioClip sfx) => AudioUtility.CreateSFX(sfx, transform.position, AudioUtility.AudioGroups.WeaponShoot, 0.0f);


    public void Reload()
    {
        if(InfiniteAmmo == false)
        {
            if (m_AmmoReserve > 0)
            {
                //Check if there is enough for full clip if not reload all remaining ammo
                float AmmoAvailableToReload = Mathf.Min(m_AmmoReserve, ClipSize);  
                //Deduct the ammo from the total count
                m_AmmoReserve -= (Mathf.Min(ClipSize - m_CurrentAmmoInClip, m_AmmoReserve));
                //Add the ammo into the clip
                m_CurrentAmmoInClip = AmmoAvailableToReload;
            }
        }
        else
        {
            m_CurrentAmmoInClip = ClipSize;
        }
        StartCoroutine(ReloadDelay());
        StartCoroutine(AudioFadeOut(m_ShootAudioSource, ReloadCooldownSound, AmmoReloadDelay));
        //IsReloading = false;
    }

    IEnumerator AudioFadeOut(AudioSource audioSource, AudioClip sound, float FadeTime)
    {
        float startVolume = audioSource.volume;
        audioSource.PlayOneShot(sound);
        CooldownEffect.Play();

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        CooldownEffect.Stop();
        audioSource.volume = startVolume;
    }

    IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(AmmoReloadDelay);
        IsReloading = false;
    }

    public void StartReloadAnimation()
    {
        if (m_CurrentAmmoInClip < m_AmmoReserve && !Owner.GetComponent<PlayerWeaponsManager>().IsAiming)
        {
            //GetComponent<Animator>().SetTrigger("Reload");
            IsReloading = true;
            Reload();
        }
    }

    void Update()
    {
        UpdateAmmo();
        UpdateContinuousShootSound();

        if (Time.deltaTime > 0)
        {
            MuzzleWorldVelocity = (WeaponMuzzle.position - m_LastMuzzlePosition) / Time.deltaTime;
            m_LastMuzzlePosition = WeaponMuzzle.position;
        }
    }

    void UpdateAmmo()
    {
        if (AutomaticReload && m_CurrentAmmoInClip == 0)
        {
            StartReloadAnimation();
        }

        CurrentAmmoRatio = m_CurrentAmmoInClip / ClipSize;
    }

    void UpdateContinuousShootSound()
    {
        if (UseContinuousShootSound)
        {
            if (m_WantsToShoot && m_CurrentAmmoInClip >= 1f)
            {
                if (!m_ContinuousShootAudioSource.isPlaying)
                {
                    m_ShootAudioSource.PlayOneShot(ShootSfx);
                    m_ShootAudioSource.PlayOneShot(ContinuousShootStartSfx);
                    m_ContinuousShootAudioSource.Play();
                }
            }
            else if (m_ContinuousShootAudioSource.isPlaying)
            {
                m_ShootAudioSource.PlayOneShot(ContinuousShootEndSfx);
                m_ContinuousShootAudioSource.Stop();
            }
        }
    }

    public void ShowWeapon(bool show)
    {
        WeaponRoot.SetActive(show);

        if (show && ChangeWeaponSfx)
        {
            m_ShootAudioSource.PlayOneShot(ChangeWeaponSfx);
        }

        IsWeaponActive = show;
    }

    public void UseAmmo(float amount)
    {
        m_CurrentAmmoInClip = Mathf.Clamp(m_CurrentAmmoInClip - amount, 0f, MaxAmmo);
        m_AmmoReserve -= Mathf.RoundToInt(amount);
        m_AmmoReserve = Mathf.Clamp(m_AmmoReserve, 0, MaxAmmo);
        m_LastTimeShot = Time.time;
    }

    public bool HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
    {
        m_WantsToShoot = inputDown || inputHeld;
        switch (ShootType)
        {
            case WeaponShootType.Manual:
                if (inputDown)
                {
                    return TryShoot();
                }

                return false;

            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    return TryShoot();
                }

                return false;

            default:
                return false;
        }
    }

    public bool TryShoot()
    {
        if (IsTurret)
        {
            if (m_LastTimeShot + delayBetweenShots < Time.time)
            {
                HandleShoot();
                return true;
            }
        }
        else
        {
            if (m_CurrentAmmoInClip >= 1f
            && m_LastTimeShot + delayBetweenShots < Time.time)
            {
                HandleShoot();
                m_CurrentAmmoInClip -= 1f;
                return true;
            }
        }
        return false;
    }


    void HandleShoot()
    {
        int bulletsPerShotFinal = BulletsPerShot;

        // spawn all bullets with random direction
        for (int i = 0; i < bulletsPerShotFinal; i++)
        {
            Vector3 shotDirection = GetShotDirectionWithinSpread(WeaponMuzzle);
            ProjectileBase newProjectile = Instantiate(ProjectilePrefab, WeaponMuzzle.position,
                Quaternion.LookRotation(shotDirection));
            newProjectile.Damage = damage;
            newProjectile.MaxLifeTime = bulletLifeTime;
            newProjectile.Shoot(this);
        }

        // muzzle flash
        if (MuzzleFlashPrefab != null)
        {
            GameObject muzzleFlashInstance = Instantiate(MuzzleFlashPrefab, WeaponMuzzle.position,
                WeaponMuzzle.rotation, WeaponMuzzle.transform);
            // Unparent the muzzleFlashInstance
            if (UnparentMuzzleFlash)
            {
                muzzleFlashInstance.transform.SetParent(null);
            }

            Destroy(muzzleFlashInstance, 2f);
        }

        /*if (HasPhysicalBullets)
        {
            ShootShell();
            m_CarriedPhysicalBullets--;
        }*/

        m_LastTimeShot = Time.time;

        // play shoot SFX
        if (ShootSfx && !UseContinuousShootSound)
        {
            m_ShootAudioSource.PlayOneShot(ShootSfx);
        }

        // Trigger attack animation if there is any
        if (WeaponAnimator)
        {
            WeaponAnimator.SetTrigger(k_AnimAttackParameter);
        }

        OnShoot?.Invoke();
        OnShootProcessed?.Invoke();
    }

    public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadAngleRatio = BulletSpreadAngle / 180f;
        Vector3 spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere,
            spreadAngleRatio);

        return spreadWorldDirection;
    }
}