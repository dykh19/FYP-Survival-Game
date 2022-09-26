using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script First Created by Sean Ong on 26/9/22 12:10pm*/

public class Upgrade : MonoBehaviour
{

    public enum EquipmentUpgradeType    //Uses Essence to Upgrade
    {
        LMelee_1, LMelee_2, LMelee_3, LMelee_4, LMeleeMax,
        HMelee_1, HMelee_2, HMelee_3, HMelee_4, HMeleeMax,
        Rifle_1, Rifle_2, Rifle_3, Rifle_4, RifleMax,
        Shotgun_1, Shotgun_2, Shotgun_3, Shotgun_4, ShotgunMax,
    }

    /*public enum PassiveSkillUpgradeType
    {
        HealthNone, Health_1, Health_2, Health_3, Health_4, Health_5, Health_6, Health_7, Health_8, Health_9, Health_10,
        MSNone, MS_1, MS_2, MS_3, MS_4, MS_5, MS_6, MS_7, MS_8, MS_9, MS_10,
        JHNone, JH_1, JH_2, JH_3, JH_4, JH_5,
        ASNone, AS_1, AS_2, AS_3, AS_4, AS_5, AS_6, AS_7, AS_8, AS_9, AS_10,
        DMGNone, DMG_1, DMG_2, DMG_3, DMG_4, DMG_5, DMG_6, DMG_7, DMG_8, DMG_9, DMG_10,
        HealNone, Heal_1, Heal_2, Heal_3, Heal_4, Heal_5, Heal_6, Heal_7, Heal_8, Heal_9, Heal_10,
        GatherNone, Gather_1, Gather_2, Gather_3, Gather_4, Gather_5, Gather_6, Gather_7, Gather_8, Gather_9, Gather_10,
        XchangeRateNone, XchangeRate_1, XchangeRate_2, XchangeRate_3, XchangeRate_4, XchangeRate_5, XchangeRate_6, XchangeRate_7, XchangeRate_8, XchangeRate_9, XchangeRate_10,
        DropRateNone, DropRate_1, DropRate_2, DropRate_3, DropRate_4, DropRate_5, DropRate_6, DropRate_7, DropRate_8, DropRate_9, DropRate_10
    }*/

    public enum ActiveSkillType
    {
        Warp
    }

    public enum BaseUpgradeType     //Uses Ores to Upgrade
    {
        BaseNone, Base_1, Base_2, Base_3, Base_4
    }

    public static int GetCost(EquipmentUpgradeType equipmentUpgradeType)
    {
        switch(equipmentUpgradeType)
        {
            default:
            //Light Melee Weapon Cases
            case EquipmentUpgradeType.LMelee_1:
                return 400;
            case EquipmentUpgradeType.LMelee_2:
                return 600;
            case EquipmentUpgradeType.LMelee_3:
                return 800;
            case EquipmentUpgradeType.LMelee_4:
                return 1000;
            case EquipmentUpgradeType.LMeleeMax:
                return 0;
            //Heavy Melee Weapon Cases
            case EquipmentUpgradeType.HMelee_1:
                return 400;
            case EquipmentUpgradeType.HMelee_2:
                return 600;
            case EquipmentUpgradeType.HMelee_3:
                return 800;
            case EquipmentUpgradeType.HMelee_4:
                return 1000;
            case EquipmentUpgradeType.HMeleeMax:
                return 0;
            //Rifle Weapon Cases
            case EquipmentUpgradeType.Rifle_1:
                return 400;
            case EquipmentUpgradeType.Rifle_2:
                return 600;
            case EquipmentUpgradeType.Rifle_3:
                return 800;
            case EquipmentUpgradeType.Rifle_4:
                return 1000;
            case EquipmentUpgradeType.RifleMax:
                return 0;
            //Shotgun Weapon Cases
            case EquipmentUpgradeType.Shotgun_1:
                return 400;
            case EquipmentUpgradeType.Shotgun_2:
                return 600;
            case EquipmentUpgradeType.Shotgun_3:
                return 800;
            case EquipmentUpgradeType.Shotgun_4:
                return 1000;
            case EquipmentUpgradeType.ShotgunMax:
                return 0;
        }
    }
}
