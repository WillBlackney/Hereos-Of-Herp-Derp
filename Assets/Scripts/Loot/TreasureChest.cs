using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureChest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SpriteRenderer mySpriteRenderer;
    public Color normalColor;
    public Color highLightColor;

    public void InitializeSetup()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.Instance.activeTreasureChest = this;
        transform.position = LevelManager.Instance.GetWorldCentreTile().WorldPosition;
    }

    public void DestroyChest()
    {
        EventManager.Instance.activeTreasureChest = null;
        Destroy(gameObject);
        Destroy(this);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick() detected on treasure chest");
        mySpriteRenderer.color = normalColor;
        UIManager.Instance.EnableRewardScreenView();
    }

    public void OnMouseDown()
    {
        mySpriteRenderer.color = normalColor;
        UIManager.Instance.EnableRewardScreenView();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter() detected on treasure chest");
        mySpriteRenderer.color = highLightColor;
    }
    public void OnMouseEnter()
    {
        mySpriteRenderer.color = highLightColor;
    }

    public void OnMouseExit()
    {
        mySpriteRenderer.color = normalColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit() detected on treasure chest");
        mySpriteRenderer.color = normalColor;
    }
}
