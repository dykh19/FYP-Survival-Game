using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Resource Manager keeps track of the player's resources stored in the base
public class ResourceManager : MonoBehaviour
{
    //Player's Resources
    [Tooltip("Total amount of Ores stored in base")]
    public int TotalOres;

    [Tooltip("Total amount of Essence stored in base")]
    public int TotalEssence;

    [Tooltip("Total amount of Boss Cores the player has")]
    public int TotalBossCores;

    //When the resource manager is created, set all resources to 0
    private void Awake()
    {
        TotalOres = 0;
        TotalEssence = 0;
        TotalBossCores = 0;
    }

    //Call this function to add ores to the Total Ores count
    public void AddOres(int value)
    {
        TotalOres = TotalOres + value;
    }

    //Call this function to deduct ores from the Total Ores count
    public void DeductOres(int value)
    {
        TotalOres = TotalOres - value;
    }

    //Call this function to add essence to the Total Essence count
    public void AddEssence(int value)
    {
        TotalEssence = TotalEssence + value;
    }

    //Call this function to deduct essence to the Total Essence count
    public void DeductEssence(int value)
    {
        TotalEssence = TotalEssence - value;
    }

    //Call this function to add boss core to the Total Boss Core count
    public void AddBossCore(int value)
    {
        TotalBossCores = TotalBossCores + value;
    }

    //Call this function to deduct boss core from the Total Boss Core count
    public void DeductBossCore(int value)
    {
        TotalBossCores = TotalBossCores - value;
    }
}
