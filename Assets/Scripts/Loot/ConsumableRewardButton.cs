using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConsumableRewardButton : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI consumableNameText;
    public Image consumableImage;

    [Header("Properties")]
    public ConsumableDataSO myData;

    public void InitializeSetup()
    {
        myData = ConsumableLibrary.Instance.GetRandomConsumable();
        consumableImage.sprite = myData.consumableSprite;
        consumableNameText.text = myData.consumableName;
    }
    public void OnConsumableButtonClicked()
    {
        if (ConsumableManager.Instance.HasAtleastOneSlotAvailble())
        {
            ConsumableManager.Instance.StartGainConsumableProcess(myData);
            Destroy(gameObject);
        }
       
    }
}
