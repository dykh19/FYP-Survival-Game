using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    //Player's Resources
    [Tooltip("Total amount of Ores stored in base")]
    public int TotalOres;

    [Tooltip("Total amount of Essence stored in base")]
    public int TotalEssence;

    [Tooltip("Total amount of Boss Cores the player has")]
    public int TotalBossCores;

    private void Awake()
    {
        TotalOres = 0;
        TotalEssence = 0;
        TotalBossCores = 0;
    }

    public void AddOres(int value)
    {
        TotalOres = TotalOres + value;
    }

    public void DeductOres(int value)
    {
        TotalOres = TotalOres - value;
    }

    public void AddEssence(int value)
    {
        TotalEssence = TotalEssence + value;
    }

    public void DeductEssence(int value)
    {
        TotalEssence = TotalEssence - value;
    }

    public void AddBossCore(int value)
    {
        TotalBossCores = TotalBossCores + value;
    }

    public void DeductBossCore(int value)
    {
        TotalBossCores = TotalBossCores - value;
    }
}
