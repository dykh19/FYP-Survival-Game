using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawner : MonoBehaviour
{
    public int waveNumber = 0;

    //Monster Numbers for elite Ranged
    private int eliteRSpawn = 0;
    public int eliteRSpawned = 0;
    public int eliteRKilled = 0;

    private int eliteMSpawn = 0;
    public int eliteMSpawned = 0;
    public int eliteMKilled = 0;

    private int bossSpawn = 0;
    public int bossSpawned = 0;
    public int bossKilled = 0;

    private int creepSpawn = 0;
    public int creepSpawned = 0;
    public int creepKilled = 0;

    public bool isWave = false;
    public bool inWave = false;

    public GameObject creep;
    public GameObject eliteR;
    public GameObject eliteM;
    public GameObject boss;
    public GameObject baseObj;
    public GameObject map;

    //Declaration of positional variables
    public float xPos;
    public float basexPosMin;
    public float basexPosMax;
    public float mapxPosMin;
    public float mapxPosMax;
    public float yPos;
    public float basezPosMin;
    public float basezPosMax;
    public float mapzPosMin;
    public float mapzPosMax;
    public float zPos;

    public Vector3 enemySpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        baseObj = GameObject.FindWithTag("Base");
        basexPosMin = baseObj.transform.position.x - 10; //replace with regards to base size afterwards
        basexPosMax = baseObj.transform.position.x + 10; //replace with regards to base size afterwards
        basezPosMin = baseObj.transform.position.z - 10; //replace with regards to base size afterwards
        basezPosMax = baseObj.transform.position.z + 10; //replace with regards to base size afterwards

        map = GameObject.FindWithTag("Ground");
        mapxPosMin = map.transform.position.x - 10; //replace with regards to map size afterwards
        mapxPosMax = map.transform.position.x + 10; //replace with regards to map size afterwards
        mapzPosMin = map.transform.position.z - 10; //replace with regards to map size afterwards
        mapzPosMax = map.transform.position.z + 10; //replace with regards to map size afterwards

        spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (isWave)
        {
            if (waveNumber < 1 && !inWave)
            {
                StartWave();
            }
            else if (waveNumber >= 1 && !inWave)
            {
                NextWave();
            }

            if (eliteRKilled == eliteRSpawned && eliteMKilled == eliteMSpawned && bossKilled == bossSpawned)
            {
                EndWave();
            }
        }
        if(!isWave && !inWave && (creepSpawned == 0))
        {
            OpenWorldSpawn();
        }
    }

    //Spawning mechanic when !isWave
    public void OpenWorldSpawn()
    {
        creepSpawn = 0;
        creepKilled = 0;

        for(int i = 0; i < creepSpawn; i++)
        {
            spawnCreep(mapxPosMin, mapxPosMax, mapzPosMin, mapzPosMax);
        }
        
    }

    //Function to spawn Creeps (Mainly for waves)
    public void spawnCreep(float xPosMin, float xPosMax, float zPosMin, float zPosMax)
    {
        GameObject newCreep = Instantiate(creep, new Vector3(Random.Range(xPosMin, xPosMax), yPos, Random.Range(zPosMin, zPosMax)), Quaternion.identity);
        newCreep.transform.parent = GameObject.Find("Spawner").transform;
        creepSpawned += 1;
    }

    //Function to spawn Elite Ranged Monsters
    public void spawnEliteR(float xPosMin, float xPosMax, float zPosMin, float zPosMax)
    {
        GameObject newEliteR = Instantiate(eliteR, new Vector3(Random.Range(xPosMin, xPosMax), yPos, Random.Range(zPosMin, zPosMax)), Quaternion.identity);
        newEliteR.transform.parent = GameObject.Find("Spawner").transform;
        eliteRSpawned += 1;
    }

    public void spawnEliteM(float xPosMin, float xPosMax, float zPosMin, float zPosMax)
    {
        GameObject newEliteM = Instantiate(eliteM, new Vector3(Random.Range(xPosMin, xPosMax), yPos, Random.Range(zPosMin, zPosMax)), Quaternion.identity);
        newEliteM.transform.parent = GameObject.Find("Spawner").transform;
        eliteMSpawned += 1;
    }

    //Function to start the wave and spawn monsters
    public void StartWave()
    {
        waveNumber = 0;
        eliteRSpawn = 2; //To change based on specs
        eliteRKilled = 0;
        isWave = true;
        inWave = true;

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < eliteRSpawn; i++)
        {
            spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
            /*~~~~~~~~Add spawn timer here if want~~~~~~~~*/
        }
    }

    //Function to reset variable at end of wave
    public void EndWave()
    {
        inWave = false;
        waveNumber += 1;
        eliteRSpawned = 0;
        eliteRKilled = 0;
        eliteMSpawned = 0;
        eliteMKilled = 0;
        bossSpawned = 0;
        bossKilled = 0;
        isWave = false;
    }

    //Function to start the next wave
    public void NextWave()
    {
        waveNumber += 1;
        inWave = true;
        eliteRSpawn = eliteRSpawn + 2; //replace the 2 with the formula

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < eliteRSpawn; i++)
        {
            spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
    }

    public void eliteRDie()
    {
        eliteRKilled += 1;
    }

    public void eliteMDie()
    {
        eliteMKilled += 1;
    }

    public void bossDie()
    {
        bossKilled += 1;
    }
}
