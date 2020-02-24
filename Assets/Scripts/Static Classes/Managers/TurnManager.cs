using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
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

    public static TurnManager Instance;
    private void Awake()
    {
        Instance = this;
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
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, middlePos1, 10 * Time.deltaTime);
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
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, endPos1, 10 * Time.deltaTime);
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
