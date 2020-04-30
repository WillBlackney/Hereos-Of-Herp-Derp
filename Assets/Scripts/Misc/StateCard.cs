using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StateCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image stateImage;

    [Header("Rarity Parent References")]
    public GameObject commonParent;
    public GameObject rareParent;
    public GameObject bossParent;
    public GameObject afflictionParent;
    public GameObject eventParent;

    [Header("Properties")]
    public StateDataSO myStateData;
    public float originalScale;
    public bool shrinking;
    public bool expanding;

    public void BuildFromStateData(StateDataSO data)
    {
        // Get data reference
        myStateData = data;

        // Set scale
        originalScale = GetComponent<RectTransform>().localScale.x;

        // Build views
        nameText.text = data.stateName;
        descriptionText.text = data.stateDescription;
        stateImage.sprite = data.stateSprite;

        // set rarity gem / afflication gem
        if (data.affliction)
        {
            afflictionParent.SetActive(true);
        }
        else if(data.rarity == StateDataSO.Rarity.Common)
        {
            commonParent.SetActive(true);
        }
        else if (data.rarity == StateDataSO.Rarity.Rare)
        {
            rareParent.SetActive(true);
        }
        else if (data.rarity == StateDataSO.Rarity.Boss)
        {
            bossParent.SetActive(true);
        }
        else if (data.eventReward)
        {
            eventParent.SetActive(true);
        }
    }

    // Mouse + Click Events
    #region
    public void OnStateCardClicked()
    {
        // Gain new state
        StateManager.Instance.GainState(myStateData, true);

        // Clear relevant reward screen elements
        RewardScreen.Instance.DestroyAllStateCards();
        Destroy(RewardScreen.Instance.currentStateRewardButton);
        RewardScreen.Instance.currentStateRewardButton = null;
        RewardScreen.Instance.DisableStateRewardScreen();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Enter detected...");
        StartCoroutine(Expand(1));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit detected...");
        StartCoroutine(Shrink(1));
    }
    #endregion

    // View Logic
    #region
    public IEnumerator Expand(int speed)
    {
        shrinking = false;
        expanding = true;

        float finalScale = originalScale * 1.2f;
        RectTransform transform = GetComponent<RectTransform>();

        while (transform.localScale.x < finalScale && expanding == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x + (0.1f * speed), transform.localScale.y + (0.1f * speed));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator Shrink(int speed)
    {
        expanding = false;
        shrinking = true;

        RectTransform transform = GetComponent<RectTransform>();

        while (transform.localScale.x != originalScale && shrinking == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x - (0.1f * speed), transform.localScale.y - (0.1f * speed));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
