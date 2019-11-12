using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArtifactSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Properties")]
    public int goldCost;
    public ArtifactDataSO myArtifactData;

    [Header("Component References")]
    public TextMeshProUGUI goldCostText;    
    public Image myArtifactImage;
    public GameObject infoPanel;
    public TextMeshProUGUI artifactNameText;
    public TextMeshProUGUI artifactDescriptionText;

    // Initialization + Setup
    #region
    public void InitializeSetup(ArtifactDataSO artifactData)
    {
        myArtifactData = artifactData;
        myArtifactImage.sprite = artifactData.sprite;
        artifactDescriptionText.text = myArtifactData.description;
        artifactNameText.text = myArtifactData.Name;

        if(myArtifactData.rarity == ArtifactDataSO.Rarity.Common)
        {
            SetGoldCost(Random.Range(100, 150));
        }
        else if (myArtifactData.rarity == ArtifactDataSO.Rarity.Rare)
        {
            SetGoldCost(Random.Range(150, 200));
        }
        else if (myArtifactData.rarity == ArtifactDataSO.Rarity.Epic)
        {
            SetGoldCost(Random.Range(200, 250));
        }

        EnableArtifactSlotView();
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
    }
    public void OnArtifactSlotButtonClicked()
    {
        BuyArtifact();
        DisableArtifactSlotView();
    }
    #endregion

    // Visibility + View Logic
    #region
    public void EnableArtifactSlotView()
    {
        gameObject.SetActive(true);
    }
    public void DisableArtifactSlotView()
    {
        gameObject.SetActive(false);
    }
    #endregion

    // Logic
    #region
    public void SetGoldCost(int newGoldCost)
    {
        goldCost = newGoldCost;
        goldCostText.text = goldCost.ToString();
    }
    public void BuyArtifact()
    {
        if (PlayerDataManager.Instance.currentGold >= goldCost)
        {
            Debug.Log("Buying Artifact " + myArtifactData.Name + " for " + goldCost.ToString());
            PlayerDataManager.Instance.ModifyGold(-goldCost);
            ArtifactManager.Instance.ObtainArtifact(myArtifactData);
        }
    }
    #endregion


}
