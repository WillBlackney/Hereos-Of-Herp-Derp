using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlessingChoiceButton : MonoBehaviour
{
    public BlessingChoice myData;
    public TextMeshProUGUI choiceNameText;
    public void OnChoiceButtonClicked()
    {
        KingsBlessingManager.Instance.StartChoiceProcess(myData.choiceName);
    }

    public void BuildButtonFromData(BlessingChoice data)
    {
        myData = data;
        choiceNameText.text = myData.choiceName;
    }
}
