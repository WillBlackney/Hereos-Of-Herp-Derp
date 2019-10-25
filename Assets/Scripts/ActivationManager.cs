using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActivationManager : Singleton<ActivationManager>
{
    [Header("Component References")]
    public GameObject activationWindowParent;
    public GameObject panelArrow;

    [Header("Properties")]
    public List<LivingEntity> activationOrder;
    public LivingEntity entityActivated;
    

    // Setup + Initializaton
    #region   
    public void CreateActivationWindow(LivingEntity entity)
    {
        GameObject newWindow = Instantiate(PrefabHolder.Instance.activationWindowPrefab, activationWindowParent.transform);
        ActivationWindow newWindowScript = newWindow.GetComponent<ActivationWindow>();
        newWindowScript.InitializeSetup(entity);
        activationOrder.Add(entity);
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
        StartNewTurnSequence();
        action.actionResolved = true;
        yield return null;
    }
    public Action StartNewTurnSequence()
    {
        Action action = new Action();
        StartCoroutine(StartNewTurnSequenceCoroutine(action));
        return action;
    }
    public IEnumerator StartNewTurnSequenceCoroutine(Action action)
    {
        TurnManager.Instance.currentTurnCount++;        

        Action rolls = CalculateActivationOrder();
        yield return new WaitUntil(() => rolls.ActionResolved() == true);

        Action turnNotification = TurnManager.Instance.DisplayTurnChangeNotification();
        yield return new WaitUntil(() => turnNotification.ActionResolved());        

        ActivateEntity(activationOrder[0]);
        action.actionResolved = true;
    }
    public void ClearAllWindowsFromActivationPanel()
    {
        activationOrder.Clear();
        foreach (Transform child in activationWindowParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion

    // Logic + Calculations
    #region
    public int CalculateInitiativeRoll(LivingEntity entity)
    {
        return entity.currentInitiative + Random.Range(1, 4);
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
        ArrangeActivationWindowPositions();
        yield return new WaitForSeconds(1f);

        // Disable roll number text components
        foreach(LivingEntity entity in activationOrder)
        {
            entity.myActivationWindow.rollText.enabled = false;
        }

        panelArrow.SetActive(true);
        action.actionResolved = true;        
    }
    public void ArrangeActivationWindowPositions()
    {
        foreach(LivingEntity entity in activationOrder)
        {
            entity.myActivationWindow.gameObject.transform.SetSiblingIndex(activationOrder.IndexOf(entity));            
        }
    }
   
    #endregion 

    // Player Input + UI interactions + Visual
    #region
    public void OnEndTurnButtonClicked()
    {
        //Action action = new Action();
        StartCoroutine(OnEndTurnButtonClickedCoroutine());
    }
    public IEnumerator OnEndTurnButtonClickedCoroutine()
    {
        Debug.Log("OnEndTurnButtonClickedCoroutine() started...");
        // TO DO: endplayerturn will need to be a coroutine most likely
        // endplayer turn will trigger all player end turn effects, BEFORE switching to enemy turn
        Action endTurnEvent = EndEntityActivation(entityActivated);
        yield return new WaitUntil(() => endTurnEvent.ActionResolved() == true);
        //action.actionResolved = true;
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
            Debug.Log("Animating roll number text....");
            numberDisplayed++;
            if (numberDisplayed > 9)
            {
                numberDisplayed = 0;
            }
            window.rollText.text = numberDisplayed.ToString();
            
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator MoveArrowTowardsTargetPanelPos(ActivationWindow window)
    {
        Vector3 destination =  new Vector2(window.transform.position.x, panelArrow.transform.position.y);
        
        while (panelArrow.transform.position != destination)
        {
            panelArrow.transform.position = Vector2.MoveTowards(panelArrow.transform.position, destination, 200 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
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
        StartCoroutine(MoveArrowTowardsTargetPanelPos(entity.myActivationWindow));

        if (entity.defender)
        {
            UIManager.Instance.EnableEndTurnButton();
            entity.defender.SelectDefender();
        }
        else if (entity.enemy)
        {
            UIManager.Instance.DisableEndTurnButton();
        }

        StartCoroutine(entity.OnActivationStart());
        entity.myOnActivationEndEffectsFinished = false;

        if (entity.enemy)
        {
            yield return new WaitForSeconds(1f);
            entity.enemy.StartMyActivation();
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => entity.enemy.ActivationFinished() == true);
        }

        
        yield return null;

    }
    public Action EndEntityActivation(LivingEntity entity)
    {
        Action action = new Action();
        StartCoroutine(EndEntityActivationCoroutine(entity, action));
        return action;
    }
    public IEnumerator EndEntityActivationCoroutine(LivingEntity entity, Action action)
    {
        StartCoroutine(entity.OnActivationEndCoroutine());
        yield return new WaitUntil(() => entity.myOnActivationEndEffectsFinished == true);
        action.actionResolved = true;
    }
    public void ActivateNextEntity()
    {
        int lastIndex = activationOrder.Count() - 1;
        int currentIndex = activationOrder.IndexOf(entityActivated);
        if (currentIndex != lastIndex)
        {
            ActivateEntity(activationOrder[currentIndex + 1]);
        }
        else
        {
            StartNewTurnSequence();
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
    #endregion







}
