using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArtifactSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public TextMeshProUGUI goldCostText;    
    public Image myArtifactImage;
    public GameObject infoPanel;
    public TextMeshProUGUI artifactNameText;
    public TextMeshProUGUI artifactDescriptionText;

    [Header("Properties")]
    public int goldCost;
    public ArtifactDataSO myArtifactData;

    public void InitializeSetup(ArtifactDataSO artifactData)
    {
        myArtifactData = artifactData;
        myArtifactImage.sprite = artifactData.artifactSprite;
        artifactDescriptionText.text = myArtifactData.artifactDescription;
        artifactNameText.text = myArtifactData.artifactName;

        if(myArtifactData.artifactRarity == ArtifactDataSO.Rarity.Common)
        {
            SetGoldCost(Random.Range(100, 150));
        }
        else if (myArtifactData.artifactRarity == ArtifactDataSO.Rarity.Rare)
        {
            SetGoldCost(Random.Range(150, 200));
        }
        else if (myArtifactData.artifactRarity == ArtifactDataSO.Rarity.Epic)
        {
            SetGoldCost(Random.Range(200, 250));
        }

        EnableArtifactSlotView();
    }

    public void EnableArtifactSlotView()
    {
        gameObject.SetActive(true);
    }
    public void DisableArtifactSlotView()
    {
        gameObject.SetActive(false);
    }

    public void SetGoldCost(int newGoldCost)
    {
        goldCost = newGoldCost;
        goldCostText.text = goldCost.ToString();
    }
    public void OnArtifactSlotButtonClicked()
    {
        BuyArtifact();
        DisableArtifactSlotView();
    }

    public void BuyArtifact()
    {
        if (PlayerDataManager.Instance.currentGold >= goldCost)
        {
            Debug.Log("Buying Artifact " + myArtifactData.artifactName + " for " + goldCost.ToString());
            PlayerDataManager.Instance.ModifyGold(-goldCost);
            ArtifactManager.Instance.ObtainArtifact(myArtifactData);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
    }
}
