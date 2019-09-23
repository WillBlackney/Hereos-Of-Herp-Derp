using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [Header("Enemy Encounter Lists")]
    public List<EnemyWaveSO> basicEnemyWaves;
    public List<EnemyWaveSO> eliteEnemyWaves;
    public List<EnemyWaveSO> bossEnemyWaves;    

    public List<TileScript> spawnLocations;   

    private void Start()
    {
        Debug.Log("EnemySpawner Start() called...");
        // PopulateEnemySpawnLocations();
    }

    public void PopulateEnemySpawnLocations()
    {
        spawnLocations = new List<TileScript>();
        spawnLocations.AddRange(LevelManager.Instance.GetEnemySpawnTiles());
    }

    public EnemyWaveSO GetRandomWaveSO(List<EnemyWaveSO> enemyWaves)
    {
        int randomIndex = Random.Range(0, enemyWaves.Count);
        return enemyWaves[randomIndex];
    }

    public void SpawnEnemyWave(string enemyType = "Basic")
    {
        Debug.Log("SpawnEnemyWave() Called....");
        PopulateEnemySpawnLocations();

        EnemyWaveSO enemyWaveSO = null;
        // select a random enemyWaveSO
        if (enemyType == "Basic")
        {
            enemyWaveSO = GetRandomWaveSO(basicEnemyWaves);
        }
        else if (enemyType == "Elite")
        {
            enemyWaveSO = GetRandomWaveSO(eliteEnemyWaves);
        }
        else if (enemyType == "Boss")
        {
            enemyWaveSO = GetRandomWaveSO(bossEnemyWaves);
        }

       
        foreach (EnemyGroup enemyGroup in enemyWaveSO.enemyGroups)
        {
            int randomIndex = Random.Range(0, enemyGroup.enemyList.Count);
            GameObject newEnemyGO = Instantiate(enemyGroup.enemyList[randomIndex]);
            Enemy newEnemy = newEnemyGO.GetComponent<Enemy>();
            // Choose a random tile from the list of spawnable locations
            TileScript spawnLocation = LevelManager.Instance.GetRandomValidMoveableTile(spawnLocations);
            // Run the enemy's constructor
            newEnemy.InitializeSetup(spawnLocation.GridPosition, spawnLocation);            
        }
               
    }

    public TileScript GetRandomSpawnLocation()
    {
        int randomSpawnLocationIndex = Random.Range(0, spawnLocations.Count);
        return spawnLocations[randomSpawnLocationIndex];
    }

    /*
    public List<GameObject> GetThreeRandomEnemies()
    {
        List<GameObject> enemiesReturned = new List<GameObject>();

        for(int i = 0; i < 3; i++)
        {
            GameObject enemyReturned = null;
            while(enemyReturned == null || enemiesReturned.Contains(enemyReturned))
            {
                enemyReturned = skeletonPrefabs[Random.Range(0, skeletonPrefabs.Count)];
            }

            enemiesReturned.Add(enemyReturned);
        }        

        return enemiesReturned;
    }
    */
}
