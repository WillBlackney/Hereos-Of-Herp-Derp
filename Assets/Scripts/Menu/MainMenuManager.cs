using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager>
{

    public GameObject HeroSelectionScreen;
    public List<string> characterClassNames;    

    public void OnNewGameButtonClicked()
    {
        HeroSelectionScreen.SetActive(true);
    }

    public void OnBackToMainMenuButtonCicked()
    {
        HeroSelectionScreen.SetActive(false);
    }

    public void OnStartGameButtonClicked()
    {
        Debug.Log("Start Game Button Clicked...");

        CharacterBox[] chosenCharacters = FindObjectsOfType<CharacterBox>();

        List<string> chosenClasses = new List<string>();

        foreach (CharacterBox character in chosenCharacters)
        {
            chosenClasses.Add(character.currentSelectedCharacter);
        }

        // choose character classes for characters that were set as random

        foreach (CharacterBox character in chosenCharacters)
        {
            if(character.currentSelectedCharacter == "Random")
            {
                while (chosenClasses.Contains(character.currentSelectedCharacter))
                {
                    character.currentSelectedCharacter = GetRandomClassString();
                }
            }
        }

        foreach(CharacterBox character in chosenCharacters)
        {
            SceneChangeDataStorage.Instance.chosenCharacters.Add(character.currentSelectedCharacter);
        }

        SceneManager.LoadScene("Game Scene");
    }

    public string GetRandomClassString()
    {
        int randomIndex = Random.Range(0, characterClassNames.Count);
        return characterClassNames[randomIndex];
    }
}
