using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
    // In order of EASY - NORMAL - HARD
    public static float[] EnemyHealthModifier = { 0.5f, 1.0f, 2.0f };

    public static float[] EnemyAttackModifier = { 0.5f, 1.0f, 2.0f };

    public static float[] EnemyDropModifier = { 2.0f, 1.0f, 0.5f };


    // In order of CREEP - HEAVY ELITE - RANGED ELITE - BOSS
    public static float[] EnemyJunkDropChance = { 0.7f, 1.0f, 1.0f, 2.0f };

    public static float[] EnemyEssenceDropChance = { 0.1f, 0.2f, 0.2f, 0.5f };

    public static float[] EnemyCoreDropChance = { 0f, 0f, 0f, 0.1f };

    public static float[] BaseEnemyHealth = { 70f, 500f, 300f, 1000f };

    public static float[] BaseEnemyDamage = { 20f, 40f, 80f, 150f };
}
