using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureChest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References")]
    public SpriteRenderer mySpriteRenderer;

    [Header("Properties")]
    public Color normalColor;
    public Color highLightColor;

    // Intialization + Setup
    #region
    public void InitializeSetup()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.Instance.activeTreasureChest = this;
        transform.position = LevelManager.Instance.GetWorldCentreTile().WorldPosition;
    }
    #endregion

    // Mouse + Click Events
    #region    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick() detected on treasure chest");
        mySpriteRenderer.color = normalColor;
        UIManager.Instance.EnableRewardScreenView();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit() detected on treasure chest");
        mySpriteRenderer.color = normalColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter() detected on treasure chest");
        mySpriteRenderer.color = highLightColor;
    }
    public void OnMouseDown()
    {
        mySpriteRenderer.color = normalColor;
        UIManager.Instance.EnableRewardScreenView();
    }
    public void OnMouseEnter()
    {
        mySpriteRenderer.color = highLightColor;
    }
    public void OnMouseExit()
    {
        mySpriteRenderer.color = normalColor;
    }
    #endregion

    // Logic
    #region
    public void DestroyChest()
    {
        EventManager.Instance.activeTreasureChest = null;
        Destroy(gameObject);
        Destroy(this);
    }
    #endregion
}
