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

    [Header("Properties")]
    public List<Tile> spawnLocations;

    // Initialization + Setup
    #region
    private void Start()
    {
        PopulateWaveList(viableBasicEnemyWaves, basicEnemyWaves);
        PopulateWaveList(viableEliteEnemyWaves, eliteEnemyWaves);
    }
    public void PopulateEnemySpawnLocations()
    {
        spawnLocations = new List<Tile>();
        spawnLocations.AddRange(LevelManager.Instance.GetEnemySpawnTiles());
    }
    #endregion

    // Enemy Spawning + Related
    #region
    public void SpawnEnemyWave(string enemyType = "Basic")
    {
        Debug.Log("SpawnEnemyWave() Called....");
        PopulateEnemySpawnLocations();

        EnemyWaveSO enemyWaveSO = null;
        // select a random enemyWaveSO
        if (enemyType == "Basic")
        {
            if(viableBasicEnemyWaves.Count == 0)
            {
                PopulateWaveList(viableBasicEnemyWaves, basicEnemyWaves);
            }

            enemyWaveSO = GetRandomWaveSO(viableBasicEnemyWaves, true);           
        }

        else if (enemyType == "Elite")
        {
            if (viableEliteEnemyWaves.Count == 0)
            {
                PopulateWaveList(viableEliteEnemyWaves, eliteEnemyWaves);
            }

            enemyWaveSO = GetRandomWaveSO(viableEliteEnemyWaves, true);            
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
            Tile spawnLocation = LevelManager.Instance.GetRandomValidMoveableTile(spawnLocations);
            // Run the enemy's constructor
            newEnemy.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }

    }
    public void PopulateWaveList(List <EnemyWaveSO> waveListToPopulate, List<EnemyWaveSO> wavesCopiedIn)
    {
        waveListToPopulate.AddRange(wavesCopiedIn);
    }    
    public EnemyWaveSO GetRandomWaveSO(List<EnemyWaveSO> enemyWaves, bool removeWaveFromList = false)
    {
        EnemyWaveSO enemyWaveReturned = enemyWaves[Random.Range(0, enemyWaves.Count)];        
        if(removeWaveFromList == true && enemyWaveReturned != null && enemyWaves.Count >= 1)
        {
            enemyWaves.Remove(enemyWaveReturned);
        }
        return enemyWaveReturned;
    }
    #endregion



}
