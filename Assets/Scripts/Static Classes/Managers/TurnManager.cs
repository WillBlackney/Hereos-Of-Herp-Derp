using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    [Header("Turn Notifier Properties + Components")]
    public TextMeshProUGUI whoseTurnText;
    public CanvasGroup visualParentCG;
    public RectTransform startPos;
    public RectTransform endPos;
    public RectTransform middlePos;

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

        RectTransform parent = visualParentCG.gameObject.GetComponent<RectTransform>();

        visualParentCG.gameObject.SetActive(true);
        parent.anchoredPosition = startPos.anchoredPosition;
        visualParentCG.alpha = 0;

        whoseTurnText.text = "Turn " + currentTurnCount;

        Vector3 middlePos1 = new Vector2(middlePos.anchoredPosition.x, middlePos.anchoredPosition.y);
        Vector3 endPos1 = new Vector2(endPos.anchoredPosition.x, endPos.anchoredPosition.y);


        while(reachedMiddlePos == false)
        {
            visualParentCG.alpha += 0.05f;
            parent.anchoredPosition = Vector2.MoveTowards(parent.anchoredPosition, middlePos1, 100 * Time.deltaTime);
            if(parent.anchoredPosition.x == middlePos1.x && parent.anchoredPosition.y == middlePos1.y)
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
            visualParentCG.alpha -= 0.05f;
            parent.anchoredPosition = Vector2.MoveTowards(parent.anchoredPosition, endPos1, 100 * Time.deltaTime);
            if (parent.anchoredPosition.x == endPos1.x && parent.anchoredPosition.y == endPos1.y)
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
