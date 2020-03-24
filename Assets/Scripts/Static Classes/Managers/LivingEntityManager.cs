using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntityManager : MonoBehaviour
{
    public List<LivingEntity> allLivingEntities;

    // Initialization + Singleton Pattern
    #region
    public static LivingEntityManager Instance;
    private void Awake()
    {
        allLivingEntities = new List<LivingEntity>();
        Instance = this;
    }

    // Call this when the player loses the game
    public void StopAllEntityCoroutines()
    {
        foreach(LivingEntity entity in allLivingEntities)
        {
            entity.StopAllCoroutines();
        }
    }
    #endregion

    // Activation related logic
    #region
    public Action EndEntityActivation(LivingEntity enemy)
    {
        Action action = new Action();
        StartCoroutine(EndEntityActivationCoroutine(enemy, action));
        return action;
    }
    private IEnumerator EndEntityActivationCoroutine(LivingEntity enemy, Action action)
    {
        Action endActivation = StartEntityOnActivationEndEvents(enemy);
        yield return new WaitUntil(() => endActivation.ActionResolved() == true);
        ActivationManager.Instance.ActivateNextEntity();
        action.actionResolved = true;
    }
    private Action StartEntityOnActivationEndEvents(LivingEntity entity)
    {
        Action action = new Action();
        StartCoroutine(StartEntityOnActivationEndEventsCoroutine(entity, action));
        return action;
    }
    private IEnumerator StartEntityOnActivationEndEventsCoroutine(LivingEntity entity, Action action)
    {
        Debug.Log("OnActivationEndCoroutine() called...");

        bool eventCompleted = false;

        // Wrap events in a while statement to allow a stoppage if the character dies during their end activation events
        while(eventCompleted == false && entity.inDeathProcess == false)
        {
            // Remove/apply relevant status effects and passives
            if (entity.myPassiveManager.vulnerable)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Vulnerable...");
                entity.myPassiveManager.ModifyVulnerable(-1);
                yield return new WaitForSeconds(1f);
            }

            // Remove Weakened
            if (entity.myPassiveManager.weakened)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Weakened...");
                entity.myPassiveManager.ModifyWeakened(-1);
                yield return new WaitForSeconds(1f);
            }

            // Remove Immobilized
            if (entity.myPassiveManager.immobilized)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Immobilized...");
                entity.myPassiveManager.ModifyImmobilized(-entity.myPassiveManager.immobilizedStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Blind
            if (entity.myPassiveManager.blind)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Blind...");
                entity.myPassiveManager.ModifyBlind(-entity.myPassiveManager.blindStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Disarmed
            if (entity.myPassiveManager.disarmed)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Disarmed...");
                entity.myPassiveManager.ModifyDisarmed(-entity.myPassiveManager.disarmedStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Silenced
            if (entity.myPassiveManager.silenced)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Silenced...");
                entity.myPassiveManager.ModifySilenced(-entity.myPassiveManager.silencedStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Stunned
            if (entity.myPassiveManager.stunned)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Stunned..");
                entity.myPassiveManager.ModifyStunned(-entity.myPassiveManager.stunnedStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Sleep
            if (entity.myPassiveManager.sleep)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Sleep...");
                entity.myPassiveManager.ModifySleep(-entity.myPassiveManager.sleepStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Taunted
            if (entity.myPassiveManager.taunted)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Taunted...");
                entity.myPassiveManager.ModifyTaunted(-entity.myPassiveManager.tauntedStacks, null);
                yield return new WaitForSeconds(1f);
            }

            // Remove Chilled
            if (entity.myPassiveManager.chilled)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Chilled...");
                entity.myPassiveManager.ModifyChilled(-entity.myPassiveManager.chilledStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Shocked
            if (entity.myPassiveManager.shocked)
            {
                Debug.Log("OnActivationEndCoroutine() clearing Shocked...");
                entity.myPassiveManager.ModifyShocked(-entity.myPassiveManager.shockedStacks);
                yield return new WaitForSeconds(1f);
            }

            // Cautious
            if (entity.myPassiveManager.cautious)
            {
                Debug.Log("OnActivationEndCoroutine() checking Cautious...");
                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Cautious");
                yield return new WaitForSeconds(0.5f);
                entity.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(entity.myPassiveManager.cautiousStacks, entity));
                yield return new WaitForSeconds(1f);
            }

            // Encouraging Aura
            if (entity.myPassiveManager.encouragingAura)
            {
                Debug.Log("OnActivationEndCoroutine() checking Encouraging Aura..");
                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Encouraging Aura");
                yield return new WaitForSeconds(0.5f);

                List<Tile> tilesInEncouragingPresenceRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(entity), entity.tile);
                foreach (LivingEntity entitty in allLivingEntities)
                {
                    if (tilesInEncouragingPresenceRange.Contains(entitty.tile) &&
                        CombatLogic.Instance.IsTargetFriendly(entity, entitty))
                    {
                        Debug.Log("Character " + entitty.name + " is within range of Encouraging presence, granting bonus Energy...");
                        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(entitty.transform.position));
                        entitty.ModifyCurrentEnergy(entity.myPassiveManager.encouragingAuraStacks);
                    }
                }

                yield return new WaitForSeconds(1f);
            }

            // Soul Drain Aura
            if (entity.myPassiveManager.soulDrainAura)
            {
                Debug.Log("OnActivationEndCoroutine() checking Soul Drain Aura...");

                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Soul Drain Aura");
                yield return new WaitForSeconds(0.5f);

                List<Tile> tilesInSoulDrainAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(entity), entity.tile);
                foreach (LivingEntity entityy in allLivingEntities)
                {
                    if (tilesInSoulDrainAuraRange.Contains(entityy.tile) &&
                        CombatLogic.Instance.IsTargetFriendly(entity, entityy) == false)
                    {
                        Debug.Log("Character " + entityy.name + " is within range of Sould Drain Aura, stealing Strength...");
                        entityy.ModifyCurrentStrength(-entity.myPassiveManager.soulDrainAuraStacks);
                        entity.ModifyCurrentStrength(entity.myPassiveManager.soulDrainAuraStacks);
                    }
                }

                yield return new WaitForSeconds(1f);
            }

            // Hateful Aura
            if (entity.myPassiveManager.hatefulAura)
            {
                Debug.Log("OnActivationEndCoroutine() checking Hateful Aura...");

                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Hateful Aura");
                yield return new WaitForSeconds(0.5f);

                List<Tile> tilesInHatefulPresenceRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(entity), entity.tile);
                foreach (LivingEntity entityy in allLivingEntities)
                {
                    if (tilesInHatefulPresenceRange.Contains(entityy.tile) &&
                        CombatLogic.Instance.IsTargetFriendly(entity, entityy))
                    {
                        Debug.Log("Character " + entityy.name + " is within range of Hateful Aura, granting bonus Strength...");
                        entityy.myPassiveManager.ModifyBonusStrength(entity.myPassiveManager.hatefulAuraStacks);
                    }
                }

                yield return new WaitForSeconds(1f);
            }

            // Fiery Aura
            if (entity.myPassiveManager.fieryAura)
            {
                Debug.Log("OnActivationEndCoroutine() checking Fiery Aura...");
                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Fiery Aura");
                yield return new WaitForSeconds(0.5f);

                List<Tile> tilesInFieryAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(entity), entity.tile);

                foreach (LivingEntity entityy in allLivingEntities)
                {
                    if (tilesInFieryAuraRange.Contains(entityy.tile) &&
                        CombatLogic.Instance.IsTargetFriendly(entity, entityy) == false)
                    {
                        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(entity, entityy, null, "Fire", false, entity.myPassiveManager.fieryAuraStacks);
                        Action damageAction = CombatLogic.Instance.HandleDamage(finalDamageValue, entity, entityy, "Fire");
                        yield return new WaitUntil(() => damageAction.ActionResolved() == true);
                    }
                }

                yield return new WaitForSeconds(1f);

            }

            // Shadow Aura
            if (entity.myPassiveManager.shadowAura)
            {
                Debug.Log("OnActivationEndCoroutine() checking Shadow Aura...");
                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Shadow Aura");
                yield return new WaitForSeconds(0.5f);

                List<Tile> tilesInShadowAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(entity), entity.tile);

                foreach (LivingEntity entityy in allLivingEntities)
                {
                    if (tilesInShadowAuraRange.Contains(entityy.tile) &&
                        CombatLogic.Instance.IsTargetFriendly(entity, entityy) == false)
                    {
                        entityy.myPassiveManager.ModifyWeakened(1);
                    }
                }
                yield return new WaitForSeconds(1f);

            }

            // Storm Aura
            if (entity.myPassiveManager.stormAura)
            {
                Debug.Log("OnActivationEndCoroutine() checking Storm Aura...");

                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Storm Aura");
                yield return new WaitForSeconds(0.5f);

                List<LivingEntity> stormAuraRange = EntityLogic.GetAllEnemiesWithinRange(entity, EntityLogic.GetTotalAuraSize(entity));
                List<LivingEntity> targetsHit = new List<LivingEntity>();

                // are there even enemies within aura range?
                if (stormAuraRange.Count > 0)
                {
                    // get a random target 2 times
                    for (int i = 0; i < 2; i++)
                    {
                        targetsHit.Add(stormAuraRange[Random.Range(0, stormAuraRange.Count)]);
                    }

                    // Resolve hits against targets
                    foreach (LivingEntity entityy in targetsHit)
                    {
                        if (entityy.inDeathProcess == false)
                        {
                            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(entity, entityy, null, "Air", false, entity.myPassiveManager.stormAuraStacks);

                            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, entity, entityy, "Air");
                            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                        }
                    }

                    yield return new WaitForSeconds(1f);
                }

            }

            // Guardian Aura
            if (entity.myPassiveManager.guardianAura)
            {
                Debug.Log("OnActivationEndCoroutine() checking Guardian Aura...");

                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Guardian Aura");
                yield return new WaitForSeconds(0.5f);

                List<Tile> tilesInGuardianAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(entity), entity.tile);

                foreach (LivingEntity entityy in allLivingEntities)
                {
                    if (tilesInGuardianAuraRange.Contains(entityy.tile) &&
                        CombatLogic.Instance.IsTargetFriendly(entity, entityy))
                    {
                        // Give target block
                        entityy.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(entity.myPassiveManager.guardianAuraStacks, entity));
                    }
                }
                yield return new WaitForSeconds(1f);
            }

            // Toxic Aura
            if (entity.myPassiveManager.toxicAura)
            {
                Debug.Log("OnActivationEndCoroutine() checking Toxic Aura...");

                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Toxic Aura");
                yield return new WaitForSeconds(0.5f);

                List<Tile> tilesInToxicAuraRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(entity), entity.tile);

                foreach (LivingEntity entityy in allLivingEntities)
                {
                    if (tilesInToxicAuraRange.Contains(entityy.tile) &&
                        CombatLogic.Instance.IsTargetFriendly(entity, entityy) == false)
                    {
                        // Modify Poisoned
                        entityy.myPassiveManager.ModifyPoisoned(entity.myPassiveManager.toxicAuraStacks, entity);
                    }
                }
                yield return new WaitForSeconds(1f);
            }

            // Sacred Aura
            if (entity.myPassiveManager.sacredAura)
            {
                Debug.Log("OnActivationEndCoroutine() checking Sacred Aura...");
                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Sacred Aura");
                yield return new WaitForSeconds(0.5f);

                List<Tile> tilesInEncouragingPresenceRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(entity), entity.tile);
                foreach (LivingEntity entityy in allLivingEntities)
                {
                    if (tilesInEncouragingPresenceRange.Contains(entityy.tile) &&
                        CombatLogic.Instance.IsTargetFriendly(entity, entityy))
                    {
                        Debug.Log("Character " + entityy.name + " is within range of Sacred Aura, removing debuffs...");
                        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(entityy.transform.position));

                        // Remove Blind
                        if (entityy.myPassiveManager.blind)
                        {
                            entityy.myPassiveManager.ModifyBlind(-entityy.myPassiveManager.blindStacks);
                            yield return new WaitForSeconds(0.5f);
                        }

                        // Remove Disarmed
                        if (entityy.myPassiveManager.disarmed)
                        {
                            entityy.myPassiveManager.ModifyDisarmed(-entityy.myPassiveManager.disarmedStacks);
                            yield return new WaitForSeconds(0.5f);
                        }

                        // Remove Silenced
                        if (entityy.myPassiveManager.silenced)
                        {
                            entityy.myPassiveManager.ModifySilenced(-entityy.myPassiveManager.silencedStacks);
                            yield return new WaitForSeconds(0.5f);
                        }

                        // Remove Weakened
                        if (entityy.myPassiveManager.weakened)
                        {
                            entityy.myPassiveManager.ModifyWeakened(-entityy.myPassiveManager.weakenedStacks);
                            yield return new WaitForSeconds(0.5f);
                        }

                        // Remove Vulnerable
                        if (entityy.myPassiveManager.vulnerable)
                        {
                            entityy.myPassiveManager.ModifyVulnerable(-entityy.myPassiveManager.vulnerableStacks);
                            yield return new WaitForSeconds(0.5f);
                        }

                    }
                }

                yield return new WaitForSeconds(1f);
            }

            // Regeneration
            if (entity.myPassiveManager.regeneration)
            {
                Debug.Log("OnActivationEndCoroutine() checking Regeneration...");

                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Regeneration");
                entity.ModifyCurrentHealth(entity.myPassiveManager.regenerationStacks);
                yield return new WaitForSeconds(0.5f);
            }

            // Poisoned
            if (entity.myPassiveManager.poisoned)
            {
                Debug.Log("OnActivationEndCoroutine() checking Poisoned...");
                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Poisoned");
                yield return new WaitForSeconds(0.5f);
                Action poisonDamage = CombatLogic.Instance.HandleDamage(entity.myPassiveManager.poisonedStacks, null, entity, "Poison", null);
                yield return new WaitUntil(() => poisonDamage.ActionResolved() == true);
                yield return new WaitForSeconds(1f);
            }

            // Burning
            if (entity.myPassiveManager.burning)
            {
                Debug.Log("OnActivationEndCoroutine() checking Burning...");

                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Burning");
                yield return new WaitForSeconds(0.5f);
                Action burningDamage = CombatLogic.Instance.HandleDamage(entity.myPassiveManager.burningStacks, null, entity, "Fire", null);
                yield return new WaitUntil(() => burningDamage.ActionResolved() == true);
                yield return new WaitForSeconds(1f);
            }

            // Fading
            if (entity.myPassiveManager.fading)
            {
                Debug.Log("OnActivationEndCoroutine() checking Fading...");

                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Fading");
                yield return new WaitForSeconds(0.5f);
                Action fadingDamage = CombatLogic.Instance.HandleDamage(entity.myPassiveManager.fadingStacks, entity, entity, "None", null, true);
                yield return new WaitUntil(() => fadingDamage.ActionResolved() == true);
                yield return new WaitForSeconds(1f);
            }

            // Remove Temporary Imbuements

            // Air Imbuement
            if (entity.myPassiveManager.temporaryAirImbuement)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Air Imbuement...");
                entity.myPassiveManager.ModifyTemporaryAirImbuement(-entity.myPassiveManager.temporaryAirImbuementStacks);
                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Air Imbuement Removed");
                yield return new WaitForSeconds(1f);
            }

            // Fire Imbuement
            if (entity.myPassiveManager.temporaryFireImbuement)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Fire Imbuement...");
                entity.myPassiveManager.ModifyTemporaryFireImbuement(-entity.myPassiveManager.temporaryFireImbuementStacks);
                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Fire Imbuement Removed");
                yield return new WaitForSeconds(1f);
            }

            // Shadow Imbuement
            if (entity.myPassiveManager.temporaryShadowImbuement)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Shadow Imbuement...");
                entity.myPassiveManager.ModifyTemporaryShadowImbuement(-entity.myPassiveManager.temporaryShadowImbuementStacks);
                yield return new WaitForSeconds(1f);
            }

            // Frost Imbuement
            if (entity.myPassiveManager.temporaryFrostImbuement)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Frost Imbuement...");
                entity.myPassiveManager.ModifyTemporaryFrostImbuement(-entity.myPassiveManager.temporaryFrostImbuementStacks);
                yield return new WaitForSeconds(1f);
            }

            // Poison Imbuement
            if (entity.myPassiveManager.temporaryPoisonImbuement)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Poison Imbuement...");
                entity.myPassiveManager.ModifyTemporaryPoisonImbuement(-entity.myPassiveManager.temporaryPoisonImbuementStacks);
                yield return new WaitForSeconds(1f);
            }

            // Rapid Cloaking
            if (entity.myPassiveManager.rapidCloaking)
            {
                Debug.Log("OnActivationEndCoroutine() checking Rapid Cloaking...");
                entity.myPassiveManager.ModifyCamoflage(1);
                yield return new WaitForSeconds(1f);
            }

            // Tile related events
            if (entity.tile.myTileType == Tile.TileType.Grass)
            {
                Debug.Log("OnActivationEndCoroutine() checking Grass Tile (Camoflage)...");
                if (entity.myPassiveManager.camoflage == false)
                {
                    entity.myPassiveManager.ModifyCamoflage(1);
                }

                yield return new WaitForSeconds(1);
            }

            // Remove Temporary Core + Secondary Stats

            // Bonus Strength
            if (entity.myPassiveManager.temporaryBonusStrength)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Strength...");
                entity.myPassiveManager.ModifyTemporaryStrength(-entity.myPassiveManager.temporaryBonusStrengthStacks);
                yield return new WaitForSeconds(1f);
            }

            // Bonus Dexterity
            if (entity.myPassiveManager.temporaryBonusDexterity)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Dexterity...");
                entity.myPassiveManager.ModifyTemporaryDexterity(-entity.myPassiveManager.temporaryBonusDexterityStacks);
                yield return new WaitForSeconds(1f);
            }

            // Bonus Stamina
            if (entity.myPassiveManager.temporaryBonusStamina)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Stamina...");
                entity.myPassiveManager.ModifyTemporaryStamina(-entity.myPassiveManager.temporaryBonusStaminaStacks);
                yield return new WaitForSeconds(1);
            }

            // Bonus Wisdom
            if (entity.myPassiveManager.temporaryBonusWisdom)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Wisdom...");
                entity.myPassiveManager.ModifyTemporaryWisdom(-entity.myPassiveManager.temporaryBonusWisdomStacks);
                yield return new WaitForSeconds(1f);
            }

            // Bonus Initiative
            if (entity.myPassiveManager.temporaryBonusInitiative)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Initiative...");
                entity.myPassiveManager.ModifyTemporaryInitiative(-entity.myPassiveManager.temporaryBonusInitiativeStacks);
                yield return new WaitForSeconds(1f);
            }

            // Bonus Mobility
            if (entity.myPassiveManager.temporaryBonusMobility)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Bonus Mobility...");
                entity.myPassiveManager.ModifyTemporaryMobility(-entity.myPassiveManager.temporaryBonusMobilityStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Terrified
            if (entity.myPassiveManager.terrified)
            {
                Debug.Log("OnActivationEndCoroutine() checking Terrified...");
                entity.myPassiveManager.ModifyTerrified(-entity.myPassiveManager.terrifiedStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Temporary True Sight
            if (entity.myPassiveManager.temporaryTrueSight)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary True Sight...");
                entity.myPassiveManager.ModifyTemporaryTrueSight(-entity.myPassiveManager.temporaryTrueSightStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Dark Gift
            if (entity.myPassiveManager.darkGift)
            {
                Debug.Log("OnActivationEndCoroutine() checking Dark Gift...");
                entity.myPassiveManager.ModifyDarkGift(-entity.myPassiveManager.darkGiftStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Pure Hate
            if (entity.myPassiveManager.pureHate)
            {
                Debug.Log("OnActivationEndCoroutine() checking Pure Hate...");
                entity.myPassiveManager.ModifyPureHate(-entity.myPassiveManager.pureHateStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Temporary Hawk Eye
            if (entity.myPassiveManager.temporaryHawkEye)
            {
                Debug.Log("OnActivationEndCoroutine() checking Temporary Hawk Eye...");
                entity.myPassiveManager.ModifyTemporaryHawkEyeBonus(-entity.myPassiveManager.temporaryHawkEyeStacks);
                yield return new WaitForSeconds(1f);
            }

            // Remove Berserk
            if (entity.myPassiveManager.berserk)
            {
                Debug.Log("OnActivationEndCoroutine() checking Berserk...");
                entity.myPassiveManager.ModifyBerserk(-entity.myPassiveManager.berserkStacks);
                yield return new WaitForSeconds(1f);
            }

            // All effects completed and checked
            eventCompleted = true;
        }       
        

        // Resolve
        Debug.Log("OnActivationEndCoroutine() finished and resolving...");
        yield return new WaitForSeconds(1f);
        entity.myOnActivationEndEffectsFinished = true;
        action.actionResolved = true;

    }
    #endregion


}
