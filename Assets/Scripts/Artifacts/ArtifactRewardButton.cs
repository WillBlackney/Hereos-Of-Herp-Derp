using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArtifactRewardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Properties")]
    public ArtifactDataSO myArtifactData;

    [Header("Component References")]
    public GameObject infoPanel;
    public TextMeshProUGUI artifactNameText;
    public Image artifactImage;       
    public TextMeshProUGUI artifactDescriptionPanelText;

    // Initialization + Setup
    #region
    public void InitializeSetup()
    {
        myArtifactData = ArtifactLibrary.Instance.GetRandomViableArtifact();
        artifactImage.sprite = myArtifactData.artifactSprite;
        artifactNameText.text = myArtifactData.artifactName;

        // info panel        
        artifactDescriptionPanelText.text = myArtifactData.artifactDescription;


    }
    #endregion

    // Mouse / Click Events
    #region
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
    #endregion
}
