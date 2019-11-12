using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{   
    [Header("Component References")]
    public GameObject enemiesParent;

    [Header("Properties")]
    public List<Enemy> allEnemies = new List<Enemy>();    
    public Enemy selectedEnemy;

    // Logic
    #region
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
            selectedDefender.StartMeteorProcess(selectedEnemy.tile);
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
    #endregion



}
