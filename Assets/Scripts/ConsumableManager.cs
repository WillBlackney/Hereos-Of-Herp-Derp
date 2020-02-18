using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableManager : MonoBehaviour
{
    public static ConsumableManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Component References")]
    public List<Consumable> activeConsumables;
    public ConsumableTopPanelSlot slotOne;
    public ConsumableTopPanelSlot slotTwo;
    public ConsumableTopPanelSlot slotThree;

    [Header("Consumable Order Properties")]
    public LivingEntity blinkPotionTarget;
    public bool awaitingAdrenalinePotionTarget;
    public bool awaitingBottledBrillianceTarget;
    public bool awaitingBottledMadnessTarget;
    public bool awaitingPotionOfClarityTarget;
    public bool awaitingPotionOfMightTarget;
    public bool awaitingVanishPotionTarget;
    public bool awaitingDynamiteTarget;
    public bool awaitingMolotovTarget;
    public bool awaitingBottledFrostTarget;
    public bool awaitingPoisonGrenadeTarget;
    public bool awaitingHandCannonTarget;
    public bool awaitingLovePotionTarget;
    public bool awaitingBlinkPotionCharacterTarget;
    public bool awaitingBlinkPotionDestinationTarget;


    public void StartGainConsumableProcess(ConsumableDataSO data)
    {
        Debug.Log("ConsumableManager.StartGainConsumableProcess() called for " + data.consumableName);

        // Check if gaining a consumable if actually possible (all slots full?)
        if (HasAtleastOneSlotAvailble())
        {
            Debug.Log("Player meets all conditions for gaining consumable...");
            GainConsumable(data);
        }
        else
        {
            Debug.Log("Player does not meet the requirments to gain a consumable...");
        }
    }
    public void GainConsumable(ConsumableDataSO data)
    {
        Debug.Log("ConsumableManager.GainConsumable() called, gaining " + data.consumableName);

        // Create GO and set slot parent
        ConsumableTopPanelSlot slot = GetNextAvailableSlot();
        GameObject newConsGO = Instantiate(PrefabHolder.Instance.consumable, slot.gameObject.transform);
        Consumable consumable = newConsGO.GetComponent<Consumable>();
        consumable.BuilFromConsumableData(data);
        activeConsumables.Add(consumable);

        // Modify slot availability
        consumable.mySlot = slot;
        slot.occupied = true;
        slot.HideSlotView();

    }
    public void RemoveConsumable(Consumable consumable)
    {
        activeConsumables.Remove(consumable);
        consumable.mySlot.occupied = false;
        consumable.mySlot.ShowSlotView();

        Destroy(consumable.gameObject);
    }
    public bool HasAtleastOneSlotAvailble()
    {
        Debug.Log("ConsumableManager.HasAtleastOneSlotAvailble() called...");

        bool boolReturned = false;

        if(!slotOne.occupied ||
            !slotTwo.occupied ||
            !slotThree.occupied)
        {
            Debug.Log("Player has at least one consumable slot availble");
            boolReturned = true;
        }
        else
        {
            Debug.Log("Player has no consumable slots availble");
            boolReturned = false;
        }

        return boolReturned;


    }
    public bool IsValidTimeToUseConsumable()
    {
        Debug.Log("ConsumableManager.IsValidTimeToUseConsumable() called...");

        bool boolReturned = false;
        if(ActivationManager.Instance.entityActivated == null)
        {
            Debug.Log("Can't use consumable: ActivationManager.entityActivated is null...");
        }
        else if (!ActivationManager.Instance.entityActivated.defender)
        {
            Debug.Log("Can't use consumable: the character currently activated is not a defender...");
        }
        else
        {
            Debug.Log("Consumable use is valid at the given moment...");
            boolReturned = true;
        }

        return boolReturned;
    }
    public ConsumableTopPanelSlot GetNextAvailableSlot()
    {
        Debug.Log("ConsumableManager.GetNextAvailableSlot() called...");

        ConsumableTopPanelSlot slotReturned = null;

        if (!slotOne.occupied)
        {
            Debug.Log("ConsumableManager.GetNextAvailableSlot() returning Slot One...");
            slotReturned = slotOne;
        }
        else if (!slotTwo.occupied)
        {
            Debug.Log("ConsumableManager.GetNextAvailableSlot() returning Slot Two...");
            slotReturned = slotTwo;
        }
        else if (!slotThree.occupied)
        {
            Debug.Log("ConsumableManager.GetNextAvailableSlot() returning Slot Three...");
            slotReturned = slotThree;
        }
        else
        {
            Debug.Log("ConsumableManager.GetNextAvailableSlot() could not find an availble slot, returning null...");
        }

        return slotReturned;
    }
    public void OnConsumableClicked(Consumable consumable)
    {
        Debug.Log("ConsumableManager.OnConsumableClicked() called...");

        if (IsValidTimeToUseConsumable())
        {
            StartConsumableUseProcess(consumable);
        }
    }
    public void StartConsumableUseProcess(Consumable consumable)
    {
        Debug.Log("ConsumableManager.OnConsumableClicked() called for " + consumable.myData.consumableName);

        // Cancel any awaiting ability order with current defender
        ActivationManager.Instance.entityActivated.defender.ClearAllOrders();
        LevelManager.Instance.UnhighlightAllTiles();
        TileHover.Instance.SetVisibility(true);

        if (consumable.myData.consumableName == "Adrenaline Potion")
        {
            awaitingAdrenalinePotionTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentDefenderLocations());
        }
        else if (consumable.myData.consumableName == "Bottled Brilliance")
        {
            awaitingBottledBrillianceTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentDefenderLocations());
        }
        else if (consumable.myData.consumableName == "Bottled Madness")
        {
            awaitingBottledMadnessTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentDefenderLocations());
        }
        else if (consumable.myData.consumableName == "Potion Of Clarity")
        {
            awaitingPotionOfClarityTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentDefenderLocations());
        }
        else if (consumable.myData.consumableName == "Potion Of Might")
        {
            awaitingPotionOfMightTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentDefenderLocations());
        }
        else if (consumable.myData.consumableName == "Vanish Potion")
        {
            awaitingVanishPotionTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentDefenderLocations());
        }
        else if (consumable.myData.consumableName == "Blink Potion")
        {
            awaitingBlinkPotionCharacterTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentDefenderLocations());
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentEnemyLocations());
        }
        else if (consumable.myData.consumableName == "Dynamite")
        {
            awaitingDynamiteTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary());
        }
        else if (consumable.myData.consumableName == "Molotov")
        {
            awaitingMolotovTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary());
        }
        else if (consumable.myData.consumableName == "Poison Grenade")
        {
            awaitingPoisonGrenadeTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary());
        }
        else if (consumable.myData.consumableName == "Bottled Frost")
        {
            awaitingBottledFrostTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary());
        }
        else if (consumable.myData.consumableName == "Hand Cannon")
        {
            awaitingHandCannonTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentEnemyLocations());
        }
        else if (consumable.myData.consumableName == "Love Potion")
        {
            awaitingLovePotionTarget = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetAllCurrentEnemyLocations());
        }
    }
    public Consumable GetActiveConsumableByName(string name)
    {
        Debug.Log("ConsumableManager.GetConsumableOnBarByName() called, looking for " + name);
        Consumable consumableReturned = null;

        foreach(Consumable consumable in activeConsumables)
        {
            if(consumable.myData.consumableName == name)
            {
                consumableReturned = consumable;
                break;
            }
        }

        if(consumableReturned == null)
        {
            Debug.Log("ConsumableManager.GetConsumableOnBarByName() could not find an active consumable called " + name +
                ", returning null...");
        }

        return consumableReturned;
    }
    public void ClearAllConsumableOrders()
    {
        Debug.Log("ConsumableManager.ClearAllConsumableOrders() called...");

        LevelManager.Instance.UnhighlightAllTiles();
        TileHover.Instance.SetVisibility(false);

        blinkPotionTarget = null;

        awaitingAdrenalinePotionTarget = false;
        awaitingBottledBrillianceTarget = false;
        awaitingBottledMadnessTarget = false;
        awaitingPotionOfClarityTarget = false;
        awaitingPotionOfMightTarget = false;
        awaitingVanishPotionTarget = false;
        awaitingDynamiteTarget = false;
        awaitingMolotovTarget = false;
        awaitingBottledFrostTarget = false;
        awaitingPoisonGrenadeTarget = false;
        awaitingHandCannonTarget = false;
        awaitingLovePotionTarget = false;
        awaitingBlinkPotionCharacterTarget = false;
        awaitingBlinkPotionDestinationTarget = false;
    }
    public void ApplyConsumableToTarget(LivingEntity target)
    {
        Debug.Log("ConsumableManager.ApplyConsumableToTarget() called for: " + target.myName);

        if (awaitingAdrenalinePotionTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() applying Adrenaline Potion to " + target.myName);

            target.ModifyCurrentEnergy(40);
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Adrenaline Potion!");
            RemoveConsumable(GetActiveConsumableByName("Adrenaline Potion"));
            ClearAllConsumableOrders();
        }
        else if (awaitingBottledBrillianceTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() applying Bottled Brilliance to " + target.myName);

            target.myPassiveManager.ModifyTemporaryWisdom(4);
            RemoveConsumable(GetActiveConsumableByName("Bottled Brilliance"));
            ClearAllConsumableOrders();
        }
        else if (awaitingBottledMadnessTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() applying Bottled Madness to " + target.myName);

            target.myPassiveManager.ModifyTemporaryStrength(4);
            RemoveConsumable(GetActiveConsumableByName("Bottled Madness"));
            ClearAllConsumableOrders();
        }
        else if (awaitingPotionOfMightTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() applying Potion Of Might to " + target.myName);

            target.myPassiveManager.ModifyBonusStrength(2);
            RemoveConsumable(GetActiveConsumableByName("Potion Of Might"));
            ClearAllConsumableOrders();
        }
        else if (awaitingVanishPotionTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() applying Vanish Potion to " + target.myName);

            target.myPassiveManager.ModifyCamoflage(1);
            RemoveConsumable(GetActiveConsumableByName("Vanish Potion"));
            ClearAllConsumableOrders();
        }
        else if (awaitingPotionOfClarityTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() applying Potion Of Clarity to " + target.myName);

            target.myPassiveManager.ModifyBonusWisdom(2);
            RemoveConsumable(GetActiveConsumableByName("Potion Of Clarity"));
            ClearAllConsumableOrders();
        }
        else if (awaitingHandCannonTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() applying Hand Cannon to " + target.myName);

            RemoveConsumable(GetActiveConsumableByName("Hand Cannon"));
            ClearAllConsumableOrders();
            PerformHandCannon(target);
        }
        else if (awaitingLovePotionTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() applying Love Potion to " + target.myName);

            RemoveConsumable(GetActiveConsumableByName("Love Potion"));
            ClearAllConsumableOrders();
            PerformLovePotion(target);
        }

    }
    public void ApplyConsumableToTarget(Tile location)
    {
        Debug.Log("ConsumableManager.ApplyConsumableToTarget() called for tile " +
            location.GridPosition.X + ", " + location.GridPosition.Y);

        if (awaitingDynamiteTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() using Dynamite on tile " +
            location.GridPosition.X + ", " + location.GridPosition.Y);

            RemoveConsumable(GetActiveConsumableByName("Dynamite"));
            ClearAllConsumableOrders();
            PerformDynamite(location);
        }
        else if (awaitingMolotovTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() using Molotov on tile " +
            location.GridPosition.X + ", " + location.GridPosition.Y);

            RemoveConsumable(GetActiveConsumableByName("Molotov"));
            ClearAllConsumableOrders();
            PerformMolotov(location);
        }
        else if (awaitingBottledFrostTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() using Bottled Frost on tile " +
            location.GridPosition.X + ", " + location.GridPosition.Y);

            RemoveConsumable(GetActiveConsumableByName("Bottled Frost"));
            ClearAllConsumableOrders();
            PerformBottledFrost(location);
        }
        else if (awaitingPoisonGrenadeTarget)
        {
            Debug.Log("ConsumableManager.ApplyConsumableToTarget() using Poison Grenade on tile " +
            location.GridPosition.X + ", " + location.GridPosition.Y);

            RemoveConsumable(GetActiveConsumableByName("Poison Grenade"));
            ClearAllConsumableOrders();
            PerformPoisonGrenade(location);
        }
    }
    public void BuyConsumableFromShop(ConsumableInShop consumable)
    {
        Debug.Log("CnsumableManager.BuyConsumableFromShop() called...");

        if (PlayerDataManager.Instance.currentGold >= consumable.goldCost && HasAtleastOneSlotAvailble())
        {
            Debug.Log("Buying Consumable " + consumable.myData.consumableName + " for " + consumable.goldCost.ToString());            
            PlayerDataManager.Instance.ModifyGold(-consumable.goldCost);
            consumable.DisableSlotView();
            GainConsumable(consumable.myData);
        }
        else
        {
            Debug.Log("Cannot buy consumable: Not enough gold, or no empty consumable slots...");
        }
    }

    // Perform Specific Consumable Actions
    #region
    public Action PerformDynamite(Tile location)
    {
        Debug.Log("ConsumableManager.PerformDynamite() called...");
        Action action = new Action();
        StartCoroutine(PerformDynamiteCoroutine(location, action));
        return action;

    }
    private IEnumerator PerformDynamiteCoroutine(Tile location, Action action)
    {
        // Calculate which characters are hit by the aoe
        List<Tile> tilesInBlastRadius = LevelManager.Instance.GetTilesWithinRange(1, location, false);
        List<LivingEntity> charactersInBlastRadius = new List<LivingEntity>();

        // Get characters in blast radius
        foreach(LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInBlastRadius.Contains(entity.tile))
            {
                charactersInBlastRadius.Add(entity);
            }
        }

        // Damage all characters within the blast radius
        foreach(LivingEntity entity in charactersInBlastRadius)
        {
            int finalDamageValue = CombatLogic.Instance.GetDamageValueAfterResistances(10, "Physical", entity);

            Action damageAction = CombatLogic.Instance.HandleDamage(finalDamageValue, null, entity, "Physical");
            yield return new WaitUntil(() => damageAction.ActionResolved() == true);
        }

        // Resolve and Finish
        action.actionResolved = true;

    }
    public Action PerformMolotov(Tile location)
    {
        Debug.Log("ConsumableManager.PerformDynamite() called...");
        Action action = new Action();
        StartCoroutine(PerformMolotovCoroutine(location, action));
        return action;

    }
    private IEnumerator PerformMolotovCoroutine(Tile location, Action action)
    {
        // Calculate which characters are hit by the aoe
        List<Tile> tilesInBlastRadius = LevelManager.Instance.GetTilesWithinRange(1, location, false);
        List<LivingEntity> charactersInBlastRadius = new List<LivingEntity>();

        // Get characters in blast radius
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInBlastRadius.Contains(entity.tile))
            {
                charactersInBlastRadius.Add(entity);
            }
        }

        // Damage all characters within the blast radius
        foreach (LivingEntity entity in charactersInBlastRadius)
        {
            int finalDamageValue = CombatLogic.Instance.GetDamageValueAfterResistances(8, "Fire", entity);

            Action damageAction = CombatLogic.Instance.HandleDamage(finalDamageValue, null, entity, "Fire");
            yield return new WaitUntil(() => damageAction.ActionResolved() == true);

            if(entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyBurning(1);
            }
        }

        // Resolve and Finish
        action.actionResolved = true;

    }
    public Action PerformBottledFrost(Tile location)
    {
        Debug.Log("ConsumableManager.PerformBottledFrost() called...");
        Action action = new Action();
        StartCoroutine(PerformBottledFrostCoroutine(location, action));
        return action;

    }
    private IEnumerator PerformBottledFrostCoroutine(Tile location, Action action)
    {
        // Calculate which characters are hit by the aoe
        List<Tile> tilesInBlastRadius = LevelManager.Instance.GetTilesWithinRange(1, location, false);
        List<LivingEntity> charactersInBlastRadius = new List<LivingEntity>();

        // Get characters in blast radius
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInBlastRadius.Contains(entity.tile))
            {
                charactersInBlastRadius.Add(entity);
            }
        }

        // Apply 'Chilled' to all characters in blast radius
        foreach (LivingEntity entity in charactersInBlastRadius)
        {
            entity.myPassiveManager.ModifyChilled(1);            
        }

        yield return new WaitForSeconds(0.5f);

        // Resolve and Finish
        action.actionResolved = true;

    }
    public Action PerformPoisonGrenade(Tile location)
    {
        Debug.Log("ConsumableManager.PerformPoisonGrenade() called...");
        Action action = new Action();
        StartCoroutine(PerformPoisonGrenadeCoroutine(location, action));
        return action;

    }
    private IEnumerator PerformPoisonGrenadeCoroutine(Tile location, Action action)
    {
        // Calculate which characters are hit by the aoe
        List<Tile> tilesInBlastRadius = LevelManager.Instance.GetTilesWithinRange(1, location, false);
        List<LivingEntity> charactersInBlastRadius = new List<LivingEntity>();

        // Get characters in blast radius
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInBlastRadius.Contains(entity.tile))
            {
                charactersInBlastRadius.Add(entity);
            }
        }

        // Apply 3 'Poisoned' to all characters in blast radius
        foreach (LivingEntity entity in charactersInBlastRadius)
        {
            entity.myPassiveManager.ModifyPoisoned(3);
        }

        yield return new WaitForSeconds(0.5f);

        // Resolve and Finish
        action.actionResolved = true;

    }
    public Action PerformHandCannon(LivingEntity target)
    {
        Debug.Log("ConsumableManager.PerformHandCannon() called...");
        Action action = new Action();
        StartCoroutine(PerformHandCannonCoroutine(target, action));
        return action;

    }
    private IEnumerator PerformHandCannonCoroutine(LivingEntity target, Action action)
    {
        // Setup
        int finalDamageValue = CombatLogic.Instance.GetDamageValueAfterResistances(15, "Physical", target);

        // Damage Event
        Action damageAction = CombatLogic.Instance.HandleDamage(finalDamageValue, null, target, "Physical");
        yield return new WaitUntil(() => damageAction.ActionResolved() == true);

        // Resolve and Finish
        action.actionResolved = true;

    }
    public Action PerformLovePotion(LivingEntity target)
    {
        Debug.Log("ConsumableManager.PerformLovePotion() called...");
        Action action = new Action();
        StartCoroutine(PerformLovePotionCoroutine(target, action));
        return action;

    }
    private IEnumerator PerformLovePotionCoroutine(LivingEntity target, Action action)
    {
        // Apply Stunned
        target.myPassiveManager.ModifyStunned(1);
        yield return new WaitForSeconds(0.5f);
        // Resolve and Finish
        action.actionResolved = true;

    }
    public void StartBlinkPotionLocationSettingProcess(LivingEntity target)
    {
        LevelManager.Instance.UnhighlightAllTiles();

        blinkPotionTarget = target;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetValidMoveableTilesWithinRange(2, blinkPotionTarget.tile, true));
        awaitingBlinkPotionCharacterTarget = false;
        awaitingBlinkPotionDestinationTarget = true;
    }
    public Action PerformBlinkPotion(Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformBlinkPotionCoroutine(location, action));
        return action;
    }
    public IEnumerator PerformBlinkPotionCoroutine(Tile location, Action action)
    {
        RemoveConsumable(GetActiveConsumableByName("Blink Potion"));        

        Action teleportAction = MovementLogic.Instance.TeleportEntity(blinkPotionTarget, location);
        ClearAllConsumableOrders();
        yield return new WaitUntil(() => teleportAction.ActionResolved() == true);

        action.actionResolved = true;
    }
    #endregion
}
