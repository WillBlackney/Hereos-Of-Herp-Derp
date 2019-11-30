using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    [Header("Gameobject + Component References")]
    public GameObject artifactPanelView;

    [Header("Properties")]
    public List<ArtifactDataSO> artifactsObtained;

    // Initialization + Singleton Pattern
    #region
    public static ArtifactManager Instance;
    private void Awake()
    {
        Instance = this;
        artifactsObtained = new List<ArtifactDataSO>();
    }
    #endregion

    // Logic
    #region
    public void ObtainArtifact(ArtifactDataSO artifactObtained)
    {
        artifactsObtained.Add(artifactObtained);
        GameObject artifactGO = Instantiate(PrefabHolder.Instance.ArtifactGO, artifactPanelView.transform);
        artifactGO.GetComponent<ArtifactGO>().InitializeSetup(artifactObtained);

        // individual artifact 'On pickup' logic
        if(artifactObtained.Name == "Comfy Pillow")
        {
            CampSiteManager.Instance.restButtonDescriptionText.text = "Choose a character heal to max HP";
        }
        else if(artifactObtained.Name == "Kettle Bell")
        {
            CampSiteManager.Instance.trainButtonDescriptionText.text = "Choose a character to gain 2 levels";
        }
        else if (artifactObtained.Name == "Ferryman Coin")
        {
            PlayerDataManager.Instance.ModifyGold(200);
        }
        else if (artifactObtained.Name == "Ice Cream")
        {
            foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyCurrentLevel(1);
                character.ModifyTalentPoints(1);
            }
        }
        else if (artifactObtained.Name == "Yummy Mushroom")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyMaxHealth(20);
                
            }
        }
    }
    #endregion

    // Conditional Checks
    #region
    public bool HasArtifact(string artifactName)
    {      
        foreach(ArtifactDataSO artData in artifactsObtained)
        {
            if(artData.Name == artifactName)
            {
                Debug.Log("ArtifactManager.HasArtifact() confirms player has obtained " + artifactName);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        return false;
    }
    #endregion



}
