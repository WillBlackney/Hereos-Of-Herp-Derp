using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [Header("Enemy Encounter Lists")]
    public List<EnemyWaveSO> basicEnemyWaves;
    public List<EnemyWaveSO> eliteEnemyWaves;
    public List<EnemyWaveSO> bossEnemyWaves;

    [Header("Current Viable Encounters Lists")]
    public List<EnemyWaveSO> viableBasicEnemyWaves;
    public List<EnemyWaveSO> viableEliteEnemyWaves;

    public List<TileScript> spawnLocations;   

    private void Start()
    {
        Debug.Log("EnemySpawner Start() called...");
        // PopulateEnemySpawnLocations();
        PopulateWaveList(viableBasicEnemyWaves, basicEnemyWaves);
        PopulateWaveList(viableEliteEnemyWaves, eliteEnemyWaves);
    }

    public void PopulateWaveList(List <EnemyWaveSO> waveListToPopulate, List<EnemyWaveSO> wavesCopiedIn)
    {
        waveListToPopulate.AddRange(wavesCopiedIn);
    }
    public void PopulateEnemySpawnLocations()
    {
        spawnLocations = new List<TileScript>();
        spawnLocations.AddRange(LevelManager.Instance.GetEnemySpawnTiles());
    }

    public EnemyWaveSO GetRandomWaveSO(List<EnemyWaveSO> enemyWaves, bool removeWaveFromList = false)
    {
        EnemyWaveSO enemyWaveReturned = enemyWaves[Random.Range(0, enemyWaves.Count)];        
        if(removeWaveFromList == true && enemyWaveReturned != null)
        {
            enemyWaves.Remove(enemyWaveReturned);
        }
        return enemyWaveReturned;
    }

    public void SpawnEnemyWave(string enemyType = "Basic")
    {
        Debug.Log("SpawnEnemyWave() Called....");
        PopulateEnemySpawnLocations();

        EnemyWaveSO enemyWaveSO = null;
        // select a random enemyWaveSO
        if (enemyType == "Basic")
        {
            enemyWaveSO = GetRandomWaveSO(viableBasicEnemyWaves, true);
            if(enemyWaveSO == null)
            {
                PopulateWaveList(viableBasicEnemyWaves, basicEnemyWaves);
                enemyWaveSO = GetRandomWaveSO(viableBasicEnemyWaves);
            }
        }
        else if (enemyType == "Elite")
        {
            enemyWaveSO = GetRandomWaveSO(viableEliteEnemyWaves, true);
            if (enemyWaveSO == null)
            {
                PopulateWaveList(viableEliteEnemyWaves, eliteEnemyWaves);
                enemyWaveSO = GetRandomWaveSO(viableEliteEnemyWaves);
            }
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
