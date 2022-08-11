using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawner : MonoBehaviour
{

    [SerializeField]
    private GameObject creep;
    [SerializeField]
    private GameObject eliteRange;
    [SerializeField]
    private GameObject eliteMelee;
    [SerializeField]
    private GameObject boss;
    [SerializeField]
    private GameObject baseObject;

    private float creepInterval = 3.5f;
    private float eliteInterval = 5f;
    private float bossInterval = 10f;

    public float xPos;
    public float xPosMin;
    public float xPosMax;
    public float yPos;
    public float zPosMin;
    public float zPosMax;
    public float zPos;
    public GameObject[] eRangeCount;
    public int rangeEliteCount = 0;
    public int meleeEliteCount = 0;
    public int bossCount = 0;

    void Start()
    {
        baseObject = GameObject.FindWithTag("base");
        xPosMin = baseObject.transform.position.x - 50;
        xPosMax = baseObject.transform.position.x + 50;
        zPosMin = baseObject.transform.position.z - 50;
        zPosMax = baseObject.transform.position.z + 50;

        StartCoroutine(spawnEnemy(creepInterval, creep));
        StartCoroutine(spawnEnemy(eliteInterval, eliteRange, 10, eRangeCount.Length)); //will stop spawning enemies at maximum and will not continue to spawn after enemy is removed???

    }

    void Update()
    {
        eRangeCount = GameObject.FindGameObjectsWithTag("eRange");
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(xPosMin, xPosMax), yPos, Random.Range(zPosMin, zPosMax)), Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy, int maximum, int currentCount)
    {
        if(currentCount < maximum)
        {
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(xPosMin, xPosMax), yPos, Random.Range(zPosMin, zPosMax)), Quaternion.identity);
            StartCoroutine(spawnEnemy(eliteInterval, eliteRange, 10, eRangeCount.Length));
        }
        else
        {
            StartCoroutine(spawnEnemy(eliteInterval, eliteRange, 10, eRangeCount.Length));
        }
    }





























    /*public GameObject baseObject;
    public GameObject creep;
    public GameObject eliteRange;
    public GameObject eliteMelee;
    public GameObject boss;

    public float xPos;
    public float xPosMin;
    public float xPosMax;
    public float yPos;
    public float zPosMin;
    public float zPosMax;
    public float zPos;

    public int creepCount;
    public int rangeECount;
    public int meleeECount;
    public int bossCount;
    public bool isWave;

    // Start is called before the first frame update
    void Start()
    {
        creepCount = 0;
        baseObject = GameObject.FindWithTag("base");
        xPosMin = baseObject.transform.position.x - 50;
        xPosMax = baseObject.transform.position.x + 50;
        zPosMin = baseObject.transform.position.z - 50;
        zPosMax = baseObject.transform.position.z + 50;
    }

    void Update()
    {
        if(isWave == true)
        {
            StartCoroutine(CreepSpawn());
        }
    }

    IEnumerator CreepSpawn()
    {
        while (creepCount < 50)
        {
            //xPos and zPos will be determined by where the base is.
            xPos = Random.Range(xPosMin, xPosMax + 1);
            zPos = Random.Range(zPosMin, zPosMax + 1);

            Instantiate(creep, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.05f);
            creepCount += 1;
        }
    }
    
    IEnumerator RangeESpawn()
    {
        while (rangeECount < 10)
        {
            //xPos and zPos will be determined by where the base is.
            xPos = Random.Range(xPosMin, xPosMax + 1);
            zPos = Random.Range(zPosMin, zPosMax + 1);

            Instantiate(eliteRange, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.01f);
            rangeECount += 1;
        }
    }

    IEnumerator RangeMSpawn()
    {
        while (meleeECount < 10)
        {
            //xPos and zPos will be determined by where the base is.
            xPos = Random.Range(xPosMin, xPosMax + 1);
            zPos = Random.Range(zPosMin, zPosMax + 1);

            Instantiate(eliteMelee, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            meleeECount += 1;
        }
    }

    IEnumerator BossSpawn()
    {
        while (bossCount < 1)
        {
            //xPos and zPos will be determined by where the base is.
            xPos = Random.Range(xPosMin, xPosMax + 1);
            zPos = Random.Range(zPosMin, zPosMax + 1);

            Instantiate(boss, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            bossCount += 1;
        }
    }*/
}
