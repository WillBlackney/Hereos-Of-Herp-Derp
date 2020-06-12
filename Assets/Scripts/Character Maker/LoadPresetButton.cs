using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadPresetButton : MonoBehaviour
{
    public TextMeshProUGUI presetNameText;
    public CharacterPresetData presetData;

    public void InitializeSetup(CharacterPresetData data)
    {
        presetData = data;
        presetNameText.text = data.characterName;
    }
    public void OnButtonClick()
    {
        MainMenuManager.Instance.OnLoadPresetButtonClicked(this);
    }
}
