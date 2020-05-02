using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardScreenManager : MonoBehaviour
{
    // Singleton Pattern
    #region
    public static CardRewardScreenManager Instance;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Properties + Component References
    #region
    [Header("Position References")]
    public RectTransform cardParent;
    public RectTransform charRosterButtonEndPosition;
    public RectTransform statePanelEndPosition;

    [Header("Prefab References")]
    public GameObject itemCardEffectPrefab;
    public GameObject stateCardEffectPrefab;
    public GameObject abilityCardEffectPrefab;
    public GameObject lootExplosionEffectPrefab;
    public GameObject talentUnlockedExplosionEffectPrefab;

    [Header("Properties")]
    public int itemCardSortingOrder;
    public float itemCardScalingSpeed;
    public float itemCardMoveSpeed;
    public float itemCardRotationSpeed;
    public float afflictionCardFadeOutSpeed;

    [Header("Slot Card References")]
    public CardRewardScreenSlot centreSlot;
    public CardRewardScreenSlot leftSlot;
    public CardRewardScreenSlot rightSlot;
    #endregion

    // Initialization + Setup
    #region
    public void BuildItemCardRewardOverlayFromData(ItemDataSO data, ItemCardRewardOverlay card)
    {
        ItemManager.Instance.SetUpItemCardFromData(card.itemCard, data, itemCardSortingOrder);
    }
    public void BuildStateCardRewardOverlayFromData(StateDataSO data, StateCardRewardOverlay card)
    {
        card.stateCard.BuildFromStateData(data);
    }
    public void BuildAbilityCardRewardOverlayFromData(AbilityDataSO data, AbilityCardRewardOverlay card)
    {
        AbilityInfoSheetController.Instance.BuildSheetFromData(card.abilityCard, data, AbilityInfoSheet.PivotDirection.Downwards);
    }

    #endregion

    // Create card Overlays
    #region
    public Action CreateAbilityCardRewardEffect(AbilityDataSO abilityData)
    {
        Action action = new Action();
        StartCoroutine(CreateAbilityCardRewardEffectCoroutine(abilityData, action));
        return action;
    }
    private IEnumerator CreateAbilityCardRewardEffectCoroutine(AbilityDataSO abilityData, Action action)
    {
        // Create Card object
        GameObject newAbilityCardGO = Instantiate(abilityCardEffectPrefab, cardParent);
        AbilityCardRewardOverlay newAbilityCard = newAbilityCardGO.GetComponent<AbilityCardRewardOverlay>();

        // Set up card properties
        BuildAbilityCardRewardOverlayFromData(abilityData, newAbilityCard);

        // Scale down card
        SetParentScale(newAbilityCard.abilityCardScaleParent, new Vector3(0.005f, 0.005f, 0.005f));

        // Insert Delay, break up multiple simoltaenous reveals
        yield return new WaitForSeconds(CalculateCardRevealDelay());

        // Get next available slot position
        CardRewardScreenSlot mySlot = GetNextAvailableSlotPosition();

        // dont proceed unless a slot is available
        if (mySlot == null)
        {
            action.actionResolved = true;
            yield break;
        }

        // Move to starting slot position
        newAbilityCard.masterLocationParent.position = mySlot.parentTransform.position;

        // Start scaling card back to normal size
        Action scaleEffect = ScaleUpCard(newAbilityCard.abilityCardScaleParent, 1);
        yield return new WaitUntil(() => scaleEffect.actionResolved == true);

        // Brief pause, so player can read the card
        yield return new WaitForSeconds(1);

        // Free up slot
        mySlot.occupied = false;

        // Start scaling card back down
        ScaleDownCard(newAbilityCard.abilityCardScaleParent, 0.01f);

        // Start card rotation towards destination
        RotateCardZAxis(newAbilityCard.abilityCardScaleParent, -45);

        // Enable Sparkle Effect
        newAbilityCard.sparkleParticleEffectParent.SetActive(true);

        // Move character panel button position
        Action moveEffect = MoveCardToDestination(newAbilityCard.masterLocationParent, charRosterButtonEndPosition.position);
        yield return new WaitUntil(() => moveEffect.actionResolved == true);

        // Create mini explosion on char roster button
        GameObject newPE = Instantiate(lootExplosionEffectPrefab, cardParent);
        newPE.transform.position = charRosterButtonEndPosition.position;

        // Destroy item card
        Destroy(newAbilityCardGO);

        // Resolve
        action.actionResolved = true;
    }
    public Action CreateItemCardRewardEffect(ItemDataSO itemData)
    {
        Action action = new Action();
        StartCoroutine(CreateItemCardRewardEffectCoroutine(itemData, action));
        return action;
    }
    private IEnumerator CreateItemCardRewardEffectCoroutine(ItemDataSO itemData, Action action)
    {
        // Create Card object
        GameObject newItemCardGO = Instantiate(itemCardEffectPrefab, cardParent);
        ItemCardRewardOverlay newItemCard = newItemCardGO.GetComponent<ItemCardRewardOverlay>();

        // Set up card properties
        BuildItemCardRewardOverlayFromData(itemData, newItemCard);

        // Scale down card
        SetParentScale(newItemCard.itemCardScaleParent, new Vector3(0.005f, 0.005f, 0.005f));

        // Insert Delay, break up multiple simoltaenous reveals
        yield return new WaitForSeconds(CalculateCardRevealDelay());

        // Get next available slot position
        CardRewardScreenSlot mySlot = GetNextAvailableSlotPosition();

        // dont proceed unless a slot is available
        if(mySlot == null)
        {
            action.actionResolved = true;
            yield break;
        }

        // Move to starting slot position
        newItemCard.masterLocationParent.position = mySlot.parentTransform.position;       

        // Start scaling card back to normal size
        Action scaleEffect = ScaleUpCard(newItemCard.itemCardScaleParent, 1);
        yield return new WaitUntil(() => scaleEffect.actionResolved == true);

        // Brief pause, so player can read the card
        yield return new WaitForSeconds(1);

        // Free up slot
        mySlot.occupied = false;

        // Start scaling card back down
        ScaleDownCard(newItemCard.itemCardScaleParent, 0.01f);

        // Start card rotation towards destination
        RotateCardZAxis(newItemCard.itemCardScaleParent, -45);

        // Enable Sparkle Effect
        newItemCard.sparkleParticleEffectParent.SetActive(true);

        // Move character panel button position
        Action moveEffect = MoveCardToDestination(newItemCard.masterLocationParent, charRosterButtonEndPosition.position);
        yield return new WaitUntil(() => moveEffect.actionResolved == true);

        // Create mini explosion on char roster button
        GameObject newPE = Instantiate(lootExplosionEffectPrefab, cardParent);
        newPE.transform.position = charRosterButtonEndPosition.position;

        // Destroy item card
        Destroy(newItemCardGO);

        // Resolve
        action.actionResolved = true;
    }
    public Action CreateStateCardRewardEffect(StateDataSO stateData)
    {
        Action action = new Action();
        StartCoroutine(CreateStateCardRewardEffectCoroutine(stateData, action));
        return action;

    }
    private IEnumerator CreateStateCardRewardEffectCoroutine(StateDataSO stateData, Action action)
    {
        // Create Card object
        GameObject newStateCardGO = Instantiate(stateCardEffectPrefab, cardParent);
        StateCardRewardOverlay newStateCard = newStateCardGO.GetComponent<StateCardRewardOverlay>();

        // Set up card properties
        BuildStateCardRewardOverlayFromData(stateData, newStateCard);

        // Scale down card
        SetParentScale(newStateCard.stateCardScaleParent, new Vector3(0.005f, 0.005f, 0.005f));

        // Insert Delay, break up multiple simoltaenous reveals
        yield return new WaitForSeconds(CalculateCardRevealDelay());

        // Get next available slot position
        CardRewardScreenSlot mySlot = GetNextAvailableSlotPosition();

        // dont proceed unless a slot is available
        if (mySlot == null)
        {
            action.actionResolved = true;
            yield break;
        }

        // Move to starting slot position
        newStateCard.masterLocationParent.position = mySlot.parentTransform.position;

        // Start scaling card back to normal size
        Action scaleEffect = ScaleUpCard(newStateCard.stateCardScaleParent, 1);
        yield return new WaitUntil(() => scaleEffect.actionResolved == true);

        // Brief pause, so player can read the card
        yield return new WaitForSeconds(1);

        // Free up slot
        mySlot.occupied = false;

        // Start scaling card back down
        ScaleDownCard(newStateCard.stateCardScaleParent, 0.01f);

        // Start card rotation towards destination
        RotateCardZAxis(newStateCard.stateCardScaleParent, 45);

        // Enable Sparkle Effect
        newStateCard.sparkleParticleEffectParent.SetActive(true);

        // Move character panel button position
        Action moveEffect = MoveCardToDestination(newStateCard.masterLocationParent, statePanelEndPosition.position);
        yield return new WaitUntil(() => moveEffect.actionResolved == true);

        // Create mini explosion on char roster button
        GameObject newPE = Instantiate(lootExplosionEffectPrefab, cardParent);
        newPE.transform.position = statePanelEndPosition.position;

        // Destroy item card
        Destroy(newStateCardGO);

        // Resolve
        action.actionResolved = true;

        yield return null;
        action.actionResolved = true;
    }
    public Action CreateAfflictionCardRemovedEffect(StateDataSO stateData)
    {
        Action action = new Action();
        StartCoroutine(CreateAfflictionCardRemovedEffect(stateData, action));
        return action;
    }
    private IEnumerator CreateAfflictionCardRemovedEffect(StateDataSO stateData, Action action)
    {
        // Create Card object
        GameObject newStateCardGO = Instantiate(stateCardEffectPrefab, cardParent);
        StateCardRewardOverlay newStateCard = newStateCardGO.GetComponent<StateCardRewardOverlay>();

        // Set up card properties
        BuildStateCardRewardOverlayFromData(stateData, newStateCard);

        // Scale down card
        SetParentScale(newStateCard.stateCardScaleParent, new Vector3(0.005f, 0.005f, 0.005f));

        // Insert Delay, break up multiple simoltaenous reveals
        yield return new WaitForSeconds(CalculateCardRevealDelay());

        // Get next available slot position
        CardRewardScreenSlot mySlot = GetNextAvailableSlotPosition();

        // dont proceed unless a slot is available
        if (mySlot == null)
        {
            action.actionResolved = true;
            yield break;
        }

        // Move to starting slot position
        newStateCard.masterLocationParent.position = mySlot.parentTransform.position;

        // Start scaling card back to normal size
        Action scaleEffect = ScaleUpCard(newStateCard.stateCardScaleParent, 1);
        yield return new WaitUntil(() => scaleEffect.actionResolved == true);

        // Brief pause, so player can read the card
        yield return new WaitForSeconds(0.5f);

        // Free up slot
        mySlot.occupied = false;

        // Start Card Fade out
        FadeOutStateCard(newStateCard);

        // Start card burn VFX
        newStateCard.fireDestroyParticleEffectParent.SetActive(true);

        // Destroy
        Destroy(newStateCard, 2);

        // Resolve
        action.actionResolved = true;
    }

    public void CreateTalentUnlockedExplosion(Talent talent)
    {
        GameObject talentExplosionGO = Instantiate(talentUnlockedExplosionEffectPrefab, talent.talentImage.gameObject.transform);
        Destroy(talentExplosionGO, 3);
        //Vector3 talentCentrePos = talent.myGlowOutline.transform.position;
       // Vector3 screenPos = 
    }
    #endregion

    // Transform + Scaling logic
    #region
    public void SetParentScale(RectTransform cardParent, Vector3 newScale)
    {
        cardParent.localScale = newScale;
    }
    public Action ScaleUpCard(RectTransform transformScaled, float targetScale)
    {
        Action action = new Action();
        StartCoroutine(ScaleUpCardRectCoroutine(transformScaled, targetScale, action));
        return action;

    }
    private IEnumerator ScaleUpCardRectCoroutine(Transform transformScaled, float targetScale, Action action)
    {
        // Just check x local scale, no need to check y or z
        while(transformScaled != null &&  transformScaled.localScale.x != targetScale)
        {
            // Increase scale
            transformScaled.localScale =
                new Vector3(
                transformScaled.localScale.x + itemCardScalingSpeed,
                transformScaled.localScale.y + itemCardScalingSpeed,
                transformScaled.localScale.z + itemCardScalingSpeed);

            // Prevent over scaling
            if(transformScaled.localScale.x > targetScale)
            {
                // Snap to final scale size
                transformScaled.localScale = new Vector3(targetScale, targetScale, targetScale);
            }

            yield return new WaitForEndOfFrame();
        }

        // Resolve
        action.actionResolved = true;
    }
    public Action ScaleDownCard(RectTransform transformScaled, float targetScale)
    {
        Action action = new Action();
        StartCoroutine(ScaleDownCardRectCoroutine(transformScaled, targetScale, action));
        return action;

    }
    private IEnumerator ScaleDownCardRectCoroutine(Transform transformScaled, float targetScale, Action action)
    {
        // Just check x local scale, no need to check y or z
        while (transformScaled != null && transformScaled.localScale.x != targetScale)
        {
            // Increase scale
            transformScaled.localScale =
                new Vector3(
                transformScaled.localScale.x - itemCardScalingSpeed,
                transformScaled.localScale.y - itemCardScalingSpeed,
                transformScaled.localScale.z - itemCardScalingSpeed);

            // Prevent over scaling ito negative
            if (transformScaled.localScale.x < targetScale)
            {
                // Snap to final scale size
                transformScaled.localScale = new Vector3(targetScale, targetScale, targetScale);
            }

            yield return new WaitForEndOfFrame();
        }

        // Resolve
        action.actionResolved = true;
    }
    public Action RotateCardZAxis(RectTransform transformScaled, float targetRotation)
    {
        Action action = new Action();
        StartCoroutine(RotateCardZAxisCoroutine(transformScaled, targetRotation, action));
        return action;

    }
    private IEnumerator RotateCardZAxisCoroutine(Transform transformScaled, float targetRotation, Action action)
    {
        float runningTime = 0;
        bool rotationFinished = false;

        while (rotationFinished == false && transformScaled != null)
        {
            runningTime += Time.deltaTime;
            Debug.Log("Current running time of rotation: " + runningTime.ToString());

            // check direction of rotation (- or +)
            if(targetRotation < 0)
            {
                // Rotate
                if(transformScaled != null)
                {
                    transformScaled.transform.Rotate(0, 0, transformScaled.rotation.z - itemCardRotationSpeed * Time.deltaTime);
                }
               

                // prevent over rotation
                if (runningTime > 0.5f) 
                {
                    rotationFinished = true;
                }
            }

            else if(targetRotation > 0)
            {
                // Rotate
                if (transformScaled != null)
                {
                    transformScaled.transform.Rotate(0, 0, transformScaled.rotation.z + itemCardRotationSpeed * Time.deltaTime);
                }                    

                // prevent over rotation
                if (runningTime > 0.5f)
                {
                    rotationFinished = true;
                }
               
            }
            yield return new WaitForEndOfFrame();

        }

        Debug.Log("Rotation finished");

        // Resolve
        action.actionResolved = true;
    }
    #endregion

    // View Logic
    #region
    public Action FadeOutStateCard(StateCardRewardOverlay stateCard)
    {
        Action action = new Action();
        StartCoroutine(FadeOutStateCardCoroutine(stateCard, action));
        return action;
    }
    private IEnumerator FadeOutStateCardCoroutine(StateCardRewardOverlay stateCard, Action action)
    {
        // Start fade out
        while (stateCard.canvasGroup.alpha > 0)
        {
            stateCard.canvasGroup.alpha -= afflictionCardFadeOutSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Resolve
        action.actionResolved = true;
    }
    #endregion

    // Movement Logic
    #region
    public Action MoveCardToDestination(RectTransform cardParent, Vector3 destination)
    {
        Action action = new Action();
        StartCoroutine(MoveCardToDestinationCoroutine(cardParent, destination, action));
        return action;
    }
    private IEnumerator MoveCardToDestinationCoroutine(RectTransform cardParent, Vector3 destination, Action action)
    {
        // Compare x and y axis, dont check z
        while (cardParent.position.x != destination.x ||
            cardParent.position.y != destination.y)
        {
            // Move card position towards destination
            cardParent.position = Vector3.MoveTowards(cardParent.position, destination, itemCardMoveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // Resolve
        action.actionResolved = true;
    }
    #endregion

    // Card Slot Logic
    #region
    public CardRewardScreenSlot GetNextAvailableSlotPosition()
    {
        if (!centreSlot.occupied)
        {
            centreSlot.occupied = true;
            return centreSlot;
        }
        else if (!rightSlot.occupied)
        {
            rightSlot.occupied = true;
            return rightSlot;
        }
        else if (!leftSlot.occupied)
        {
            leftSlot.occupied = true;
            return leftSlot;
        }
        else
        {
            Debug.Log("WARNING: CardRewardScreenManager.GetNextAvailableSlotPosition() returning a null slot...");            
            return null;
        }
       
    }    
    public float CalculateCardRevealDelay()
    {
        float delayReturned = 0;

        if (centreSlot.occupied)
        {
            delayReturned += 0.25f;
        }
        if (rightSlot.occupied)
        {
            delayReturned += 0.5f;
        }
        if (leftSlot.occupied)
        {
            delayReturned += 0.5f;
        }

        Debug.Log("CardRewardScreenManager.CalculateCardRevealDelay() returning float value: " + delayReturned.ToString());
        return delayReturned;
    }
    #endregion

    

}
