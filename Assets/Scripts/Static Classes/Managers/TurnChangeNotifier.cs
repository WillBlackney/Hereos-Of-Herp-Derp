using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnChangeNotifier : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI whoseTurnText;
    public CanvasGroup visualParentCG;
    public RectTransform startPos;
    public RectTransform endPos;
    public RectTransform middlePos;

    [Header("Properties")]
    public float animSpeed;
    public float alphaChangeSpeed;
    public bool currentlyPlayersTurn = false;
    public int currentTurnCount = 0;
    public int enemyTurnCount = 0;

    public static TurnChangeNotifier Instance;
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

       // Vector3 middlePos1 = new Vector2(middlePos.anchoredPosition.x, middlePos.anchoredPosition.y);
        //Vector3 endPos1 = new Vector2(endPos.anchoredPosition.x, endPos.anchoredPosition.y);


        while(reachedMiddlePos == false)
        {
            visualParentCG.alpha += alphaChangeSpeed;
            parent.anchoredPosition = Vector2.MoveTowards(parent.anchoredPosition, middlePos.anchoredPosition, animSpeed  * Time.deltaTime);
            if(parent.anchoredPosition.x == middlePos.anchoredPosition.x)
            {
                Debug.Log("reached Middle pos");
                reachedMiddlePos = true;
            }
            yield return new WaitForEndOfFrame();
        }

        visualParentCG.alpha = 1;

        // brief pause while text in centred on screen
        yield return new WaitForSeconds(0.8f);

        while (reachedEndPos == false)
        {
            visualParentCG.alpha -= alphaChangeSpeed;
            parent.anchoredPosition = Vector2.MoveTowards(parent.anchoredPosition, endPos.anchoredPosition, animSpeed * Time.deltaTime);

            // snap to end position if already alpha'd out
            if(visualParentCG.alpha == 0)
            {
                parent.anchoredPosition = endPos.anchoredPosition;
            }

            if (parent.anchoredPosition.x == endPos.anchoredPosition.x)
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
