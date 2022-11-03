using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will be used for tracking the player's stats during the game
// It will also be used for saving and loading of the game
[System.Serializable]
public class PlayerStatistics
{
    // Stores player's chosen game mode and difficulty
    public GameMode CurrentGameMode;
    public Difficulty CurrentDifficutly;

    // Stores the number of enemies killed in current wave and in total
    public int TotalEnemiesKilled;
    public int CurrentWaveEnemiesKilled;

    public int TotalCreepKilled;
    public int TotalEliteRKilled;
    public int TotalEliteMKilled;
    public int TotalBossKilled;

    // Stores the number of ores obtained in total and currently stored in base
    public int TotalOresObtained;
    public int CurrentOresInBase;

    // Stores the number of essence obtained in total and currently stored in base
    public int TotalEssenceObtained;
    public int CurrentEssenceInBase;

    // Stores the number of boss cores obtained in total and currently stored in base
    public int TotalBossCoresObtained;
    public int CurrentBossCoresInBase;

    // Stores data for the monster spawner script
    public int WavesCleared;
    public int currentWave;

    // Stores data for the timer
    public float TimeLeftToNextWave;

    // Stores data for player health
    public float maxHealth;
    public float currentHealth;

    // Stores data for player base health
    public float maxBaseHealth;
    public float currentBaseHealth;

    // Stores data for player upgrades
    public int playerHealthLevel = 0;
    public int baseLevel = 0;

    public int playerRifleLevel = 0;
    public int playerShotgunLevel = 0;
    public int playerAxeLevel = 0;
    public int playerSwordLevel = 0;

    public float[] playerPos;
    public float[] playerRot;


    public Inventory PlayerInventory;

    public WorldGenData WorldGenSaveData;

    public PlayerStatistics()
    {
        TotalEnemiesKilled = 0;
        CurrentWaveEnemiesKilled = 0;

        TotalCreepKilled = 0;
        TotalEliteRKilled = 0;
        TotalEliteMKilled = 0;
        TotalBossKilled = 0;
        TotalBossKilled = 0;

        TotalOresObtained = 0;
        CurrentOresInBase = 0;

        TotalEssenceObtained = 0;
        CurrentEssenceInBase = 0;

        TotalBossCoresObtained = 0;
        CurrentBossCoresInBase = 0;

        WavesCleared = 0;
        currentWave = 0;

        maxHealth = 0;
        currentHealth = 0;

        playerPos = new float[3];
        playerRot = new float[4];

        PlayerInventory = new Inventory();

        WorldGenSaveData = new WorldGenData();
    }

    public void CalculateTotalEnemiesKilled()
    {
        TotalEnemiesKilled = TotalCreepKilled + TotalEliteRKilled + TotalEliteMKilled + TotalBossKilled;
    }

    public void AddOres(int oresToAdd)
    {
        TotalOresObtained += oresToAdd;
        CurrentOresInBase += oresToAdd;
    }

    public void AddEssence(int essenceToAdd)
    {
        TotalEssenceObtained += essenceToAdd;
        CurrentEssenceInBase += essenceToAdd;
    }

    public void AddCores(int coresToAdd)
    {
        TotalBossCoresObtained += coresToAdd;
        CurrentBossCoresInBase += coresToAdd;
    }

    public void DeductOres(int oresToDeduct)
    {
        CurrentOresInBase -= oresToDeduct;
    }

    public void DeductEssence(int essenceToDeduct)
    {
        CurrentEssenceInBase -= essenceToDeduct;
    }
}
