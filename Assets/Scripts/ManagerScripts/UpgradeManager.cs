using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Use these variables to get the current level for your NPC UI
    public int playerHealthLevel = 0;
    public int baseLevel = 0;

    public int playerRifleLevel = 0;
    public int playerShotgunLevel = 0;
    public int playerAxeLevel = 0;
    public int playerSwordLevel = 0;


    // References to the upgradable objects
    GameObject playerObject;
    GameObject baseObject;

    PlayerWeaponsManager playerWeaponManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SaveData += SaveUpgrades;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        baseObject = GameObject.FindGameObjectWithTag("Base").transform.root.gameObject;
        playerWeaponManager = playerObject.GetComponent<PlayerWeaponsManager>();
        if (GameManager.Instance.LoadingSavedGame == true)
        {
            LoadUpgrades();
        }
    }


    // Call this to upgrade player health by 1 level
    public void UpgradePlayerHealth()
    {
        float newPlayerHealth = playerObject.GetComponent<Health>().MaxHealth + 20;
        playerObject.GetComponent<Health>().SetHealth(newPlayerHealth); 
    }

    // Call this to upgrade base by 1 level
    public void UpgradeBase(bool UpgradeHealth = true)
    {
        if (UpgradeHealth)
        {
            // Upgrade base health
            float newBaseHealth = GameStats.PlayerBaseHealth[baseLevel];
            baseObject.GetComponent<Health>().SetHealth(newBaseHealth);
        }
        // Upgrade base turrets
        switch (baseLevel)
        {
            case 1:
                {
                    baseObject.transform.GetChild(0).gameObject.SetActive(true);
                    foreach (Transform turret in baseObject.transform.GetChild(0).gameObject.transform)
                    {
                        turret.GetComponentInChildren<RangedWeaponController>().damage = GameStats.PlayerBaseTurretDamage[1];
                    }
                    break;
                }
            case 2:
                {
                    baseObject.transform.GetChild(0).gameObject.SetActive(false);
                    baseObject.transform.GetChild(1).gameObject.SetActive(true);
                    foreach (Transform turret in baseObject.transform.GetChild(1).gameObject.transform)
                    {
                        turret.GetComponentInChildren<RangedWeaponController>().damage = GameStats.PlayerBaseTurretDamage[2];
                    }
                    
                    break;
                }
            case 3:
                {
                    foreach (Transform turret in baseObject.transform.GetChild(1).gameObject.transform)
                    {
                        turret.GetComponentInChildren<RangedWeaponController>().damage = GameStats.PlayerBaseTurretDamage[3];
                    }
                    break;
                }
            case 4:
                {
                    baseObject.transform.GetChild(1).gameObject.SetActive(false);
                    baseObject.transform.GetChild(2).gameObject.SetActive(true);
                    foreach (Transform turret in baseObject.transform.GetChild(2).gameObject.transform)
                    {
                        turret.GetComponentInChildren<RangedWeaponController>().damage = GameStats.PlayerBaseTurretDamage[4];
                    }
                    break;
                }
        }
    }

    // Call this with weapon index to upgrade by 1 level (Rifle - 0, Shotgun - 1, Axe - 2, Sword - 3)
    public void UpgradePlayerWeapon(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                {
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
                    //Set Damage
                    float newDamage = playerWeaponManager.m_WeaponSlots[2].GetComponent<MeleeWeaponController>().damage + 5;
                    playerWeaponManager.m_WeaponSlots[2].GetComponent<MeleeWeaponController>().damage = newDamage;

                    //Set Fire Rate
                    float newFireRate = playerWeaponManager.m_WeaponSlots[2].GetComponent<MeleeWeaponController>().attackSpeed - 0.02f;
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
                    //Set Damage
                    float newDamage = playerWeaponManager.m_WeaponSlots[3].GetComponent<MeleeWeaponController>().damage + 5;
                    playerWeaponManager.m_WeaponSlots[3].GetComponent<MeleeWeaponController>().damage = newDamage;

                    //Set Fire Rate
                    float newFireRate = playerWeaponManager.m_WeaponSlots[3].GetComponent<MeleeWeaponController>().attackSpeed - 0.05f;
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

    public void SaveUpgrades()
    {
        GameManager.Instance.PlayerStats.playerHealthLevel = playerHealthLevel;
        GameManager.Instance.PlayerStats.baseLevel = baseLevel;
        GameManager.Instance.PlayerStats.playerRifleLevel = playerRifleLevel;
        GameManager.Instance.PlayerStats.playerShotgunLevel = playerShotgunLevel;
        GameManager.Instance.PlayerStats.playerAxeLevel = playerAxeLevel;
        GameManager.Instance.PlayerStats.playerSwordLevel = playerSwordLevel;
    }

    public void LoadUpgrades()
    {
        playerHealthLevel = GameManager.Instance.PlayerStats.playerHealthLevel;
        baseLevel = GameManager.Instance.PlayerStats.baseLevel;
        playerRifleLevel = GameManager.Instance.PlayerStats.playerRifleLevel;
        playerShotgunLevel = GameManager.Instance.PlayerStats.playerShotgunLevel;
        playerAxeLevel = GameManager.Instance.PlayerStats.playerAxeLevel;
        playerSwordLevel = GameManager.Instance.PlayerStats.playerSwordLevel;
        UpgradeBase(false);
        for (int i = 0; i < playerHealthLevel; i++)
        {
            UpgradePlayerHealth();
        }
        for (int i = 0; i < playerRifleLevel; i++)
        {
            UpgradePlayerWeapon(1);
        }
        for (int i = 0; i < playerShotgunLevel; i++)
        {
            UpgradePlayerWeapon(2);
        }
        for (int i = 0; i < playerAxeLevel; i++)
        {
            UpgradePlayerWeapon(3);
        }
        for (int i = 0; i < playerSwordLevel; i++)
        {
            UpgradePlayerWeapon(4);
        }
       
    }
}
