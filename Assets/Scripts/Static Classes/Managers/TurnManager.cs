using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : Singleton<TurnManager>
{
    [Header("Turn Notifier Properties + Components")]
    public TextMeshProUGUI whoseTurnText;
    public CanvasGroup visualParentCG;
    public GameObject startPos;
    public GameObject endPos;
    public GameObject middlePos;

    public bool currentlyPlayersTurn = false;

    public int currentTurnCount = 0;
    public int enemyTurnCount = 0;

    public bool playerOnTurnEndEventsResolved;
    public bool PlayerOnTurnEndEventsResolved()
    {
        if(playerOnTurnEndEventsResolved == true)
        {
            Debug.Log("Player 'OnTurnEnd' events resolved and finished...");
            return true;
        }
        else
        {
            return false;
        }
    }


    public void OnEndTurnButtonClicked()
    {
        StartCoroutine(OnEndTurnButtonClickedCoroutine());
    }
    public IEnumerator OnEndTurnButtonClickedCoroutine()
    {
        Debug.Log("OnEndTurnButtonClickedCoroutine() started...");
        // TO DO: endplayerturn will need to be a coroutine most likely
        // endplayer turn will trigger all player end turn effects, BEFORE switching to enemy turn
        EndPlayerTurn();
        yield return new WaitUntil(() => PlayerOnTurnEndEventsResolved() == true);
        // reset boolean for future use
        playerOnTurnEndEventsResolved = false;

        StartEnemyTurn();
    }

    public void StartEnemyTurn()
    {
        foreach (Defender defender in DefenderManager.Instance.allDefenders)
        {
            defender.myOnActivationEndEffectsFinished = false;
        }
        Debug.Log("StartEnemyTurn() called...");
        //ResetEnemyTurnProperties();
        StartCoroutine(StartEnemyTurnCoroutine());
    }

    public IEnumerator StartEnemyTurnCoroutine()
    {
        /*
        enemyTurnCount++;
        Action action = DisplayTurnChangeNotification();
        // notification yield not currently working
        yield return new WaitUntil(() => NotificationComplete() == true);
        //yield return new WaitForSeconds(1.5f);

        EnemyManager.Instance.StartEnemyTurnSequence();
        yield return new WaitUntil(() => EnemyManager.Instance.AllEnemiesHaveActivated() == true);
        StartCoroutine(EndEnemyTurnCoroutine());
        */
        yield return null;
    }

    
    public IEnumerator StartPlayerTurn()
    {
        /*
        currentTurnCount++;

        StartCoroutine( DisplayTurnChangeNotificationCoroutine(true));
        yield return new WaitUntil(() => NotificationComplete() == true);
        //yield return new WaitForSeconds(1.5f);

        
        currentlyPlayersTurn = true;
        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            StartCoroutine(defender.OnActivationStart());
            defender.myOnActivationEndEffectsFinished = false;
        }

        UIManager.Instance.EnableEndTurnButton();
        // turn on controls
        // re-enable end turn button
        */
        yield return null;
    }

    public void EndPlayerTurn()
    {
        // disable defender spell bars to stop player doing stuff during enemy turn
        DefenderManager.Instance.ClearSelectedDefender();
        UIManager.Instance.DisableEndTurnButtonInteractions();
        currentlyPlayersTurn = false;
        StartCoroutine(EndPlayerTurnCoroutine());
        // disable en turn button
        // turn off controls
        // trigger on turn effects(bleed, poison etc)
    }

    public IEnumerator EndPlayerTurnCoroutine()
    {
        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            defender.OnActivationEnd();
            yield return new WaitUntil(() => defender.MyOnTurnEndEffectsFinished() == true);
        }

        playerOnTurnEndEventsResolved = true;
    }

    public void ResetTurnManagerPropertiesOnCombatStart()
    {
        currentTurnCount = 0;
        enemyTurnCount = 0;
    }


    public Action DisplayTurnChangeNotification()
    {
        Action action = new Action();
        StartCoroutine(DisplayTurnChangeNotificationCoroutine(action));
        return action;
    }
    public IEnumerator DisplayTurnChangeNotificationCoroutine(Action action)
    {
        bool reachedMiddlePos = false;
        bool reachedEndPos = false;

        GameObject parent = visualParentCG.gameObject;

        visualParentCG.gameObject.SetActive(true);
        parent.transform.position = startPos.transform.position;
        visualParentCG.alpha = 0;

        whoseTurnText.text = "Turn " + currentTurnCount;

        Vector3 middlePos1 = new Vector2(middlePos.transform.position.x, middlePos.transform.position.y);
        Vector3 endPos1 = new Vector2(endPos.transform.position.x, endPos.transform.position.y);


        while(reachedMiddlePos == false)
        {
            visualParentCG.alpha += 0.08f;
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, middlePos1, 15 * Time.deltaTime);
            if(parent.transform.position.x == middlePos1.x && parent.transform.position.y == middlePos1.y)
            {
                Debug.Log("reached Middle pos");
                reachedMiddlePos = true;
            }
            yield return new WaitForEndOfFrame();
        }

        visualParentCG.alpha = 1;

        // brief pause while text in centred on screen
        yield return new WaitForSeconds(0.5f);

        while (reachedEndPos == false)
        {
            visualParentCG.alpha -= 0.08f;
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, endPos1, 15 * Time.deltaTime);
            if (parent.transform.position.x == endPos1.x && parent.transform.position.y == endPos1.y)
            {
                Debug.Log("reached end pos");
                reachedEndPos = true;
            }
            yield return new WaitForEndOfFrame();
        }        

        visualParentCG.alpha = 0;
        visualParentCG.gameObject.SetActive(false);
        action.actionResolved = true;
    }

}
