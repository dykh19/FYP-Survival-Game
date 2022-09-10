using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will be used for tracking the player's stats during the game
// It will also be used for saving and loading of the game
public class PlayerStatistics
{
    // Stores player's chosen game mode and difficulty
    public GameMode currentGameMode;
    public Difficulty currentDifficutly;

    // Stores the number of enemies killed in current wave and in total
    public int TotalEnemiesKilled;
    public int CurrentWaveEnemiesKilled;

    // Stores the number of ores obtained in total and currently stored in base
    public int TotalOresObtained;
    public int CurrentOresInBase;

    // Stores the number of essence obtained in total and currently stored in base
    public int TotalEssenceObtained;
    public int CurrentEssenceInBase;

    // Stores the number of boss cores obtained in total and currently stored in base
    public int TotalBossCoresObtained;
    public int CurrentBossCoresInBase;

    // Stores the number of waves cleared and the current wave
    public int WavesCleared;
    public int CurrentWave;

    // Stores the max and current health of the player
    public float MaxHealth;
    public float CurrentHealth;

    public PlayerStatistics()
    {

    }


}
