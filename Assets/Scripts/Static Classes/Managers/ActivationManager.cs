using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActivationManager : MonoBehaviour
{
    [Header("Component References")]
    public GameObject activationSlotContentParent;
    public GameObject activationWindowContentParent;
    public GameObject windowStartPos;
    public GameObject activationPanelParent;
    public GameObject panelArrow;
    public GameObject panelSlotPrefab;
    public GameObject slotHolderPrefab;
    public GameObject windowHolderPrefab;

    [Header("Properties")]
    public List<LivingEntity> activationOrder;
    public List<GameObject> panelSlots;
    public LivingEntity entityActivated;
    public bool panelIsMousedOver;
    public bool updateWindowPositions;
    

    // Setup + Initializaton
    #region   
    public void CreateActivationWindow(LivingEntity entity)
    {
        GameObject newSlot = Instantiate(panelSlotPrefab, activationSlotContentParent.transform);
        panelSlots.Add(newSlot);

        GameObject newWindow = Instantiate(PrefabHolder.Instance.activationWindowPrefab, activationWindowContentParent.transform);
        newWindow.transform.position = windowStartPos.transform.position;      
        
        ActivationWindow newWindowScript = newWindow.GetComponent<ActivationWindow>();
        newWindowScript.InitializeSetup(entity);        
        activationOrder.Add(entity);
        newWindowScript.myUCM.SetBaseAnim();
    }
    public static ActivationManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Events
    #region
    public Action OnNewCombatEventStarted()
    {        
        Action action = new Action();
        StartCoroutine(OnNewCombatEventStartedCoroutine(action));
        return action;
    }
    public IEnumerator OnNewCombatEventStartedCoroutine(Action action)
    {
        TurnManager.Instance.currentTurnCount = 0;
        SetActivationWindowViewState(true);
        StartNewTurnSequence();
        action.actionResolved = true;
        yield return null;
    }
    public void CreateSlotAndWindowHolders()
    {        
        activationSlotContentParent = Instantiate(slotHolderPrefab, activationPanelParent.transform);
        activationWindowContentParent = Instantiate(windowHolderPrefab, activationPanelParent.transform);
        updateWindowPositions = true;
    }
    public Action StartNewTurnSequence()
    {
        Debug.Log("ActivationManager.StartNewTurnSequenceCalled()....");
        Action action = new Action();
        StartCoroutine(StartNewTurnSequenceCoroutine(action));
        return action;
    }
    public IEnumerator StartNewTurnSequenceCoroutine(Action action)
    {
        TurnManager.Instance.currentTurnCount++;

        // Resolve each entity's OnNewTurnCycleStarted events
        foreach(LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            Action endTurnCycleEvent = entity.OnNewTurnCycleStarted();
            yield return new WaitUntil(() => endTurnCycleEvent.ActionResolved());
        }

        Action rolls = CalculateActivationOrder();
        yield return new WaitUntil(() => rolls.ActionResolved() == true);

        Action turnNotification = TurnManager.Instance.DisplayTurnChangeNotification();
        yield return new WaitUntil(() => turnNotification.ActionResolved());        

        ActivateEntity(activationOrder[0]);
        action.actionResolved = true;
    }
    public void ClearAllWindowsFromActivationPanel()
    {
        updateWindowPositions = false;
        foreach(LivingEntity entity in activationOrder)
        {
            if(entity.myActivationWindow != null)
            {

                entity.myActivationWindow.DestroyWindowOnCombatEnd();
            }
            
        }

        activationOrder.Clear();
        panelSlots.Clear();
        Destroy(activationSlotContentParent);
        Destroy(activationWindowContentParent);        
        
    }
    #endregion

    // Logic + Calculations
    #region
    private void Update()
    {
        MoveArrowTowardsEntityActivatedWindow();
    }
    public int CalculateInitiativeRoll(LivingEntity entity)
    {
        return EntityLogic.GetTotalInitiative(entity) + Random.Range(1, 4);
    }
    public Action CalculateActivationOrder()
    {
        Action action = new Action();
        StartCoroutine(CalculateActivationOrderCoroutine(action));
        return action;
    }
    public IEnumerator CalculateActivationOrderCoroutine(Action action)
    {
        // Disable arrow to prevtn blocking numbers
        panelArrow.SetActive(false);

        foreach (LivingEntity entity in activationOrder)
        {
            // generate the characters initiative roll
            entity.currentInitiativeRoll = CalculateInitiativeRoll(entity);
            // start animating their roll number text
            StartCoroutine(PlayRandomNumberAnim(entity.myActivationWindow));            
        }

        yield return new WaitForSeconds(1);

        foreach (LivingEntity entity in activationOrder)
        {
            // stop the number anim
            entity.myActivationWindow.animateNumberText = false;
            // set the number text as their initiative roll
            entity.myActivationWindow.rollText.text = entity.currentInitiativeRoll.ToString();
            yield return new WaitForSeconds(0.3f);
        }

        // Re arrange the activation order list based on the entity rolls
        List<LivingEntity> sortedList = activationOrder.OrderBy(entity => entity.currentInitiativeRoll).ToList();
        sortedList.Reverse();
        activationOrder = sortedList;

        // Move activation windows to their new positions
        //ArrangeActivationWindowPositions();
        yield return new WaitForSeconds(1f);

        // Disable roll number text components
        foreach(LivingEntity entity in activationOrder)
        {
            entity.myActivationWindow.rollText.enabled = false;
        }

        panelArrow.SetActive(true);
        action.actionResolved = true;        
    }  
   
    #endregion 

    // Player Input + UI interactions + Visual
    #region
    public void OnEndTurnButtonClicked()
    {
        if (!ActionManager.Instance.UnresolvedCombatActions())
        {
            StartCoroutine(OnEndTurnButtonClickedCoroutine());
        }
        
    }
    public IEnumerator OnEndTurnButtonClickedCoroutine()
    {
        Debug.Log("OnEndTurnButtonClickedCoroutine() started...");

        UIManager.Instance.DisableEndTurnButtonInteractions();             
        // endplayer turn will trigger all player end turn effects, BEFORE switching to enemy turn
        Action endTurnEvent = EndEntityActivation(entityActivated);
        yield return new WaitUntil(() => endTurnEvent.ActionResolved() == true);        
        ActivateNextEntity();

    }
    public IEnumerator PlayRandomNumberAnim(ActivationWindow window)
    {
        Debug.Log("PlayRandomNumberAnim() called....");
        int numberDisplayed = 0;
        window.animateNumberText = true;
        window.rollText.enabled = true;

        while (window.animateNumberText == true)
        {
            //Debug.Log("Animating roll number text....");
            numberDisplayed++;
            if (numberDisplayed > 9)
            {
                numberDisplayed = 0;
            }
            window.rollText.text = numberDisplayed.ToString();
            
            yield return new WaitForEndOfFrame();
        }
    }
    public Action MoveArrowTowardsTargetPanelPos(ActivationWindow window, float moveDelay = 0f, float arrowMoveSpeed = 400)
    {
        Debug.Log("ActivationManager.MoveArrowTowardsTargetPanelPos() called....");
        Action action = new Action();
        StartCoroutine(MoveArrowTowardsTargetPanelPosCoroutine(window, action, moveDelay, arrowMoveSpeed));
        return action;
    }
    public IEnumerator MoveArrowTowardsTargetPanelPosCoroutine(ActivationWindow window, Action action, float moveDelay = 0, float arrowMoveSpeed = 400)
    {        
        yield return new WaitForSeconds(moveDelay);
        Vector3 destination = new Vector2(window.transform.position.x, panelArrow.transform.position.y);

        while (panelArrow.transform.position != destination)
        {
            panelArrow.transform.position = Vector2.MoveTowards(panelArrow.transform.position, destination, arrowMoveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        action.actionResolved = true;
    }
    public void MoveArrowTowardsEntityActivatedWindow()
    {
        int currentActivationIndex = 0;

        if(activationOrder.Count > 0)
        {
            for (int i = 0; i < activationOrder.Count; i++)
            {
                //Check if GameObject is in the List
                if (activationOrder[i] == entityActivated)
                {
                    //It is. Return the current index
                    currentActivationIndex = i;
                    break;
                }
            }

            if (panelSlots[currentActivationIndex] != null && panelSlots.Count > 0)
            {
                Vector3 destination = new Vector2(panelSlots[currentActivationIndex].transform.position.x, panelArrow.transform.position.y);
                panelArrow.transform.position = Vector2.MoveTowards(panelArrow.transform.position, destination, 400 * Time.deltaTime);
            }
        }
       


    }
    public void SetActivationWindowViewState(bool onOrOff)
    {
        activationPanelParent.SetActive(onOrOff);
    }
    #endregion

    // Entity / Activation related
    #region
    public Action ActivateEntity(LivingEntity entity)
    {
        Action action = new Action();
        StartCoroutine(ActivateEntityCoroutine(entity, action));
        return action;
    }
    public IEnumerator ActivateEntityCoroutine(LivingEntity entity, Action action)
    {
        Debug.Log("Activating entity: " + entity.name);
        entityActivated = entity;        
        CameraManager.Instance.SetCameraLookAtTarget(entity.gameObject);

        if (entity.defender)
        {
            UIManager.Instance.SetEndTurnButtonText("End Activation");
            UIManager.Instance.EnableEndTurnButtonView();
            UIManager.Instance.EnableEndTurnButtonInteractions();
            entity.defender.SelectDefender();
        }
        else if (entity.enemy)
        {
            UIManager.Instance.EnableEndTurnButtonView();
            UIManager.Instance.SetEndTurnButtonText("Enemy Activation...");
            UIManager.Instance.DisableEndTurnButtonInteractions();
        }

        entity.myOnActivationEndEffectsFinished = false;
        Action activationStartAction = entity.OnActivationStart();
        yield return new WaitUntil(() => activationStartAction.ActionResolved() == true);

        entity.hasActivatedThisTurn = true;

        if (entity.enemy)
        {
            yield return new WaitForSeconds(1f);
            entity.enemy.StartMyActivation();
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => entity.enemy.ActivationFinished() == true);
        }

        action.actionResolved = true;        
    }
    public Action EndEntityActivation(LivingEntity entity)
    {
        Action action = new Action();
        StartCoroutine(EndEntityActivationCoroutine(entity, action));
        return action;
    }
    public IEnumerator EndEntityActivationCoroutine(LivingEntity entity, Action action)
    {
        Action endActivationAction = entity.OnActivationEnd();
        yield return new WaitUntil(() => endActivationAction.ActionResolved() == true);
        action.actionResolved = true;
    }
    public void ActivateNextEntity()
    {
        LivingEntity nextEntityToActivate = null;
        if (AllEntitiesHaveActivatedThisTurn())
        {
            StartNewTurnSequence();
        }
        else
        {
            for (int index = 0; index < activationOrder.Count; index++)
            {
                if (activationOrder[index].inDeathProcess == false &&
                   (activationOrder[index].hasActivatedThisTurn == false ||
                    (activationOrder[index].hasActivatedThisTurn == true && activationOrder[index].myPassiveManager.timeWarp)))
                {
                    nextEntityToActivate = activationOrder[index];
                    break;
                }
            }


            if (nextEntityToActivate == null)
            {
                StartNewTurnSequence();
            }
            else
            {
                ActivateEntity(nextEntityToActivate);
            }
        }
        
    }
    public bool IsEntityActivated(LivingEntity entity)
    {
        if(entityActivated == entity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool AllEntitiesHaveActivatedThisTurn()
    {
        bool boolReturned = true;
        foreach (LivingEntity entity in activationOrder)
        {
            if (entity.hasActivatedThisTurn == false ||
                (entity.myPassiveManager.timeWarp == true && entity.myPassiveManager.timeWarp))
            {
                boolReturned = false;
            }
        }
        return boolReturned;
    }
    #endregion







}
