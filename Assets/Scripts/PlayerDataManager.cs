using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [Header("Component References")]
    public TextMeshProUGUI currentGoldText;    

    [Header("Properties")]
    public int startingGold;
    public int currentGold;

    private void Start()
    {
        InitializeSetup();
    }
    public void InitializeSetup()
    {
        ModifyGold(startingGold);
    }

    public void ModifyGold(int goldGainedOrLost)
    {
        currentGold += goldGainedOrLost;
        currentGoldText.text = currentGold.ToString();
    }
}
