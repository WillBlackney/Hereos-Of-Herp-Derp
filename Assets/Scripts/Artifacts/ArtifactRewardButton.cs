using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArtifactRewardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel;
    public TextMeshProUGUI artifactNameText;
    public Image artifactImage;
    public ArtifactDataSO myArtifactData;
    
    public TextMeshProUGUI artifactDescriptionPanelText;

    public void InitializeSetup()
    {
        myArtifactData = ArtifactLibrary.Instance.GetRandomViableArtifact();
        artifactImage.sprite = myArtifactData.artifactSprite;
        artifactNameText.text = myArtifactData.artifactName;

        // info panel        
        artifactDescriptionPanelText.text = myArtifactData.artifactDescription;


    }
    public void OnArtifactRewardButtonClicked()
    {
        ArtifactManager.Instance.ObtainArtifact(myArtifactData);
        Destroy(gameObject);
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
