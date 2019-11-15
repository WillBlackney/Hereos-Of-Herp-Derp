using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterBox : MonoBehaviour
{
    [Header("Component References")]
    public Image myImageComponent;
    public TextMeshProUGUI myClassNameText;

    [Header("Properties")]
    public string currentSelectedCharacter;
    public Sprite warriorSprite;
    public Sprite mageSprite;
    public Sprite rangerSprite;
    public Sprite priestSprite;
    public Sprite rogueSprite;
    public Sprite shamanSprite;
    public Sprite randomSprite;

    // Initialization + Setup
    #region
    private void Awake()
    {
        SetCharacterAsRandom();
    }
    #endregion

    // Set Character
    #region
    public void SetCharacterAsWarrior()
    {
        currentSelectedCharacter = "Warrior";
        myImageComponent.sprite = warriorSprite;
        myClassNameText.text = "Warrior";
    }
    public void SetCharacterAsPriest()
    {
        currentSelectedCharacter = "Priest";
        myImageComponent.sprite = priestSprite;
        myClassNameText.text = "Priest";
    }
    public void SetCharacterAsMage()
    {
        currentSelectedCharacter = "Mage";
        myImageComponent.sprite = mageSprite;
        myClassNameText.text = "Mage";
    }
    public void SetCharacterAsRanger()
    {
        currentSelectedCharacter = "Ranger";
        myImageComponent.sprite = rangerSprite;
        myClassNameText.text = "Ranger";
    }
    public void SetCharacterAsRogue()
    {
        currentSelectedCharacter = "Rogue";
        myImageComponent.sprite = rogueSprite;
        myClassNameText.text = "Rogue";
    }
    public void SetCharacterAsShaman()
    {
        currentSelectedCharacter = "Shaman";
        myImageComponent.sprite = shamanSprite;
        myClassNameText.text = "Shaman";
    }
    public void SetCharacterAsRandom()
    {
        currentSelectedCharacter = "Random";
        myImageComponent.sprite = randomSprite;
        myClassNameText.text = "Random";
    }
    #endregion

    // Click + Mouse Events
    #region
    public void OnNextCharacterButtonClicked()
    {
        Debug.Log("Next Character button clicked...");

        if(currentSelectedCharacter == "Random")
        {
            SetCharacterAsWarrior();
        }

        else if(currentSelectedCharacter == "Warrior")
        {
            SetCharacterAsPriest();
        }

        /*
        else if(currentSelectedCharacter == "Ranger")
        {
            SetCharacterAsPriest();
        }
        */

        else if(currentSelectedCharacter == "Priest")
        {
            SetCharacterAsMage();
        }
        else if(currentSelectedCharacter == "Mage")
        {
            SetCharacterAsRogue();
        }
        else if (currentSelectedCharacter == "Rogue")
        {
            SetCharacterAsRandom();
        }
        /*
        else if (currentSelectedCharacter == "Shaman")
        {
            SetCharacterAsRandom();
        }
        */
    }
    public void OnPreviousCharacterButtonClicked()
    {
        Debug.Log("Next Character button clicked...");

        if (currentSelectedCharacter == "Random")
        {
            SetCharacterAsRogue();
        }
        else if (currentSelectedCharacter == "Rogue")
        {
            SetCharacterAsMage();
        }

        else if (currentSelectedCharacter == "Mage")
        {
            SetCharacterAsPriest();
        }
        else if (currentSelectedCharacter == "Priest")
        {
            SetCharacterAsWarrior();
        }
        else if (currentSelectedCharacter == "Warrior")
        {
            SetCharacterAsRandom();
        }
       
    }
    #endregion

}
