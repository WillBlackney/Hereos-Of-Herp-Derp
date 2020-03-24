using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Encounter Lists")]
    public List<EnemyWaveSO> basicEnemyWavesActOneHalfOne;
    public List<EnemyWaveSO> basicEnemyWavesActOneHalfTwo;
    public List<EnemyWaveSO> eliteEnemyWaves;
    public List<EnemyWaveSO> bossEnemyWaves;
    public List<EnemyWaveSO> storyEventEnemyWaves;

    [Header("Current Viable Encounters Lists")]
    public List<EnemyWaveSO> viableBasicEnemyActOneHalfOneWaves;
    public List<EnemyWaveSO> viableBasicEnemyActOneHalfTwoWaves;
    public List<EnemyWaveSO> viableEliteEnemyWaves;

    [Header("Properties")]
    public List<Tile> spawnLocations;

    // Initialization + Setup
    #region
    public static EnemySpawner Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PopulateWaveList(viableBasicEnemyActOneHalfOneWaves, basicEnemyWavesActOneHalfOne);
        PopulateWaveList(viableBasicEnemyActOneHalfTwoWaves, basicEnemyWavesActOneHalfTwo);
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
    public void SpawnEnemyWave(string enemyType = "Basic", EnemyWaveSO enemyWave = null)
    {
        Debug.Log("SpawnEnemyWave() Called....");
        PopulateEnemySpawnLocations();
        EnemyWaveSO enemyWaveSO = enemyWave;

        // If we have not given a specific enemy wave to spawn, get a random one
        if(enemyWaveSO == null)
        {
            // select a random enemyWaveSO
            if (enemyType == "Basic" &&
                WorldManager.Instance.playerColumnPosition <= 5)
            {
                if (viableBasicEnemyActOneHalfOneWaves.Count == 0)
                {
                    PopulateWaveList(viableBasicEnemyActOneHalfOneWaves, basicEnemyWavesActOneHalfOne);
                }

                enemyWaveSO = GetRandomWaveSO(viableBasicEnemyActOneHalfOneWaves, true);
            }
            else if (enemyType == "Basic" &&
                WorldManager.Instance.playerColumnPosition > 5)
            {
                if (viableBasicEnemyActOneHalfTwoWaves.Count == 0)
                {
                    PopulateWaveList(viableBasicEnemyActOneHalfTwoWaves, basicEnemyWavesActOneHalfTwo);
                }

                enemyWaveSO = GetRandomWaveSO(viableBasicEnemyActOneHalfTwoWaves, true);
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
    public EnemyWaveSO GetEnemyWaveByName(string name)
    {
        List<EnemyWaveSO> allWaves = new List<EnemyWaveSO>();
        EnemyWaveSO waveReturned = null;

        allWaves.AddRange(basicEnemyWavesActOneHalfOne);
        allWaves.AddRange(eliteEnemyWaves);
        allWaves.AddRange(bossEnemyWaves);
        allWaves.AddRange(storyEventEnemyWaves);

        foreach(EnemyWaveSO wave in allWaves)
        {
            if(wave.waveName == name)
            {
                waveReturned = wave;
                break;
            }
        }

        return waveReturned;
    }
    #endregion



}
