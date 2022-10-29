using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class Monster_Spawner : MonoBehaviour
{
    public int waveNumber = 0;
    public int monsterCap = 0;

    //Monster Numbers for elite Ranged
    public int eliteRCountToEndWave = 0;
    public int eliteRSpawned = 0;
    public int eliteRKilled = 0;
/*    private int eliteRKilledThisInstance;
    private int eliteRKilledFromSave;*/

    public int eliteMCountToEndWave = 0;
    public int eliteMSpawned = 0;
    public int eliteMKilled = 0;
    /*    private int eliteMKilledThisInstance;
        private int eliteMKilledFromSave;*/

    public int bossCountToEndWave = 0;
    public int bossSpawned = 0;
    public int bossKilled = 0;
/*    private int bossKilledThisInstance;
    private int bossKilledFromSave;*/

    private int creepCountToEndWave = 0;
    public int creepSpawned = 0;
    public int creepKilled = 0;
/*    private int creepKilledThisInstance;
    private int creepKilledFromSave;*/

    public bool isWave = false;
    public bool inWave = false;
/*    public bool IsLoadedGame = false;*/

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

    public Button saveGameButton;

    public GameObject healthPickupPrefab;

    // Start is called before the first frame update

    private void Awake()
    {
        //Make sure delegate is only added once
        GameManager.Instance.LoadData -= LoadSpawnerData;
        GameManager.Instance.SaveData -= SaveSpawnerData;
        GameManager.Instance.LoadData += LoadSpawnerData;
        GameManager.Instance.SaveData += SaveSpawnerData;

        saveGameButton = GameObject.Find("SaveGameButton").GetComponent<Button>();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (inWave)
        {
            EnemiesLeftText.text = "Enemies Left This Wave: " + (creepCountToEndWave - creepKilled);
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

            if (creepKilled == creepCountToEndWave && eliteRKilled == eliteRSpawned && eliteMKilled == eliteMSpawned && bossKilled == bossSpawned)
            {
                EndWave();
            }
        }
        if (!isWave && !inWave && GameManager.Instance.CurrentGameMode == GameMode.NORMAL && waveNumber == GameManager.Instance.WaveCountToWin + 1)
        {
            GameManager.Instance.WinGame();
        }

        //if(!isWave && !inWave && ((creepSpawned == 0) || (creepCountToEndWave != 0 && (creepSpawned == creepKilled))))
        //{
        //    OpenWorldSpawn();
        //}

        if(!isWave && !inWave && (bossSpawned == 0 || (bossCountToEndWave != 0 && (bossSpawned == bossKilled))))
        {
            OpenWorldSpawn();
        }
    }

    //Spawning mechanic when !isWave
    public void OpenWorldSpawn()
    {
        creepCountToEndWave = 30; //Change value here (Previously 30)
        creepSpawned = 0;
        creepKilled = 0;
        //eliteRCountToEndWave = 2;
        //eliteMCountToEndWave = 2;
        //bossCountToEndWave = 1;

        for(int i = 0; i < creepCountToEndWave; i++)
        {
            spawnCreep(mapxPosMin, mapxPosMax, mapzPosMin, mapzPosMax);
        }
        //for(int i = 0; i < eliteRCountToEndWave; i++)
        //{
        //    spawnEliteR(mapxPosMin, mapxPosMax, mapzPosMin, mapzPosMax);
        //}
        //for(int i = 0; i < eliteMCountToEndWave; i++)
        //{
        //    spawnEliteM(mapxPosMin, mapxPosMax, mapzPosMin, mapzPosMax);
        //}
        //(int i = 0; i < bossCountToEndWave; i++)
        //{
        //    spawnBoss(mapxPosMin, mapxPosMax, mapzPosMin, mapzPosMax);
        //}
    }

    //Function to spawn Creeps (Mainly for waves)
    public void spawnCreep(float xPosMin, float xPosMax, float zPosMin, float zPosMax)
    {
        bool validSpawn = false;
        float x = 0f;
        float z = 0f;
        // Generate until position is outside of radius around base then spawn
        while (!validSpawn)
        {
            x = Random.Range(xPosMin, xPosMax);
            z = Random.Range(zPosMin, zPosMax);
            if ((new Vector3(x, baseObj.transform.position.y, z) - baseObj.transform.position).sqrMagnitude >= 25f * 25f)
            {
                validSpawn = true;
            }
        }
        var rayOrigin = new Vector3(x, 100f, z);
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
        bool validSpawn = false;
        float x = 0f;
        float z = 0f;
        // Generate until position is outside of radius around base then spawn
        while (!validSpawn)
        {
            x = Random.Range(xPosMin, xPosMax);
            z = Random.Range(zPosMin, zPosMax);
            if ((new Vector3(x, baseObj.transform.position.y, z) - baseObj.transform.position).sqrMagnitude >= 25f * 25f)
            {
                validSpawn = true;
            }
        }
        var rayOrigin = new Vector3(x, 100f, z);
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
        bool validSpawn = false;
        float x = 0f;
        float z = 0f;
        // Generate until position is outside of radius around base then spawn
        while (!validSpawn)
        {
            x = Random.Range(xPosMin, xPosMax);
            z = Random.Range(zPosMin, zPosMax);
            if ((new Vector3(x, baseObj.transform.position.y, z) - baseObj.transform.position).sqrMagnitude >= 25f * 25f)
            {
                validSpawn = true;
            }
        }
        var rayOrigin = new Vector3(x, 100f, z);
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
            eliteMSpawned += 1;
        }
    }

    public void spawnBoss(float xPosMin, float xPosMax, float zPosMin, float zPosMax)
    {
        bool validSpawn = false;
        float x = 0f;
        float z = 0f;
        // Generate until position is outside of radius around base then spawn
        while (!validSpawn)
        {
            x = Random.Range(xPosMin, xPosMax);
            z = Random.Range(zPosMin, zPosMax);
            if ((new Vector3(x, baseObj.transform.position.y, z) - baseObj.transform.position).sqrMagnitude >= 25f * 25f)
            {
                validSpawn = true;
            }
        }
        var rayOrigin = new Vector3(x, 100f, z);
        var ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject newBoss = Instantiate(boss);
            newBoss.transform.position = hit.point + hit.normal;
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(newBoss.transform.position, out closestHit, 500, 1))
            {
                newBoss.transform.position = closestHit.position;
                newBoss.AddComponent<NavMeshAgent>();
            }
            newBoss.transform.parent = GameObject.Find("Spawner").transform;
            bossSpawned += 1;
        }
    }

    //Function to start the wave and spawn monsters
    public void StartWave()
    {
        waveNumber = 1;

        creepSpawned = 0;
        creepKilled = 0;
        eliteRSpawned = 0;
        eliteRKilled = 0;
        eliteMSpawned = 0;
        eliteMKilled = 0;
        bossSpawned = 0;
        bossKilled = 0;
        creepCountToEndWave = Mathf.FloorToInt(10 * (Mathf.Pow(waveNumber, 0.5f)));
        eliteRCountToEndWave = monsterCap - bossCountToEndWave - eliteMCountToEndWave;
        eliteMCountToEndWave = Mathf.CeilToInt((monsterCap - bossCountToEndWave) * 0.7258244816f);
        if(waveNumber % 5 == 0)
        {
            bossCountToEndWave += 1;
        }
        
        isWave = true;
        inWave = true;
        WaveTimerManager.Instance.IncomingWave();
        GameManager.Instance.PlayerStats.currentWave = waveNumber;
        saveGameButton.interactable = false;
        saveGameButton.GetComponentInChildren<TMP_Text>().SetText("Cannot Save During Wave");

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject); //Destroys all existing mobs
        }

        for (int i = 0; i < eliteRCountToEndWave; i++)
        {
            spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
            /*~~~~~~~~Add spawn timer here if want~~~~~~~~*/
        }
        for (int i = 0; i < creepCountToEndWave; i++)
        {
            spawnCreep(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
        for (int i = 0; i < eliteMCountToEndWave; i++)
        {
            spawnEliteM(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
        for(int i = 0; i < bossCountToEndWave; i++)
        {
            spawnBoss(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
    }

    //Function to reset variable at end of wave
    public void EndWave()
    {
        //Store Data into PlayerStatistics
        GameManager.Instance.PlayerStats.CalculateTotalEnemiesKilled();
        GameManager.Instance.PlayerStats.WavesCleared = waveNumber;
        saveGameButton.interactable = true;
        saveGameButton.GetComponentInChildren<TMP_Text>().SetText("Save Game");

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
        monsterCap = Mathf.FloorToInt(10 * Mathf.Pow(waveNumber, 0.4f)) - 10;

        WaveTimerManager.Instance.ShowTimer();
        WaveTimerManager.Instance.StartNewWaveTimer();
        GameManager.Instance.PlayerStats.currentWave = waveNumber;
        EnemiesLeftText.text = "";
    }

    //Function to start the next wave
    public void NextWave()
    {
        isWave = true;
        inWave = true;

        creepSpawned = 0;
        creepKilled = 0;
        eliteRSpawned = 0;
        eliteRKilled = 0;
        eliteMSpawned = 0;
        eliteMKilled = 0;
        bossSpawned = 0;
        bossKilled = 0;
        creepCountToEndWave = Mathf.FloorToInt(10 * (Mathf.Pow(waveNumber, 0.5f)));
        eliteMCountToEndWave = Mathf.CeilToInt((monsterCap - bossCountToEndWave) * 0.7258244816f);
        eliteRCountToEndWave = Mathf.FloorToInt(monsterCap - bossCountToEndWave - eliteMCountToEndWave);
        if (waveNumber % 5 == 0)
        {
            bossCountToEndWave += 1;
        }

        WaveTimerManager.Instance.IncomingWave();
        saveGameButton.interactable = false;
        saveGameButton.GetComponentInChildren<TMP_Text>().SetText("Cannot Save During Wave");

        if (creepCountToEndWave > 100)
        {
            creepCountToEndWave = 100;
        }
        //eliteRSpawn = eliteRSpawn + 2; //replace the 2 with the formula

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        

        for (int i = 0; i < eliteRCountToEndWave; i++)
        {
            spawnEliteR(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
        for (int i = 0; i < creepCountToEndWave; i++)
        {
            spawnCreep(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
        for (int i = 0; i < eliteMCountToEndWave; i++)
        {
            spawnEliteM(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
        for (int i = 0; i < bossCountToEndWave; i++)
        {
            spawnBoss(basexPosMin, basexPosMax, basezPosMin, basezPosMax);
        }
    }

    public void creepDie()
    {
        creepKilled += 1;
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

        Start();
        OpenWorldSpawn();
        WaveTimerManager.Instance.StartTimer();
        //Debug.Log("Loaded Spawner Data");
    }

    public void SaveSpawnerData()
    {
        GameManager.Instance.PlayerStats.currentWave = waveNumber;
        GameManager.Instance.LoadData -= LoadSpawnerData;
        //Debug.Log("Saved Spawner Data");
    }
}
