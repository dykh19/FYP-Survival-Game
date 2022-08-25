using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Spawn Manager is incharge of spawning of objects such as player, base and enemies

// Possibly Redundant
public class SpawnManager : MonoBehaviour
{
    public GameObject basePrefab;
    public GameObject player;


    // Start is called before the first frame update
    private void Start()
    {
        
    }

    void SpawnEnemy()
    {
        if(GameManager.Instance.CurrentDifficulty == Difficulty.EASY)
        {
            //Spawn easy mobs
        }
    }
}
