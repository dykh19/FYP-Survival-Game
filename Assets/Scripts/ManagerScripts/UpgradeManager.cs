using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Use these variables to get the current level for your NPC UI
    public int playerHealthLevel = 0;
    public int baseHealthLevel = 0;

    public int playerRifleLevel = 0;
    public int playerShotgunLevel = 0;
    public int playerAxeLevel = 0;
    public int playerSwordLevel = 0;

    // References to the upgradable objects
    Health playerHealthClass;
    Health baseHealthClass;

    PlayerWeaponsManager playerWeaponManager;

    // Start is called before the first frame update
    void Start()
    {
        playerHealthClass = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        baseHealthClass = GameObject.FindGameObjectWithTag("Base").GetComponent<Health>();
        playerWeaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeaponsManager>();
    }

    // Call this to upgrade player health by 1 level
    public void UpgradePlayerHealth()
    {
        playerHealthLevel++;
        float newPlayerHealth = playerHealthClass.MaxHealth + 20;
        playerHealthClass.SetHealth(newPlayerHealth); 
    }

    // Call this to upgrade base health by 1 level
    public void UpgradeBaseHealth()
    {
        baseHealthLevel++;
        float newBaseHealth = GameStats.PlayerBaseHealth[baseHealthLevel];
        baseHealthClass.SetHealth(newBaseHealth);
    }

    // Call this with weapon index to upgrade by 1 level (Rifle - 0, Shotgun - 1, Axe - 3, Sword - 4)
    public void UpgradePlayerWeapon(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                {
                    playerRifleLevel++;
                    //Set Damage
                    float newDamage = playerWeaponManager.m_WeaponSlots[0].GetComponent<RangedWeaponController>().damage + 5;
                    playerWeaponManager.m_WeaponSlots[0].GetComponent<RangedWeaponController>().damage = newDamage;

                    //Set Fire Rate
                    float newFireRate = playerWeaponManager.m_WeaponSlots[0].GetComponent<RangedWeaponController>().delayBetweenShots - 0.02f;
                    playerWeaponManager.m_WeaponSlots[0].GetComponent<RangedWeaponController>().delayBetweenShots = newFireRate;
                    
                    //Set Range
                    if (playerRifleLevel % 3 == 0)
                    {
                        float newRange = playerWeaponManager.m_WeaponSlots[0].GetComponent<RangedWeaponController>().bulletLifeTime + 1f;
                        playerWeaponManager.m_WeaponSlots[0].GetComponent<RangedWeaponController>().bulletLifeTime = newRange;
                    }
                    break;
                }
            case 1:
                {
                    playerShotgunLevel++;
                    //Set Damage
                    float newDamage = playerWeaponManager.m_WeaponSlots[1].GetComponent<RangedWeaponController>().damage + 5;
                    playerWeaponManager.m_WeaponSlots[1].GetComponent<RangedWeaponController>().damage = newDamage;

                    //Set Fire Rate
                    float newFireRate = playerWeaponManager.m_WeaponSlots[1].GetComponent<RangedWeaponController>().delayBetweenShots - 0.02f;
                    playerWeaponManager.m_WeaponSlots[1].GetComponent<RangedWeaponController>().delayBetweenShots = newFireRate;

                    //Set Range
                    if (playerShotgunLevel % 3 == 0)
                    {
                        float newRange = playerWeaponManager.m_WeaponSlots[1].GetComponent<RangedWeaponController>().bulletLifeTime + 0.5f;
                        playerWeaponManager.m_WeaponSlots[1].GetComponent<RangedWeaponController>().bulletLifeTime = newRange;
                    }
                    break;
                }
            case 2:
                {
                    playerAxeLevel++;
                    //Set Damage
                    float newDamage = playerWeaponManager.m_WeaponSlots[2].GetComponent<MeleeWeaponController>().damage + 5;
                    playerWeaponManager.m_WeaponSlots[2].GetComponent<MeleeWeaponController>().damage = newDamage;

                    //Set Fire Rate
                    float newFireRate = playerWeaponManager.m_WeaponSlots[2].GetComponent<MeleeWeaponController>().attackSpeed - 0.2f;
                    playerWeaponManager.m_WeaponSlots[2].GetComponent<MeleeWeaponController>().attackSpeed = newFireRate;

                    //Set Range
                    if (playerAxeLevel % 3 == 0)
                    {
                        float newRange = playerWeaponManager.m_WeaponSlots[2].GetComponent<MeleeWeaponController>().attackRange + 0.3f;
                        playerWeaponManager.m_WeaponSlots[2].GetComponent<MeleeWeaponController>().attackRange = newRange;
                    }
                    break;
                }
            case 3:
                {
                    playerSwordLevel++;
                    //Set Damage
                    float newDamage = playerWeaponManager.m_WeaponSlots[3].GetComponent<MeleeWeaponController>().damage + 5;
                    playerWeaponManager.m_WeaponSlots[3].GetComponent<MeleeWeaponController>().damage = newDamage;

                    //Set Fire Rate
                    float newFireRate = playerWeaponManager.m_WeaponSlots[3].GetComponent<MeleeWeaponController>().attackSpeed - 0.5f;
                    playerWeaponManager.m_WeaponSlots[3].GetComponent<MeleeWeaponController>().attackSpeed = newFireRate;

                    //Set Range
                    if (playerSwordLevel % 3 == 0)
                    {
                        float newRange = playerWeaponManager.m_WeaponSlots[3].GetComponent<MeleeWeaponController>().attackRange + 0.5f;
                        playerWeaponManager.m_WeaponSlots[3].GetComponent<MeleeWeaponController>().attackRange = newRange;
                    }
                    break;
                }
        }
    }
}
