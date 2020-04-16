using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI currentGoldText;    

    [Header("Properties")]
    public int startingGold;
    public int currentGold;

    public static PlayerDataManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Initialization + Setup
    #region
    private void Start()
    {
        InitializeSetup();
    }
    public void InitializeSetup()
    {
        ModifyGold(startingGold);
    }
    #endregion

    public void ModifyGold(int goldGainedOrLost)
    {
        Debug.Log("PlayerDataManager.ModifyGold() called, initial gold gain value = " + goldGainedOrLost.ToString());
        int goldGainFinalValue = 0;

        // Check pennywise state
        if(StateManager.Instance.DoesPlayerAlreadyHaveState("Pennywise") && goldGainedOrLost > 0)
        {
            goldGainFinalValue = (int) (goldGainedOrLost * 1.3f);
            Debug.Log("PlayerDataManager.ModifyGold() detected player has 'Pennywise' state, new gold gain value: " + goldGainFinalValue.ToString());
        }
        else
        {
            goldGainFinalValue = goldGainedOrLost;
        }

        // Check Oath Of Honour State
        if(StateManager.Instance.DoesPlayerAlreadyHaveState("Oath Of Honour") && goldGainedOrLost > 0)
        {
            Debug.Log("PlayerDataManager.ModifyGold() detected player has 'Oath Of Honour' state, reducing gold gain to 0");
            goldGainFinalValue = 0;
        }

        currentGold += goldGainFinalValue;
        currentGoldText.text = currentGold.ToString();

        // Modify player score
        if(goldGainFinalValue > 0)
        {
            ScoreManager.Instance.goldGained += goldGainFinalValue;
        }
        
    }
}
