using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class EntityLogic
{
    // Get Living Entity Methods
    #region
    public static LivingEntity GetClosestEnemy(LivingEntity entity)
    {
        // TO DO: this method determines which defender is closest by drawing a straight line. It should instead calculate the closest by drawing a path to each with Astar

        LivingEntity closestTarget = null;
        float minimumDistance = Mathf.Infinity;

        // Declare new temp list for storing valid entities
        List<LivingEntity> potentialEnemies = new List<LivingEntity>();

        // Add all active entities that are NOT friendly to the temp list
        foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
        {
            if(!CombatLogic.Instance.IsTargetFriendly(entity, entityy))
            {
                potentialEnemies.Add(entityy);
            }
            
        }

        // Iterate throught the temp list to find the closest enemy to this character
        foreach (LivingEntity enemy in potentialEnemies)
        {
            float distancefromCharacter = Vector2.Distance(enemy.gameObject.transform.position, entity.transform.position);
            if (distancefromCharacter < minimumDistance)
            {
                closestTarget = enemy;
                minimumDistance = distancefromCharacter;
            }
        }
        return closestTarget;
    }
    public static LivingEntity GetClosestValidEnemy(LivingEntity entity)
    {
        // TO DO: this method determines which defender is closest by drawing a straight line. It should instead calculate the closest by drawing a path to each with Astar

        LivingEntity closestTarget = null;
        float minimumDistance = Mathf.Infinity;

        // Declare new temp list for storing valid entities
        List<LivingEntity> potentialEnemies = new List<LivingEntity>();

        // Add all active entities that are NOT friendly to the temp list
        foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
        {
            if (!CombatLogic.Instance.IsTargetFriendly(entity, entityy))
            {
                potentialEnemies.Add(entityy);
            }

        }

        // Iterate throught the temp list to find the closest VALID enemy to this character
        foreach (LivingEntity enemy in potentialEnemies)
        {
            if (IsTargetVisible(entity, enemy))
            {
                float distancefromCharacter = Vector2.Distance(enemy.gameObject.transform.position, entity.transform.position);
                if (distancefromCharacter < minimumDistance)
                {
                    closestTarget = enemy;
                    minimumDistance = distancefromCharacter;
                }
            }
            
        }
        return closestTarget;
    }
    public static LivingEntity GetClosestAlly(LivingEntity entity, bool includeSelf = true)
    {
        LivingEntity closestAlly = null;
        float minimumDistance = Mathf.Infinity;

        foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
        {
            if(CombatLogic.Instance.IsTargetFriendly(entityy, entity))
            {
                float distancefromThisCharacter = Vector2.Distance(entityy.gameObject.transform.position, entity.transform.position);
                if (distancefromThisCharacter < minimumDistance && entityy != entity)
                {
                    if(
                        (includeSelf == true && entityy == entity) ||
                        (includeSelf == false && entityy != entity))
                    {
                        closestAlly = entityy;
                        minimumDistance = distancefromThisCharacter;
                    }
                    
                }
            }
            
        }


        if (closestAlly == null)
        {
            closestAlly = entity;
        }

        return closestAlly;
    }
    public static LivingEntity GetMostVulnerableEnemy(LivingEntity entity)
    {
        LivingEntity bestTarget = null;
        int pointScore = 0;

        foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
        {
            if(!CombatLogic.Instance.IsTargetFriendly(entity, entityy))
            {
                int myPointScore = 1;

                if (entityy.myPassiveManager.vulnerable)
                {
                    myPointScore += 1;
                }

                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = entityy;
                }
            }
            
        }

        return bestTarget;
    }
    public static LivingEntity GetEnemyWithLowestCurrentHP(LivingEntity entity)
    {
        LivingEntity bestTarget = null;
        int lowestHP = 1000;

        // Declare new temp list for storing defender 
        List<LivingEntity> potentialEnemies = new List<LivingEntity>();

        // Add all active defenders to the temp list
        foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
        {
            if(!CombatLogic.Instance.IsTargetFriendly(entity, entityy))
            {
                potentialEnemies.Add(entityy);
            }            
        }

        foreach (LivingEntity entityy in potentialEnemies)
        {
            if (entityy.currentHealth < lowestHP)
            {
                bestTarget = entityy;
                lowestHP = entityy.currentHealth;
            }
        }

        if (bestTarget == null)
        {
            Debug.Log("GetDefenderWithLowestCurrentHP() returning null !!...");
        }

        return bestTarget;
    }
    public static List<LivingEntity> GetAllEnemiesWithinRange(LivingEntity entityFrom, int range)
    {
        List<Tile> tilesInRange = LevelManager.Instance.GetTilesWithinRange(range, entityFrom.tile);
        List<LivingEntity> enemiesInRange = new List<LivingEntity>();

        foreach (LivingEntity livingEntity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInRange.Contains(livingEntity.tile) && 
                CombatLogic.Instance.IsTargetFriendly(entityFrom, livingEntity) == false)
            {
                enemiesInRange.Add(livingEntity);
            }
        }

        return enemiesInRange;
    }
    public static LivingEntity GetBestTarget(LivingEntity entity, bool prioritizeClosest = false, bool prioritizeLowHP = false, bool prioritizeMostVulnerable = false)
    {
        Debug.Log("EntityLogic.GetBestTarget() called...");
        LivingEntity targetReturned = null;

        // Check for taunted first
        if (entity.myPassiveManager.taunted &&
            entity.myTaunter != null &&
            entity.myTaunter.inDeathProcess == false)
        {
            Debug.Log(entity.name + " is taunted by " + entity.myTaunter.name + ", returning this as best target...");
            targetReturned = entity.myTaunter;
        }
        else if (prioritizeClosest)
        {
            targetReturned = GetClosestEnemy(entity);
        }
        else if (prioritizeLowHP)
        {
            targetReturned = GetEnemyWithLowestCurrentHP(entity);
        }
        else if (prioritizeMostVulnerable)
        {
            targetReturned = GetEnemyWithLowestCurrentHP(entity);
        }
        else
        {
            targetReturned = null;
        }

        // Check for null
        if(targetReturned == null)
        {
            Debug.Log("EntityLogic.GetBestTarget() returned a null value...");
        }
        else if (targetReturned != null)
        {
            Debug.Log("EntityLogic.GetBestTarget() returned " + targetReturned.name + " as the best target for " + entity.name);
        }
           
        return targetReturned;
    }
    

    #endregion

    // Conditional Checks + Booleans
    #region
    public static bool IsAbleToMove(LivingEntity entity)
    {
        if(entity.myPassiveManager.immobilized ||
           GetTotalMobility(entity) <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    public static bool HasEnoughAP(LivingEntity entity, Ability ability)
    {
        if (entity.currentEnergy >= ability.abilityEnergyCost)
        {
            return true;
        }

        else if (entity.myPassiveManager.preparation)
        {
            return true;
        }

        else if (entity.myPassiveManager.flux &&
                entity.moveActionsTakenThisActivation == 0 &&
                ability.abilityName == "Move")
        {
            return true;
        }

        else
        {
            Debug.Log("Action failed: Not enough AP");
            return false;
        }
    }
    public static bool IsAbleToTakeActions(LivingEntity entity)
    {
        if (entity.myPassiveManager.stunned ||
            entity.myPassiveManager.sleep)
        {
            Debug.Log("Action failed. Unable to take actions while stunned or sleeping");
            return false;
        }
        
        else
        {
            return true;
        }
    }
    public static bool IsTargetInRange(LivingEntity caster, LivingEntity target, int range, bool ignoreLoS = false)
    {
        List<Tile> tilesWithinMyRange = LevelManager.Instance.GetTilesWithinRange(range, caster.tile, false, ignoreLoS);

        if (target == null)
        {
            Debug.Log("IsTargetInRange() target value is null...");
            return false;
        }

        Tile targetsTile = target.tile;

        if (tilesWithinMyRange.Contains(targetsTile) && IsTargetVisible(caster, target))
        {
            Debug.Log("Target enemy is range");
            return true;
        }
        else
        {
            Debug.Log("Target enemy is NOT range");
            return false;
        }
    }
    public static bool IsTargetVisible(LivingEntity caster, LivingEntity target)
    {
        List<Tile> tilesWithinStealthSight = LevelManager.Instance.GetTilesWithinRange(1, caster.tile);
        bool passedStealthCheck = false;
        bool passedLosCheck = false;

        // check for null target
        if(target == null)
        {
            Debug.Log("IsTargetVisible() target recieved is null, returning false...");
            return false;
        }

        // check for stealth
        if (tilesWithinStealthSight.Contains(target.tile) == false &&
            (target.myPassiveManager.camoflage || target.myPassiveManager.stealth) &&
            (CombatLogic.Instance.IsTargetFriendly(caster, target) == false) &&
             (caster.myPassiveManager.trueSight == false && caster.myPassiveManager.temporaryTrueSight == false)
            )
        {
            Debug.Log("IsTargetVisible() determined that " + target.name + " CANNOT be seen by "+ caster.name + "...");
            passedStealthCheck = false;
        }

        else
        {
            Debug.Log("IsTargetVisible() determined that " + target.name + " CAN be seen by " + caster.name + "...");
            passedStealthCheck = true;
        }

        // check for LoS
        if(PositionLogic.Instance.IsThereLosFromAtoB(caster.tile, target.tile))
        {
            Debug.Log("IsTargetVisible() determined that " + caster.name + " has line of sight to " + target.name + "...");
            passedLosCheck = true;
        }
        // check if caster has ignores LoS to target
        else if (PositionLogic.Instance.IsThereLosFromAtoB(caster.tile, target.tile) == false &&
                 caster.myPassiveManager.etherealBeing)
        {
            Debug.Log("IsTargetVisible() determined that " + caster.name + " does not have direct line of sight to " + target.name + ", but has 'Ethereal Being' passive...");
            passedLosCheck = true;
        }
        else
        {
            Debug.Log("IsTargetVisible() determined that " + caster.name + " DOES NOT have line of sight to " + target.name + "...");
            passedLosCheck = false;
        }

        // return the result of the checks
        if(passedLosCheck && passedStealthCheck)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool IsAbilityOffCooldown(Ability ability)
    {
        if (ability.abilityCurrentCooldownTime == 0)
        {
            return true;
        }
        else
        {
            Debug.Log("Cannot use ability: Ability is on cooldown");
            return false;
        }
    }
    public static bool IsAbilityUseable(LivingEntity entity, Ability ability)
    {
        if(!HasEnoughAP(entity, ability) ||
            !IsAbilityOffCooldown(ability) ||
            !AbilityLogic.Instance.DoesAbilityMeetWeaponRequirements(entity, ability) ||
            (ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack && entity.myPassiveManager.disarmed) ||
            (ability.abilityType == AbilityDataSO.AbilityType.RangedAttack && entity.myPassiveManager.blind) ||
            (ability.abilityType == AbilityDataSO.AbilityType.Skill && entity.myPassiveManager.silenced)
            )
        {
            return false;
        }
        else
        {
            return true; ;
        }
    }
    public static bool CanPerformAbilityTwoAfterAbilityOne(Ability abilityOne, Ability abilityTwo, LivingEntity entity)
    {
        // method is mainly used to stop enemies from move forward too agressively.
        // prevents enemies from move into a defenders range without being able to attack.
        // some enemies should only advance if they can make attacks after moving (like squishy melee fighters, or ranged attackers)

        int currentAP = entity.currentEnergy;

        if(currentAP - abilityOne.abilityEnergyCost >= abilityTwo.abilityEnergyCost)
        {
            Debug.Log("CanPerformAbilityTwoAfterAbilityOne() calculated that " + entity.name + " has enougn AP to perform " + 
                abilityTwo.abilityName + " after " + abilityOne.abilityName);
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Get Tiles + Pathfinding Logic
    #region
    public static Tile GetValidGrassTileWithinRange(LivingEntity entityFrom, int range)
    {
        Tile closestGrassTile = null;
        List<Tile> adjacentTiles = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, entityFrom.tile);

        foreach (Tile tile in adjacentTiles)
        {
            if (tile.myTileType == Tile.TileType.Grass && tile.IsEmpty && tile.IsWalkable)
            {
                closestGrassTile = tile;
            }
        }

        return closestGrassTile;
    }
    public static Tile GetFurthestTileFromTargetWithinRange(LivingEntity originCharacter, LivingEntity target, int range)
    {
        // method used to enemy AI move away from a defender logically to safety
        List<Tile> tilesWithinRangeOfOriginCharacter = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, originCharacter.tile);
        return LevelManager.Instance.GetFurthestTileFromTargetFromList(tilesWithinRangeOfOriginCharacter, target.tile);
    }
    public static Tile GetBestValidMoveLocationBetweenMeAndTarget(LivingEntity characterActing, LivingEntity target, int rangeFromTarget, int movePoints)
    {
        Tile tileReturned = null;
        Tile tileClosestToTarget = LevelManager.Instance.GetClosestValidTile(LevelManager.Instance.GetTilesWithinRange(rangeFromTarget, target.tile), characterActing.tile);

        Stack<Node> pathFromMeToIdealTile = MovementLogic.Instance.GeneratePath(characterActing.tile.GridPosition, tileClosestToTarget.GridPosition);

        Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() generated a path with " + pathFromMeToIdealTile.Count.ToString() + " tiles on it");        

        
        if (pathFromMeToIdealTile.Count > 1 && pathFromMeToIdealTile.Count < movePoints)
        {
            tileReturned = LevelManager.Instance.GetTileFromPointReference(pathFromMeToIdealTile.ElementAt(pathFromMeToIdealTile.Count - 1).GridPosition);
        }       

        else if (pathFromMeToIdealTile.Count > 1)
        {
            tileReturned = LevelManager.Instance.GetTileFromPointReference(pathFromMeToIdealTile.ElementAt(movePoints - 1).GridPosition);
        }

        else if (pathFromMeToIdealTile.Count == 1)
        {
            tileReturned = LevelManager.Instance.GetTileFromPointReference(pathFromMeToIdealTile.ElementAt(0).GridPosition);
        }

        if (tileReturned == null)
        {
            Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() could not draw a valid path from" + characterActing.name +
                " to " + target.name + ", returning a null Tile destination...");
        }
        else if (tileReturned != null)
        {
            Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() determined that the best move location between " +
                characterActing.name + " and " + target.name + " is Tile " + tileReturned.GridPosition.X.ToString() + ", " + tileReturned.GridPosition.Y.ToString());
        }

        return tileReturned;

    }

    #endregion

    // Get Stats And Attribute Total Values
    #region
    // Core stats
    public static int GetTotalDexterity(LivingEntity entity)
    {
        // Get base dexterity
        int dexterityReturned = entity.currentDexterity;

        // Add from bonus dexterity passive
        if (entity.myPassiveManager.bonusDexterity)
        {
            dexterityReturned += entity.myPassiveManager.bonusDexterityStacks;
        }

        // Add from temporary bonus dexterity passive
        if (entity.myPassiveManager.temporaryBonusDexterity)
        {
            dexterityReturned += entity.myPassiveManager.temporaryBonusDexterityStacks;
        }

        // Add bonus from purity passive
        if (entity.myPassiveManager.purity)
        {
            dexterityReturned += 2;
        }

        // return final value
        return dexterityReturned;
    }
    public static int GetTotalStrength(LivingEntity entity)
    {
        // Get base dexterity
        int strengthReturned = entity.currentStrength;

        // Add bonus strength
        strengthReturned += entity.myPassiveManager.bonusStrengthStacks;

        // Add temporary bonus strength
        strengthReturned += entity.myPassiveManager.temporaryBonusStrengthStacks;

        // Add bonus from purity passive
        if (entity.myPassiveManager.purity)
        {
            strengthReturned += 2;
        }

        // return final value
        return strengthReturned;
    }
    public static int GetTotalWisdom(LivingEntity entity)
    {
        // Get base wisdom
        int wisdomReturned = entity.currentWisdom;

        // Add bonus wisdom
        wisdomReturned += entity.myPassiveManager.bonusWisdomStacks;

        // Add temporary bonus wisdom
        wisdomReturned += entity.myPassiveManager.temporaryBonusWisdomStacks;

        // Add bonus from purity passive
        if (entity.myPassiveManager.purity)
        {
            wisdomReturned += 2;
        }

        // return final value
        return wisdomReturned;
    }
    public static int GetTotalInitiative(LivingEntity entity)
    {
        // Get base Initiative
        int initiativeReturned = entity.currentInitiative;

        // Add bonus Initiative
        initiativeReturned += entity.myPassiveManager.bonusInitiativeStacks;

        // Add temporary bonus Initiative
        initiativeReturned += entity.myPassiveManager.temporaryBonusInitiativeStacks;

        // Reduce by 1 if 'Chilled'
        if (entity.myPassiveManager.chilled)
        {
            initiativeReturned -= 1;
        }

        // Prevent going below zero
        if (initiativeReturned < 0)
        {
            initiativeReturned = 0;
        }

        // return final value
        return initiativeReturned;
    }
    public static int GetTotalMobility(LivingEntity entity)
    {
        // Get base Mobility
        int mobilityReturned = entity.currentMobility;

        // Add bonus Mobility
        mobilityReturned += entity.myPassiveManager.bonusMobilityStacks;

        // Add temporary bonus Mobility
        mobilityReturned += entity.myPassiveManager.temporaryBonusMobilityStacks;

        // Reduce by 1 if 'Chilled'
        if (entity.myPassiveManager.chilled)
        {
            mobilityReturned -= 1;
        }

        // Check for Patient Stalker
        if (entity.myPassiveManager.patientStalker &&
          (entity.myPassiveManager.camoflage || entity.myPassiveManager.stealth)
          )
        {
            mobilityReturned += 10;
        }

        // Prevent going below zero
        if (mobilityReturned < 0)
        {
            mobilityReturned = 0;
        }
        // return final value
        return mobilityReturned;
    }
    public static int GetTotalStamina(LivingEntity entity)
    {
        // Get base Stamina
        int staminaReturned = entity.currentStamina;

        // Add bonus Stamina
        staminaReturned += entity.myPassiveManager.bonusStaminaStacks;

        // Add temporary bonus Stamina
        staminaReturned += entity.myPassiveManager.temporaryBonusStaminaStacks;

        // Check for Patient Stalker
        if(entity.myPassiveManager.patientStalker && 
          (entity.myPassiveManager.camoflage || entity.myPassiveManager.stealth)
          )
        {
            staminaReturned += 10;
        }

        // Minus 10 if 'Shocked'
        if (entity.myPassiveManager.shocked)
        {
            staminaReturned -= 10;
        }

        // prevent reducing below 0
        if(staminaReturned < 0)
        {
            staminaReturned = 0;
        }

        // return final value
        return staminaReturned;
    }

    // Secondary Stats
    public static int GetTotalRangeOfRangedAttack(LivingEntity entity, Ability ability)
    {
        int rangeReturned = 0;

        // Get base range from ability
        rangeReturned = ability.abilityRange;

        // Get range bonus from hawk eye passive
        if (entity.myPassiveManager.hawkEye)
        {
            rangeReturned += entity.myPassiveManager.hawkEyeStacks;
        }

        // Get range bonus from TEMPORARY hawk eye passive
        if (entity.myPassiveManager.temporaryHawkEye)
        {
            rangeReturned += entity.myPassiveManager.temporaryHawkEyeStacks;
        }

        // return final value
        return rangeReturned;
    }
    public static int GetTotalDodge(LivingEntity entity)
    {
        // Get base Dodge
        int dodgeReturned = entity.currentDodgeChance;

        // Add temporary bonus Dodge
        dodgeReturned += entity.myPassiveManager.temporaryBonusDodgeStacks;

        // Add nimble
        if (entity.myPassiveManager.nimble)
        {
            dodgeReturned += 10;
        }

        // Add perfect reflexes
        if (entity.myPassiveManager.perfectReflexes)
        {
            dodgeReturned += 20;
        }

        // return final value
        return dodgeReturned;
    }
    public static int GetTotalParry(LivingEntity entity)
    {
        // Get base Parry
        int parryReturned = entity.currentParryChance;

        // Add temporary bonus Parry
        parryReturned += entity.myPassiveManager.temporaryBonusParryStacks;

        // Add nimble
        if (entity.myPassiveManager.nimble)
        {
            parryReturned += 10;
        }

        // Add perfect reflexes
        if (entity.myPassiveManager.perfectReflexes)
        {
            parryReturned += 20;
        }

        // return final value
        return parryReturned;
    }
    public static int GetTotalCriticalChance(LivingEntity entity, Ability ability = null)
    {
        // Get base Crit
        int criticalReturned = entity.currentCriticalChance;

        // Consider ability first
        if (ability != null)
        {
            if(ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
            {
                // TO DO!: Add melee specific crit modifiers here
                criticalReturned += GetTotalMeleeCriticalChance(entity);
            }
            else if (ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
            {
                // TO DO!: Add ranged specific crit modifiers here
                criticalReturned += GetTotalRangedCriticalChance(entity);
            }

            // Check for predator
            if (entity.myPassiveManager.predator)
            {
               if((ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack ||
               ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack) &&
               (entity.myPassiveManager.camoflage || entity.myPassiveManager.stealth)
               )
               {
                    criticalReturned += 20;
               }
            }
           

        }

        // Check for Patient Stalker
        if (entity.myPassiveManager.patientStalker &&
          (entity.myPassiveManager.camoflage || entity.myPassiveManager.stealth)
          )
        {
            criticalReturned += 20;
        }

        // Check for Masochist
        if (entity.myPassiveManager.masochist &&
           ((entity.currentMaxHealth / 2) > entity.currentHealth)
           )
        {
            criticalReturned += 20;
        }

        // return final value
        return criticalReturned;
    }
    public static int GetTotalMeleeCriticalChance(LivingEntity entity)
    {
        // Get base melee Crit
        int criticalReturned = 0;

        // Check recklessness
        if (entity.myPassiveManager.recklessness)
        {
            criticalReturned += 20;
        }

        // return final value
        return criticalReturned;
    }
    public static int GetTotalRangedCriticalChance(LivingEntity entity)
    {
        // Get base ranged Crit
        int criticalReturned = 0;

        // Check for 'Concentration' passive
        if (entity.myPassiveManager.concentration)
        {
            criticalReturned += 20;
        }

        // return final value
        return criticalReturned;
    }

    // Resistances
    public static int GetTotalResistance(LivingEntity entity, string resistanceType)
    {
        int targetResistance = 0;

        // get the targets resistance value
        if (resistanceType == "Physical")
        {
            targetResistance = GetTotalPhysicalResistance(entity);
        }
        else if (resistanceType == "Fire")
        {
            targetResistance = GetTotalFireResistance(entity);
        }
        else if (resistanceType == "Air")
        {
            targetResistance = GetTotalAirResistance(entity);
        }
        else if (resistanceType == "Poison")
        {
            targetResistance = GetTotalPoisonResistance(entity);
        }
        else if (resistanceType == "Shadow")
        {
            targetResistance = GetTotalShadowResistance(entity);
        }
        else if (resistanceType == "Frost")
        {
            targetResistance = GetTotalFrostResistance(entity);
        }

        // Check Infuse passive
        if (entity.myPassiveManager.infuse && 
            (resistanceType == "Physical" ||
             resistanceType == "Fire" ||
             resistanceType == "Frost" ||
             resistanceType == "Poison" ||
             resistanceType == "Shadow" ||
             resistanceType == "Air"))
        {
            targetResistance += 20;
        }

        // Finish
        return targetResistance;
    }
    public static int GetTotalPhysicalResistance(LivingEntity entity)
    {
        // Get base resistance
        int physicalResistanceReturned = entity.currentPhysicalResistance;

        // return final value
        return physicalResistanceReturned;
    }
    public static int GetTotalFireResistance(LivingEntity entity)
    {
        // Get base resistance
        int fireResistanceReturned = entity.currentFireResistance;

        // Check Demon passive
        if (entity.myPassiveManager.demon)
        {
            fireResistanceReturned += 20;
        }

        // return final value
        return fireResistanceReturned;
    }
    public static int GetTotalShadowResistance(LivingEntity entity)
    {
        // Get base resistance
        int shadowResistanceReturned = entity.currentShadowResistance;

        // Check Shadow Form passive
        if (entity.myPassiveManager.shadowForm)
        {
            shadowResistanceReturned += 20;
        }

        // return final value
        return shadowResistanceReturned;
    }
    public static int GetTotalFrostResistance(LivingEntity entity)
    {
        // Get base resistance
        int frostResistanceReturned = entity.currentFrostResistance;

        // Check Frozen Heart passive
        if (entity.myPassiveManager.frozenHeart)
        {
            frostResistanceReturned += 20;
        }

        // return final value
        return frostResistanceReturned;
    }
    public static int GetTotalPoisonResistance(LivingEntity entity)
    {
        // Get base resistance
        int poisonResistanceReturned = entity.currentPoisonResistance;

        // Check Toxicity passive
        if (entity.myPassiveManager.toxicity)
        {
            poisonResistanceReturned += 20;
        }

        // return final value
        return poisonResistanceReturned;
    }
    public static int GetTotalAirResistance(LivingEntity entity)
    {
        // Get base resistance
        int airResistanceReturned = entity.currentAirResistance;

        // Check Storm Lord passive
        if (entity.myPassiveManager.stormLord)
        {
            airResistanceReturned += 20;
        }

        // return final value
        return airResistanceReturned;
    }

    #endregion
}
