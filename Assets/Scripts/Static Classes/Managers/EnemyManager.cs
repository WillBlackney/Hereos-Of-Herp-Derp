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
        else if (selectedDefender != null && selectedDefender.awaitingHeadShotOrder == true)
        {
            selectedDefender.StartHeadShotProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingRapidFireOrder == true)
        {
            selectedDefender.StartRapidFireProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingBlizzardOrder == true)
        {
            selectedDefender.StartBlizzardProcess(selectedEnemy.tile);
        }
        else if (selectedDefender != null && selectedDefender.awaitingToxicEruptionOrder == true)
        {
            selectedDefender.StartToxicEruptionProcess(selectedEnemy.tile);
        }
        else if (selectedDefender != null && selectedDefender.awaitingBlindingLightOrder == true)
        {
            selectedDefender.StartBlindingLightProcess(selectedEnemy.tile);
        }
        else if (selectedDefender != null && selectedDefender.awaitingRainOfChaosOrder == true)
        {
            selectedDefender.StartRainOfChaosProcess(selectedEnemy.tile);
        }
        else if (selectedDefender != null && selectedDefender.awaitingThunderStormOrder == true)
        {
            selectedDefender.StartThunderStormProcess(selectedEnemy.tile);
        }

        else if (selectedDefender != null && selectedDefender.awaitingTelekinesisTargetOrder == true)
        {
            selectedDefender.StartTelekinesisLocationSettingProcess(selectedEnemy);
        }
        
        else if (selectedDefender != null && selectedDefender.awaitingFrostBoltOrder == true)
        {
            selectedDefender.StartFrostBoltProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingThawOrder == true)
        {
            selectedDefender.StartThawProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingFireBallOrder == true)
        {
            selectedDefender.StartFireBallProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingDimensionalBlastOrder == true)
        {
            selectedDefender.StartDimensionalBlastProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingShootOrder == true)
        {
            selectedDefender.StartShootProcess();
        }
        else if (selectedDefender != null && selectedDefender.awaitingSnipeOrder == true)
        {
            selectedDefender.StartSnipeProcess();
        }        
        else if (selectedDefender != null && selectedDefender.awaitingImpalingBoltOrder == true)
        {
            selectedDefender.StartImpalingBoltProcess(selectedEnemy);
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
        else if (selectedDefender != null && selectedDefender.awaitingSmashOrder == true)
        {
            selectedDefender.StartSmashProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingShankOrder == true)
        {
            selectedDefender.StartShankProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingProvokeOrder == true)
        {
            selectedDefender.StartProvokeProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingCheapShotOrder == true)
        {
            selectedDefender.StartCheapShotProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingDecapitateOrder == true)
        {
            selectedDefender.StartDecapitateProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingAmbushOrder == true)
        {
            selectedDefender.StartAmbushProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingShadowStepOrder == true)
        {
            selectedDefender.StartShadowStepProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingChainLightningOrder == true)
        {
            selectedDefender.StartChainLightningProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingPrimalBlastOrder == true)
        {
            selectedDefender.StartPrimalBlastProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingLightningBoltOrder == true)
        {
            selectedDefender.StartLightningBoltProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingPhaseShiftOrder == true)
        {
            selectedDefender.StartPhaseShiftProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingDimensionalHexOrder == true)
        {
            selectedDefender.StartDimensionalHexProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingDragonBreathOrder == true)
        {
            selectedDefender.StartDragonBreathProcess(selectedEnemy.tile);
        }
        else if (selectedDefender != null && selectedDefender.awaitingCombustionOrder == true)
        {
            selectedDefender.StartCombustionProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingSiphonLifeOrder == true)
        {
            selectedDefender.StartSiphonLifeProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingChaosBoltOrder == true)
        {
            selectedDefender.StartChaosBoltProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingKickToTheBallsOrder == true)
        {
            selectedDefender.StartKickToTheBallsProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingBlightOrder == true)
        {
            selectedDefender.StartBlightProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingChemicalReactionOrder == true)
        {
            selectedDefender.StartChemicalReactionProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingDrainOrder == true)
        {
            selectedDefender.StartDrainProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingDevastatingBlowOrder == true)
        {
            selectedDefender.StartDevastatingBlowProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingToxicSlashOrder == true)
        {
            selectedDefender.StartToxicSlashProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingThunderStrikeOrder == true)
        {
            selectedDefender.StartThunderStrikeProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingHexOrder == true)
        {
            selectedDefender.StartHexProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingJudgementOrder == true)
        {
            selectedDefender.StartJudgementProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingShieldSlamOrder == true)
        {
            selectedDefender.StartShieldSlamProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingTendonSlashOrder == true)
        {
            selectedDefender.StartTendonSlashProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingChillingBlowOrder == true)
        {
            selectedDefender.StartChillingBlowProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingShieldShatterOrder == true)
        {
            selectedDefender.StartShieldShatterProcess(selectedEnemy);
        }
        else if (selectedDefender != null && selectedDefender.awaitingSwordAndBoardOrder == true)
        {
            selectedDefender.StartSwordAndBoardProcess(selectedEnemy);
        }
    }
    #endregion



}
