using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Monster_Spawner : MonoBehaviour
{
    public int waveNumber = 0;

    //Monster Numbers for elite Ranged
    private int eliteRSpawn = 0;
    public int eliteRSpawned = 0;
    public int eliteRKilled = 0;
    private int eliteRKilledThisInstance;
    private int eliteRKilledFromSave;

    private int eliteMSpawn = 0;
    public int eliteMSpawned = 0;
    public int eliteMKilled = 0;
    private int eliteMKilledThisInstance;
    private int eliteMKilledFromSave;

    private int bossSpawn = 0;
    public int bossSpawned = 0;
    public int bossKilled = 0;
    private int bossKilledThisInstance;
    private int bossKilledFromSave;

    private int creepCountToEndWave = 0;
    public int creepSpawned = 0;
    public int creepKilled = 0;
    private int creepKilledThisInstance;
    private int creepKilledFromSave;

    public bool isWave = false;
    public bool inWave = false;
    public bool IsLoadedGame = false;

    public GameObject[] creeps = new GameObject[3];
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

    public TMP_Text EnemiesLeftText;


    // Start is called before the first frame update

    private void Awake()
    {
        //Make sure delegate is only added once
        GameManager.Instance.LoadData -= LoadSpawnerData;
        GameManager.Instance.SaveData -= SaveSpawnerData;
        GameManager.Instance.LoadData += LoadSpawnerData;
        GameManager.Instance.SaveData += SaveSpawnerData;
    }
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
        EnemiesLeftText = GameObject.Find("EnemiesLeftText").GetComponent<TMP_Text>();

        if (!IsLoadedGame)
        {
            WaveTimerManager.Instance.StartTimer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inWave)
        {
            EnemiesLeftText.text = "Enemies Left This Wave: " + (creepCountToEndWave - creepKilledThisInstance);
        }

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

            if (creepKilledThisInstance == creepCountToEndWave && eliteRKilled == eliteRSpawned && eliteMKilled == eliteMSpawned && bossKilled == bossSpawned)
            {
                EndWave();
            }
        }
        if (!isWave && !inWave && waveNumber == GameManager.Instance.WaveCountToWin + 1)
        {
            GameManager.Instance.WinGame();
        }

        if(!isWave && !inWave && ((creepSpawned == 0) || (creepCountToEndWave != 0 && (creepSpawned == creepKilled))))
        {
            OpenWorldSpawn();
        }
    }

    //Spawning mechanic when !isWave
    public void OpenWorldSpawn()
    {
        creepCountToEndWave = 30; //Change value here
        creepSpawned = 0;
        creepKilled = 0;

        for(int i = 0; i < creepCountToEndWave; i++)
        {
            spawnCreep(mapxPosMin, mapxPosMax, mapzPosMin, mapzPosMax);
        }
        
    }

    //Function to spawn Creeps (Mainly for waves)
    public void spawnCreep(float xPosMin, float xPosMax, float zPosMin, float zPosMax)
    {
        var rayOrigin = new Vector3(Random.Range(xPosMin, xPosMax), 100f, Random.Range(zPosMin, zPosMax));
        var ray = new Ray(rayOrigin, Vector3.down);
        
        int creepIndex;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Randomly chooses the design of creep to spawn from 4 prefabs
            creepIndex = Random.Range(0, creeps.Length);
            GameObject newCreep = Instantiate(creeps[creepIndex]);
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
        if (!IsLoadedGame)
        {
            creepSpawned = 0;
            creepKilled = 0;
            eliteRSpawned = 0;
            eliteRKilled = 0;
            eliteMSpawned = 0;
            eliteMKilled = 0;
            bossSpawned = 0;
            bossKilled = 0;
            creepCountToEndWave = Mathf.FloorToInt(10 * (Mathf.Pow(waveNumber, 0.5f)));
        }
        else
        {
            creepCountToEndWave = Mathf.FloorToInt(10 * (Mathf.Pow(waveNumber, 0.5f))) - creepKilledFromSave;
        }

        eliteRSpawn = 0; //To change based on specs
        eliteMSpawn = 0;

        
        isWave = true;
        inWave = true;
        WaveTimerManager.Instance.IncomingWave();
        GameManager.Instance.PlayerStats.currentWave = waveNumber;

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject); //Destroys all existing mobs
        }

        for (int i = 0; i < eliteRSpawn; i++)
        {
            spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
            /*~~~~~~~~Add spawn timer here if want~~~~~~~~*/
        }
        for (int i = 0; i < creepCountToEndWave; i++)
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
        //Store Data into PlayerStatistics
        GameManager.Instance.PlayerStats.CalculateTotalEnemiesKilled();
        GameManager.Instance.PlayerStats.WavesCleared = waveNumber;

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
        creepKilledFromSave = 0;
        creepKilledThisInstance = 0;
        WaveTimerManager.Instance.ShowTimer();
        WaveTimerManager.Instance.StartNewWaveTimer();
        GameManager.Instance.PlayerStats.currentWave = waveNumber;
        EnemiesLeftText.text = "";
        IsLoadedGame = false;
    }

    //Function to start the next wave
    public void NextWave()
    {
        isWave = true;
        inWave = true;

        if (!IsLoadedGame)
        {
            creepSpawned = 0;
            creepKilled = 0;
            eliteRSpawned = 0;
            eliteRKilled = 0;
            eliteMSpawned = 0;
            eliteMKilled = 0;
            bossSpawned = 0;
            bossKilled = 0;
            creepCountToEndWave = Mathf.FloorToInt(10 * (Mathf.Pow(waveNumber, 0.5f)));
        }
        else
        {
            creepCountToEndWave = Mathf.FloorToInt(10 * (Mathf.Pow(waveNumber, 0.5f))) - creepKilledFromSave;
        }

        //creepSpawn = Mathf.FloorToInt(10 * (Mathf.Pow(waveNumber, 0.5f)));

        WaveTimerManager.Instance.IncomingWave();
        if(creepCountToEndWave > 100)
        {
            creepCountToEndWave = 100;
        }
        //eliteRSpawn = eliteRSpawn + 2; //replace the 2 with the formula

        if (!IsLoadedGame)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        

        for (int i = 0; i < eliteRSpawn; i++)
        {
            spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
        for (int i = 0; i < creepCountToEndWave; i++)
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
        if (isWave && inWave)
        {
            creepKilledThisInstance++;
        }
        
        GameManager.Instance.PlayerStats.TotalCreepKilled += 1;
    }

    public void eliteRDie()
    {
        eliteRKilled += 1;
        GameManager.Instance.PlayerStats.TotalEliteRKilled += 1;
    }

    public void eliteMDie()
    {
        eliteMKilled += 1;
        GameManager.Instance.PlayerStats.TotalEliteMKilled += 1;
    }

    public void bossDie()
    {
        bossKilled += 1;
        GameManager.Instance.PlayerStats.TotalBossKilled += 1;
    }

    public void LoadSpawnerData()
    {
        waveNumber = GameManager.Instance.PlayerStats.currentWave;
        isWave = GameManager.Instance.PlayerStats.isWave;
        inWave = GameManager.Instance.PlayerStats.inWave;
        creepCountToEndWave = GameManager.Instance.PlayerStats.creepCountToEndWave;
        creepKilledFromSave = GameManager.Instance.PlayerStats.creepKilledThisWave;
        eliteRSpawn = GameManager.Instance.PlayerStats.eliteRCountToEndWave;
        eliteRKilled = GameManager.Instance.PlayerStats.eliteRKilledThisWave;
        eliteMSpawn = GameManager.Instance.PlayerStats.eliteMCountToEndWave;
        eliteMKilled = GameManager.Instance.PlayerStats.eliteMKilledThisWave;
        bossSpawn = GameManager.Instance.PlayerStats.bossCountToEndWave;
        bossKilled = GameManager.Instance.PlayerStats.bossKilledThisWave;
        IsLoadedGame = true;
        if (isWave == true && inWave == true)
        {
            Start();
            if (waveNumber == 1)
            {
                StartWave();
            }
            else
            {
                NextWave();
            }
        }
        else
        {
            Start();
            OpenWorldSpawn();
            WaveTimerManager.Instance.StartTimer();
        }
        //Debug.Log("Loaded Spawner Data");
    }

    public void SaveSpawnerData()
    {
        GameManager.Instance.PlayerStats.currentWave = waveNumber;
        GameManager.Instance.PlayerStats.isWave = isWave;
        GameManager.Instance.PlayerStats.inWave = inWave;
        GameManager.Instance.PlayerStats.creepCountToEndWave = creepCountToEndWave;
        GameManager.Instance.PlayerStats.creepKilledThisWave = creepKilledThisInstance + creepKilledFromSave;
        GameManager.Instance.PlayerStats.eliteRCountToEndWave = eliteRSpawn;
        GameManager.Instance.PlayerStats.eliteRKilledThisWave = eliteRKilled;
        GameManager.Instance.PlayerStats.eliteMCountToEndWave = eliteMSpawn;
        GameManager.Instance.PlayerStats.eliteMKilledThisWave = eliteMKilled;
        GameManager.Instance.PlayerStats.bossCountToEndWave = bossSpawn;
        GameManager.Instance.PlayerStats.bossKilledThisWave = bossKilled;
        GameManager.Instance.LoadData -= LoadSpawnerData;
        //Debug.Log("Saved Spawner Data");
    }
}
