using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class ItemCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [Header("Component References")]    
    public Image itemImage;
    public Image itemImageFrame;
    public Image itemNameRibbonImage;
    public TextMeshProUGUI myNameText;
    public TextMeshProUGUI myDescriptionText;
    public Sprite commonRibbonSprite;
    public Sprite rareRibbonSprite;
    public Sprite epicRibbonSprite;
    public Sprite commonFrameSprite;
    public Sprite rareFrameSprite;
    public Sprite epicFrameSprite;

    [Header("Properties")]
    public bool inInventory;
    public bool inShop;
    public ItemSlot myItemSlot;
    public string myName;
    public ItemDataSO myItemDataSO;
    public ItemDataSO.ItemRarity myItemRarity;
    public float originalScale;
    public bool expanding;
    public bool shrinking;


    // Setup + Initialization
    #region
    public void RunSetupFromItemData(ItemDataSO data)
    {
        originalScale = GetComponent<RectTransform>().localScale.x;
        myItemDataSO = data;
        Debug.Log("RunSetupFromItemData() called...");
        myName = data.itemName;
        myNameText.text = myName;
        myDescriptionText.text = data.itemDescription;
        itemImage.sprite = data.itemImage;
        myItemRarity = data.itemRarity;

        if (myItemRarity == ItemDataSO.ItemRarity.Common)
        {
            itemImageFrame.sprite = commonFrameSprite;
            itemNameRibbonImage.sprite = commonRibbonSprite;
        }
        else if (myItemRarity == ItemDataSO.ItemRarity.Rare)
        {
            itemImageFrame.sprite = rareFrameSprite;
            itemNameRibbonImage.sprite = rareRibbonSprite;
        }
        else if (myItemRarity == ItemDataSO.ItemRarity.Epic)
        {
            itemImageFrame.sprite = epicFrameSprite;
            itemNameRibbonImage.sprite = epicRibbonSprite;
        }

    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnItemCardClicked()
    {
        if (inInventory)
        {
            if(InventoryManager.Instance.readyToAcceptNewItem == true)
            {
                CharacterRoster.Instance.selectedCharacterData.AddItemToEquiptment(this);
                InventoryManager.Instance.RemoveItemFromInventory(this);
                InventoryManager.Instance.readyToAcceptNewItem = false;
            }
            return;
        }

        else if (inShop)
        {
            myItemSlot.BuyItem();
            return;
        }

        Debug.Log("Adding Item to inventory: " + myName);
        // add item to inventory
        InventoryManager.Instance.AddItemToInventory(this);
        RewardScreen.Instance.DestroyAllItemCards();
        Destroy(RewardScreen.Instance.currentItemRewardButton);
        RewardScreen.Instance.currentItemRewardButton = null;        
        RewardScreen.Instance.DisableItemLootScreen();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!inInventory)
        {
            Debug.Log("Mouse Enter detected...");
            StartCoroutine(Expand(1));
        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!inInventory)
        {
            Debug.Log("Mouse Exit detected...");
            StartCoroutine(Shrink(1));
        }
            

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
