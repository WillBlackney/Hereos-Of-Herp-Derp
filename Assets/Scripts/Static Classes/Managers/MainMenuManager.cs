using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("Component References")]
    public GameObject HeroSelectionScreen;

    [Header("Properties")]
    public List<string> characterClassNames;
    public bool startGameEventTriggered;

    // Mouse + Button + Click Events
    #region
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
        if(startGameEventTriggered == false)
        {
            startGameEventTriggered = true;
            StartCoroutine(OnStartGameButtonClickedCoroutine());
        }
        
    }
    public IEnumerator OnStartGameButtonClickedCoroutine()
    {
        Debug.Log("Start Game Button Clicked...");

        // Start screen fade transistion
        Action fadeAction = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything,2, 1, true);
        yield return new WaitUntil(() => fadeAction.ActionResolved() == true);        

        CharacterBox[] chosenCharacters = FindObjectsOfType<CharacterBox>();

        List<string> chosenClasses = new List<string>();

        foreach (CharacterBox character in chosenCharacters)
        {
            chosenClasses.Add(character.currentSelectedCharacter);
        }

        // choose character classes for characters that were set as random

        foreach (CharacterBox character in chosenCharacters)
        {
            if (character.currentSelectedCharacter == "Random")
            {
                while (chosenClasses.Contains(character.currentSelectedCharacter))
                {
                    character.currentSelectedCharacter = GetRandomClassString();
                }
            }
        }

        foreach (CharacterBox character in chosenCharacters)
        {
            SceneChangeDataStorage.Instance.chosenCharacters.Add(character.currentSelectedCharacter);
        }

        SceneManager.LoadScene("Game Scene");
    }
    #endregion

    // Misc Logic
    #region
    public string GetRandomClassString()
    {
        int randomIndex = Random.Range(0, characterClassNames.Count);
        return characterClassNames[randomIndex];
    }
    #endregion
}
