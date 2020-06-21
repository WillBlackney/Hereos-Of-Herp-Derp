using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class ItemCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Properties + Component References
    #region

    public enum Location { None, Shop, Inventory, LootScreen };
    [Header("Image References")]
    public Image itemImage;
    public Image itemDamageTypeImage;

    [Header("Canvas References")]
    public Canvas mainCanvas;
    public CanvasGroup cg;

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
    public Location location;
    public ItemSlot myItemSlot;
    public ItemDataSO myData;
    public string myName;
    public float originalScale;
    public bool expanding;
    public bool shrinking;

    [Header("Info Panels + References")]
    public GameObject panelOneParent;
    public bool panelOneActive;
    #endregion

    // Setup + Initialization
    #region
    public void RunSetupFromItemData(ItemDataSO data, int sortingOrder)
    {
        ItemManager.Instance.SetUpItemCardFromData(this, data, sortingOrder);
        myData = data;
    }

    #endregion

    // Mouse + Click Events
    #region
    public void OnItemCardClicked()
    {
        Debug.Log("ItemCard.OnItemCardClicked() called...");

        if (location == Location.Inventory)
        {
            return;
        }

        else if (location == Location.Shop)
        {
            myItemSlot.BuyItem();
            return;
        }

        else if (location == Location.LootScreen)
        {
            if (myData.itemRarity == ItemDataSO.ItemRarity.Common)
            {
                RewardScreen.Instance.DestroyAllCommonItemCards();
                Destroy(RewardScreen.Instance.currentCommonItemRewardButton);
                RewardScreen.Instance.currentCommonItemRewardButton = null;
                RewardScreen.Instance.DisableCommonItemLootScreen();
            }
            else if (myData.itemRarity == ItemDataSO.ItemRarity.Rare)
            {
                RewardScreen.Instance.DestroyAllRareItemCards();
                Destroy(RewardScreen.Instance.currentRareItemRewardButton);
                RewardScreen.Instance.currentRareItemRewardButton = null;
                RewardScreen.Instance.DisableRareItemLootScreen();
            }
            else if (myData.itemRarity == ItemDataSO.ItemRarity.Epic)
            {
                RewardScreen.Instance.DestroyAllEpicItemCards();
                Destroy(RewardScreen.Instance.currentEpicItemRewardButton);
                RewardScreen.Instance.currentEpicItemRewardButton = null;
                RewardScreen.Instance.DisableEpicItemLootScreen();
            }

            Debug.Log("Adding Item to inventory: " + myName);
            InventoryController.Instance.AddItemToInventory(myItemDataSO, true);
        }

        ItemCardPanelHover.Instance.OnItemCardMouseExit(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ItemCard.OnPointerEnter() called...");

        if (location == Location.Shop ||
            location == Location.LootScreen)
        {
            Debug.Log("Mouse Enter detected...");
            StartCoroutine(Expand(1));            
        }

        else
        {
            if(location != Location.Inventory)
            {
                ItemCardPanelHover.Instance.OnItemCardMousedOver(this);
            }            
        }       

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ItemCard.OnPointerExit() called...");

        if (location == Location.Shop ||
            location == Location.LootScreen)
        {
            Debug.Log("Mouse Exit detected...");
            StartCoroutine(Shrink(1));
        }

        ItemCardPanelHover.Instance.OnItemCardMouseExit(this);
    }
    public void OnDisable()
    {
        ItemCardPanelHover.Instance.OnItemCardMouseExit(this);
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
