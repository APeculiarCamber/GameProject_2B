using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    PointTracker pt;
    [Header("Difficulty Deltas")]
    [Space]
    [SerializeField]
    float difficultyIncrFrequency = 0f;

    [Space]
    [SerializeField]
    float spawnDecrease = 0.0f; //the amount of decrement that is applied to rates (rates depend on what entity spawned)
    [SerializeField]
    float spawnDecreaseDelta = 0f;
    [SerializeField]
    float maxSpawnDecrease = 0f;

    float timer = 0f;
    [Space]
    [SerializeField]
    float enemySpeed;
    [SerializeField]
    float enemySpeedDelta;
    [SerializeField]
    float maxEnemySpeed;

    float spawnRate = 0;
    float spawnTimer = 0f;

    [Space]
    [SerializeField]
    GameObject enemy;
    [SerializeField]
    [Space]
    Transform[] enemyLanes;
    bool[] activeLanes = { true, true, true, true };  //a bool array handling what lanes are active
    
    // Start is called before the first frame update
    void Start()
    {
        pt = FindObjectOfType<PointTracker>();   
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!pt.gameIsStarted)
        {
            CancelInvoke();
            return;
        }
        if (SettingsMenu.settingsOn)
        {
            CancelInvoke();
            Invoke("SpawnEnemy", spawnRate - spawnTimer);
            return;
        }
        else
        {
            spawnTimer += Time.deltaTime;
            spawnTimer = spawnTimer >= spawnRate ? spawnRate : spawnTimer;
        }

        handleDifficultyScaling();

        if (!IsInvoking())
            Invoke("SpawnEnemy", spawnRate);
    }

    void SpawnEnemy()
    {
        spawnTimer = 0; //reset spawn timer

        int randomTypeIndex = Random.Range(0, 4);

        Enemy.EnemyType eType;
        switch (randomTypeIndex)
        {
            case 0:
                spawnRate = 6.5f - spawnDecrease;
                eType = Enemy.EnemyType.Half;
                break;
            case 1:
                spawnRate = 6.5f - spawnDecrease;
                eType = Enemy.EnemyType.OneFull;
                break;
            case 2:
                spawnRate = 7 - spawnDecrease;
                eType = Enemy.EnemyType.TwoFull;
                break;
            case 3:
                spawnRate = 6.5f - spawnDecrease;
                eType = Enemy.EnemyType.VAttack;
                break;
            default:
                spawnRate = 6.5f - spawnDecrease;
                eType = Enemy.EnemyType.OneFull;
                break;
        }
        
        Enemy e;
        int enemyLane = -1;

        if (eType == Enemy.EnemyType.VAttack)  //VAttack only has one line
        {
            enemyLane = 4; //the special VAttack Lane
            e = Instantiate(enemy, enemyLanes[enemyLane].position, Quaternion.identity, enemyLanes[enemyLane]).GetComponent<Enemy>();
        }
        else
        {
            enemyLane = getActiveLane();

            if (enemyLane == -1)  //there are no active lanes, don't spawn anything
            {
                spawnRate = 4 - spawnDecrease;
                return;
            }

            activeLanes[enemyLane] = false;
            e = Instantiate(enemy, enemyLanes[enemyLane].position, Quaternion.identity, enemyLanes[enemyLane]).GetComponent<Enemy>();
        }
        
        e.setEnemyType(eType);
        e.setSpeed(enemySpeed);
        e.setLane(enemyLane);
    }

    void handleDifficultyScaling()
    {
        if (timer > difficultyIncrFrequency)
        {
            spawnDecrease += (spawnDecrease + spawnDecreaseDelta) > maxSpawnDecrease ? 0f : spawnDecreaseDelta;
            enemySpeed += (enemySpeed + enemySpeedDelta) > maxEnemySpeed ? 0f : enemySpeedDelta;
            timer = 0;
        }
        timer += Time.deltaTime;
    }


    public void freeLane(int freedLaneIndex)
    {
        activeLanes[freedLaneIndex] = true;
    }


    int getActiveLane()  
    {
        int numOfLanes = 0;
        for (int i = 0; i < 4; i++)
        {
            if (activeLanes[i])
                numOfLanes++;
        }
        if (numOfLanes == 0)
            return -1;

        int selectedLane = Random.Range(0, numOfLanes);
        int k = 0;

        for (int i = 0; i < 4; i++)
        {
            if (activeLanes[i])
            {
                if (k == selectedLane)
                    return i;
                else
                    k++;
            }
        }
        return -1;
    }
}
