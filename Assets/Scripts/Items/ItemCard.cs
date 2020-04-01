using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class ItemCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [Header("Image References")]    
    public Image itemImage;
    public Image itemDamageTypeImage;

    [Header("Text References")]
    public TextMeshProUGUI myNameText;
    public TextMeshProUGUI myDescriptionText;
    public TextMeshProUGUI myItemTypeText;
    public TextMeshProUGUI myBaseDamageText;

    [Header("Game Object References")]
    public GameObject weaponDamageTypeParent;
    public GameObject mouseOverInfoPanelPos;

    [Header("Rarity Parent References")]
    public GameObject commonParent;
    public GameObject rareParent;
    public GameObject epicParent;

    [Header("Properties")]
    public ItemDataSO myItemDataSO;
    public bool inInventory;
    public bool inShop;
    public ItemSlot myItemSlot;
    public string myName;    
    public float originalScale;
    public bool expanding;
    public bool shrinking;

    [Header("Info Panels + References")]
    public GameObject panelOneParent;
    public bool panelOneActive;


    // Setup + Initialization
    #region
    public void RunSetupFromItemData(ItemDataSO data)
    {
        ItemManager.Instance.SetUpItemCardFromData(this, data);
    }    
    
    #endregion

    // Mouse + Click Events
    #region
    public void OnItemCardClicked()
    {
        Debug.Log("ItemCard.OnItemCardClicked() called...");

        if (inInventory)
        {
            return;
        }

        else if (inShop)
        {
            myItemSlot.BuyItem();
            return;
        }

        Debug.Log("Adding Item to inventory: " + myName);
        // add item to inventory
        InventoryController.Instance.AddItemToInventory(myItemDataSO);
        RewardScreen.Instance.DestroyAllItemCards();
        Destroy(RewardScreen.Instance.currentItemRewardButton);
        RewardScreen.Instance.currentItemRewardButton = null;        
        RewardScreen.Instance.DisableItemLootScreen();
        ItemCardPanelHover.Instance.OnItemCardMouseExit();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ItemCard.OnPointerEnter() called...");

        if (!inInventory)
        {
            Debug.Log("Mouse Enter detected...");
            StartCoroutine(Expand(1));            
        }
        else
        {
            ItemCardPanelHover.Instance.OnItemCardMousedOver(this);
        }
        

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ItemCard.OnPointerEnter() called...");

        if (!inInventory)
        {
            Debug.Log("Mouse Exit detected...");
            StartCoroutine(Shrink(1));
        }

        ItemCardPanelHover.Instance.OnItemCardMouseExit();
    }
    #endregion

    // Visibility + View Logic
    #region
    public IEnumerator Expand(int speed)
    {
        shrinking = false;
        expanding = true;

        float finalScale = originalScale * 1.2f;
        RectTransform transform = GetComponent<RectTransform>();

        while (transform.localScale.x < finalScale && expanding == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x + (1 * speed * Time.deltaTime), transform.localScale.y + (1 * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }

        ItemCardPanelHover.Instance.OnItemCardMousedOver(this);
    }
    public IEnumerator Shrink(int speed)
    {
        expanding = false;
        shrinking = true;

        RectTransform transform = GetComponent<RectTransform>();

        while (transform.localScale.x > originalScale && shrinking == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x - (1 * speed * Time.deltaTime), transform.localScale.y - (1 * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
