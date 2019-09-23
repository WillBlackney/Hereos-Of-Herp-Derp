using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager Instance;

    [Header("Gameobject + Component References")]
    public GameObject artifactPanelView;

    [Header("Properties")]
    public List<ArtifactDataSO> artifactsObtained;

    private void Awake()
    {
        Instance = this;
        artifactsObtained = new List<ArtifactDataSO>();
    }

    public void ObtainArtifact(ArtifactDataSO artifactObtained)
    {
        artifactsObtained.Add(artifactObtained);
        GameObject artifactGO = Instantiate(PrefabHolder.Instance.ArtifactGO, artifactPanelView.transform);
        artifactGO.GetComponent<ArtifactGO>().InitializeSetup(artifactObtained);

        // individual artifact 'On pickup' logic
        if(artifactObtained.artifactName == "Comfy Pillow")
        {
            CampSiteManager.Instance.restButtonDescriptionText.text = "All characters heal 50% HP";
        }
        else if(artifactObtained.artifactName == "Kettle Bell")
        {
            CampSiteManager.Instance.trainButtonDescriptionText.text = "Choose a character to gain 2 levels";
        }
        else if (artifactObtained.artifactName == "Ferryman Token")
        {
            PlayerDataManager.Instance.ModifyGold(300);
        }
        else if (artifactObtained.artifactName == "Ice Cream")
        {
            foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyCurrentLevel(1);
                character.ModifyTalentPoints(1);
            }
        }
        else if (artifactObtained.artifactName == "Yummy Mushroom")
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyMaxHealth(15);
                
            }
        }
    }

    public bool HasArtifact(string artifactName)
    {      
        foreach(ArtifactDataSO artData in artifactsObtained)
        {
            if(artData.artifactName == artifactName)
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

    

}
