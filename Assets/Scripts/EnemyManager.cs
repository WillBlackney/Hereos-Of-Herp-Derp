using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{   

    public List<GameObject> enemyPrefabs;
    public GameObject enemiesParent;

    public List<Enemy> allEnemies = new List<Enemy>();
    public List<TileScript> spawnLocations = new List<TileScript>();

    public Enemy enemyCurrentlyInActivation;
    public Enemy selectedEnemy;

    public bool currentlyMyTurn = false;
    public bool allEnemiesHaveActivated = false;

    private void Start()
    {
        CreateSpawnLocations();
    }    

    public void SpawnEnemy()
    {
        // create a new game object
        GameObject newEnemyGO = Instantiate(enemyPrefabs[0]);
        // Get the script component and store it in a variable called "newEnemy"
        Enemy newEnemy = newEnemyGO.GetComponent<Enemy>();
        // Choose a random tile from the list of spawnable locations
        TileScript spawnLocation = LevelManager.Instance.GetRandomTile(spawnLocations);
        // Run the enemy's constructor
        newEnemy.InitializeSetup(spawnLocation.GridPosition, spawnLocation);              
    }    

    public void SelectEnemy(Enemy enemy)
    {
        Debug.Log("EnemyManager.SelectEnemy() called");
        selectedEnemy = enemy;

        Defender selectedDefender = DefenderManager.Instance.selectedDefender;

        if (selectedDefender != null && selectedDefender.awaitingStrikeOrder == true)
        {
            selectedDefender.StartStrikeProcess();
        }

        else if (selectedDefender != null && selectedDefender.awaitingChargeTargetOrder == true)
        {
            selectedDefender.StartChargeLocationSettingProcess();
        }

        else if (selectedDefender != null && selectedDefender.awaitingMeteorOrder == true)
        {
            selectedDefender.StartMeteorProcess(selectedEnemy.TileCurrentlyOn);
        }

        else if (selectedDefender != null && selectedDefender.awaitingTelekinesisTargetOrder == true)
        {
            selectedDefender.StartTelekinesisLocationSettingProcess(selectedEnemy);
        }
        
        else if (selectedDefender != null && selectedDefender.awaitingFrostBoltOrder == true)
        {
            selectedDefender.StartFrostBoltProcess();
        }

        else if (selectedDefender != null && selectedDefender.awaitingFireBallOrder == true)
        {
            selectedDefender.StartFireBallProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingShootOrder == true)
        {
            selectedDefender.StartShootProcess();
        }
        else if (selectedDefender != null && selectedDefender.awaitingRapidFireOrder == true)
        {
            selectedDefender.StartRapidFireProcess();
        }
        else if (selectedDefender != null && selectedDefender.awaitingImpalingBoltOrder == true)
        {
            selectedDefender.StartImpalingBoltProcess();
        }

        else if (selectedDefender != null && selectedDefender.awaitingHolyFireOrder == true)
        {
            selectedDefender.StartHolyFireProcess(selectedEnemy);
        }

        else if (selectedDefender != null && selectedDefender.awaitingVoidBombOrder == true)
        {
            selectedDefender.StartVoidBombProcess(selectedEnemy);
        }

        else if (selectedDefender != null && selectedDefender.awaitingNightmareOrder == true)
        {
            selectedDefender.StartNightmareProcess(selectedEnemy);
        }

        else if (selectedDefender != null && selectedDefender.awaitingTwinStrikeOrder == true)
        {
            selectedDefender.StartTwinStrikeProcess(selectedEnemy);
        }

        else if (selectedDefender != null && selectedDefender.awaitingSliceAndDiceOrder == true)
        {
            selectedDefender.StartSliceAndDiceProcess(selectedEnemy);
        }

        else if (selectedDefender != null && selectedDefender.awaitingPoisonDartOrder == true)
        {
            selectedDefender.StartPoisonDartProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingChemicalReactionOrder == true)
        {
            selectedDefender.StartChemicalReactionProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingSmashOrder == true)
        {
            selectedDefender.StartSmashProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingChainLightningOrder == true)
        {
            selectedDefender.StartChainLightningProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingPrimalBlastOrder == true)
        {
            selectedDefender.StartPrimalBlastProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingPhaseShiftOrder == true)
        {
            selectedDefender.StartPhaseShiftProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingSiphonLifeOrder == true)
        {
            selectedDefender.StartSiphonLifeProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingChaosBoltOrder == true)
        {
            selectedDefender.StartChaosBoltProcess(selectedEnemy);
        }
    }

    public void CreateSpawnLocations()
    {
        spawnLocations = LevelManager.Instance.GetTilesOnMapEdges();
    }

    public void StartEnemyTurnSequence()
    {
        StartCoroutine(MakeEnemyTurn());
    }

    public IEnumerator MakeEnemyTurn()
    {
        currentlyMyTurn = true;
        List<Enemy> enemiesActivatingThisTurn = new List<Enemy>();

        foreach(Enemy enemy in allEnemies)
        {
            enemiesActivatingThisTurn.Add(enemy);
            enemy.myOnActivationEndEffectsFinished = false;
        }
        foreach(Enemy enemy in enemiesActivatingThisTurn)
        {
            CameraManager.Instance.SetCameraLookAtTarget(enemy.gameObject);
            enemy.StartMyActivation();
            yield return new WaitUntil(() => enemy.ActivationFinished() == true);
        }

        CameraManager.Instance.ClearCameraLookAtTarget();
        allEnemiesHaveActivated = true;        
    }

    public void StartEnemyTurnEndSequence()
    {
        StartCoroutine(EndEnemyTurn());
    }

    public IEnumerator EndEnemyTurn()
    {
        // Need to make a temp list and not iterate over 'allEnemies'. If an enemy is killed/destroyed while iterating over 'allEnemies', an invalid operation error occurs.
        List<Enemy> enemiesResolvingEndOfTurnEffects = new List<Enemy>();
        enemiesResolvingEndOfTurnEffects.AddRange(allEnemies);

        foreach(Enemy enemy in enemiesResolvingEndOfTurnEffects)
        {
            enemy.OnActivationEnd();
            yield return new WaitUntil(() => enemy.MyOnTurnEndEffectsFinished() == true);
        }

        currentlyMyTurn = false;
    }

    public bool AllEnemiesHaveActivated()
    {
        if (allEnemiesHaveActivated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EnemyTurnFinished()
    {
        if (currentlyMyTurn)
        {
            return false;
        }
        else
        {
            return true;
        }
    }




}
