using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public float mapSize;

    public Vector3 enemySpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        baseObj = GameObject.FindWithTag("Base");
        basexPosMin = baseObj.transform.position.x - mapSize; //replace with regards to base size afterwards
        basexPosMax = baseObj.transform.position.x + mapSize; //replace with regards to base size afterwards
        basezPosMin = baseObj.transform.position.z - mapSize; //replace with regards to base size afterwards
        basezPosMax = baseObj.transform.position.z + mapSize; //replace with regards to base size afterwards

        map = GameObject.FindWithTag("Ground");
        mapxPosMin = map.transform.position.x - mapSize; //replace with regards to map size afterwards
        mapxPosMax = map.transform.position.x + mapSize; //replace with regards to map size afterwards
        mapzPosMin = map.transform.position.z - mapSize; //replace with regards to map size afterwards
        mapzPosMax = map.transform.position.z + mapSize; //replace with regards to map size afterwards

        yPos = map.transform.position.y;

        //spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
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

            if (creepKilled == creepSpawned && eliteRKilled == eliteRSpawned && eliteMKilled == eliteMSpawned && bossKilled == bossSpawned)
            {
                EndWave();
            }
        }
        if(!isWave && !inWave && ((creepSpawned == 0) || (creepSpawn != 0 && (creepSpawned == creepKilled))))
        {
            OpenWorldSpawn();
        }
    }

    //Spawning mechanic when !isWave
    public void OpenWorldSpawn()
    {
        creepSpawn = 30; //Change value here
        creepSpawned = 0;
        creepKilled = 0;

        for(int i = 0; i < creepSpawn; i++)
        {
            spawnCreep(mapxPosMin, mapxPosMax, mapzPosMin, mapzPosMax);
        }
        
    }

    //Function to spawn Creeps (Mainly for waves)
    public void spawnCreep(float xPosMin, float xPosMax, float zPosMin, float zPosMax)
    {
        var rayOrigin = new Vector3(Random.Range(xPosMin, xPosMax), 100f, Random.Range(zPosMin, zPosMax));
        var ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject newCreep = Instantiate(creep);
            newCreep.transform.position = hit.point + hit.normal;
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(newCreep.transform.position, out closestHit, 500, 1))
            {
                newCreep.transform.position = closestHit.position;
                newCreep.AddComponent<NavMeshAgent>();
            }
            newCreep.transform.parent = GameObject.Find("Spawner").transform;
            creepSpawned += 1;
        }
    }

    //Function to spawn Elite Ranged Monsters
    public void spawnEliteR(float xPosMin, float xPosMax, float zPosMin, float zPosMax)
    {
        var rayOrigin = new Vector3(Random.Range(xPosMin, xPosMax), 100f, Random.Range(zPosMin, zPosMax));
        var ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject newEliteR = Instantiate(eliteR);
            newEliteR.transform.position = hit.point + hit.normal;
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(newEliteR.transform.position, out closestHit, 500, 1))
            {
                newEliteR.transform.position = closestHit.position;
                newEliteR.AddComponent<NavMeshAgent>();
            }
            newEliteR.transform.parent = GameObject.Find("Spawner").transform;
            eliteRSpawned += 1;
        }
    }

    public void spawnEliteM(float xPosMin, float xPosMax, float zPosMin, float zPosMax)
    {
        var rayOrigin = new Vector3(Random.Range(xPosMin, xPosMax), 100f, Random.Range(zPosMin, zPosMax));
        var ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject newEliteM = Instantiate(eliteM);
            newEliteM.transform.position = hit.point + hit.normal;
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(newEliteM.transform.position, out closestHit, 500, 1))
            {
                newEliteM.transform.position = closestHit.position;
                newEliteM.AddComponent<NavMeshAgent>();
            }
            newEliteM.transform.parent = GameObject.Find("Spawner").transform;
            eliteRSpawned += 1;
        }
    }

    //Function to start the wave and spawn monsters
    public void StartWave()
    {
        waveNumber = 1;
        eliteRSpawn = 0; //To change based on specs
        eliteRKilled = 0;
        eliteMSpawn = 0;
        eliteMKilled = 0;
        creepSpawn = Mathf.FloorToInt(10 * (Mathf.Pow(waveNumber, 0.5f)));
        creepSpawned = 0;
        creepKilled = 0;
        isWave = true;
        inWave = true;
        WaveTimerManager.Instance.HideTimer();

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject); //Destroys all existing mobs
        }

        for (int i = 0; i < eliteRSpawn; i++)
        {
            spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
            /*~~~~~~~~Add spawn timer here if want~~~~~~~~*/
        }
        for (int i = 0; i < creepSpawn; i++)
        {
            spawnCreep(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
        for (int i = 0; i < eliteMSpawn; i++)
        {
            spawnEliteM(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
    }

    //Function to reset variable at end of wave
    public void EndWave()
    {
        inWave = false;
        waveNumber += 1;
        creepKilled = 0;
        creepSpawned = 0;
        eliteRSpawned = 0;
        eliteRKilled = 0;
        eliteMSpawned = 0;
        eliteMKilled = 0;
        bossSpawned = 0;
        bossKilled = 0;
        isWave = false;
        WaveTimerManager.Instance.ShowTimer();
        WaveTimerManager.Instance.StartNewWaveTimer();
    }

    //Function to start the next wave
    public void NextWave()
    {
        isWave = true;
        inWave = true;
        creepSpawned = 0;
        creepSpawn = Mathf.FloorToInt(10 * (Mathf.Pow(waveNumber, 0.5f)));
        WaveTimerManager.Instance.HideTimer();
        if(creepSpawn > 100)
        {
            creepSpawn = 100;
        }
        //eliteRSpawn = eliteRSpawn + 2; //replace the 2 with the formula

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < eliteRSpawn; i++)
        {
            spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
        for (int i = 0; i < creepSpawn; i++)
        {
            spawnCreep(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
        for (int i = 0; i < eliteMSpawn; i++)
        {
            spawnEliteM(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
    }

    public void creepDie()
    {
        creepKilled += 1;
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
