using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityTomeInShop : MonoBehaviour
{
    [Header("Component References")]
    public AbilityInfoSheet abilityInfoSheet;
    public GameObject visualParent;
    public TextMeshProUGUI goldCostText;
    public Image bgColorImage;
    public Image bookImage;
    public TextMeshProUGUI ribbonNameText;

    [Header("Properties")]
    public int goldCost;
    public AbilityDataSO myData;

    [Header("Sizing Properties + Components")]
    public GameObject bookParent;
    private float buttonScale;
    private bool shrinking;
    private bool expanding;
    public int expandSpeed;


    // Initialization + Setup
    #region
    public void BuildFromData(AbilityDataSO data)
    {
        myData = data;

        // reset button scale
        buttonScale = 1;
        ResetScale();

        ribbonNameText.text = data.abilityName;

        AbilityInfoSheetController.Instance.BuildSheetFromData(abilityInfoSheet, data, AbilityInfoSheet.PivotDirection.Downwards);

        // Randomize and set gold cost
        if (data.tier == 1)
        {
            goldCost = Random.Range(4, 7);
        }
        else if (data.tier == 2)
        {
            goldCost = Random.Range(7,10);
        }
        else if (data.tier == 3)
        {
            goldCost = Random.Range(10, 13);
        }
        goldCostText.text = goldCost.ToString();
        

        // Enable Views
        EnableSlotView();        
        SetBookImage();

    }
    public void SetBackgroundColor()
    {
        /*
        if (myData.abilitySchool == AbilityDataSO.AbilitySchool.None)
        {
            bgColorImage.color = InventoryController.Instance.neutralColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Brawler)
        {
            bgColorImage.color = InventoryController.Instance.brawlerColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Duelist)
        {
            bgColorImage.color = InventoryController.Instance.duelistColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Assassination)
        {
            bgColorImage.color = InventoryController.Instance.assassinationColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Guardian)
        {
            bgColorImage.color = InventoryController.Instance.guardianColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Pyromania)
        {
            bgColorImage.color = InventoryController.Instance.pyromaniaColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Cyromancy)
        {
            bgColorImage.color = InventoryController.Instance.cyromancyColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Ranger)
        {
            bgColorImage.color = InventoryController.Instance.rangerColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Manipulation)
        {
            bgColorImage.color = InventoryController.Instance.manipulationColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Divinity)
        {
            bgColorImage.color = InventoryController.Instance.divinityColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Shadowcraft)
        {
            bgColorImage.color = InventoryController.Instance.shadowcraftColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Corruption)
        {
            bgColorImage.color = InventoryController.Instance.corruptionColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Naturalism)
        {
            bgColorImage.color = InventoryController.Instance.naturalismColor;
        }

        // set transperancy
        bgColorImage.color = new Color(bgColorImage.color.r / 255, bgColorImage.color.g / 255, bgColorImage.color.g / 255, 0.30f);
        */
    }
    public void SetBookImage()
    {

        if (myData.abilitySchool == AbilityDataSO.AbilitySchool.None)
        {
            bookImage.sprite = InventoryController.Instance.neutralBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Brawler)
        {
            bookImage.sprite = InventoryController.Instance.brawlerBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Duelist)
        {
            bookImage.sprite = InventoryController.Instance.duelistBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Assassination)
        {
            bookImage.sprite = InventoryController.Instance.assassinationBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Guardian)
        {
            bookImage.sprite = InventoryController.Instance.guardianBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Pyromania)
        {
            bookImage.sprite = InventoryController.Instance.pyromaniaBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Cyromancy)
        {
            bookImage.sprite = InventoryController.Instance.cyromancyBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Ranger)
        {
            bookImage.sprite = InventoryController.Instance.rangerBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Manipulation)
        {
            bookImage.sprite = InventoryController.Instance.manipulationBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Divinity)
        {
            bookImage.sprite = InventoryController.Instance.divinityBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Shadowcraft)
        {
            bookImage.sprite = InventoryController.Instance.shadowcraftBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Corruption)
        {
            bookImage.sprite = InventoryController.Instance.corruptionBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Naturalism)
        {
            bookImage.sprite = InventoryController.Instance.naturalismBookImage;
        }
    }
    #endregion

    // View Logic
    #region
    public void EnableInfoPanelView()
    {
        AbilityInfoSheetController.Instance.EnableSheetView(abilityInfoSheet, true, true);
    }
    public void DisableInfoPanelView()
    {
        AbilityInfoSheetController.Instance.DisableSheetView(abilityInfoSheet);
    }
    public void EnableSlotView()
    {
        visualParent.SetActive(true);
    }
    public void DisableSlotView()
    {
        visualParent.SetActive(false);
    }
    public IEnumerator Expand(int speed)
    {
        shrinking = false;
        expanding = true;

        float finalScale = buttonScale * 1.2f;
        RectTransform transform = bookParent.GetComponent<RectTransform>();

        while (transform.localScale.x < finalScale && expanding == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x + (0.1f * speed * Time.deltaTime), transform.localScale.y + (0.1f * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }

        if (transform.localScale.x >= finalScale)
        {
            EnableInfoPanelView();
        }

    }
    public IEnumerator Shrink(int speed)
    {
        expanding = false;
        shrinking = true;
        DisableInfoPanelView();

        RectTransform transform = bookParent.GetComponent<RectTransform>();

        while (transform.localScale.x > buttonScale && shrinking == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x - (0.1f * speed * Time.deltaTime), transform.localScale.y - (0.1f * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    public void ResetScale()
    {
        bookParent.GetComponent<RectTransform>().localScale = new Vector3(buttonScale, buttonScale);
    }

    #endregion

    // Mouse + Click Events
    #region    
    public void OnMouseDown()
    {
        InventoryController.Instance.BuyAbilityTomeFromShop(this);
    }
    public void OnMouseEnter()
    {
        StartCoroutine(Expand(expandSpeed));
    }
    public void OnMouseExit()
    {
        StartCoroutine(Shrink(expandSpeed));
    }
    #endregion
}
