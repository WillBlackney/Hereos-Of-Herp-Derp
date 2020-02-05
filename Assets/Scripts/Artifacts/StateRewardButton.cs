using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StateRewardButton : MonoBehaviour
{
    [Header("Component References")]
    public Image artifactImage;  
    
    // Mouse / Click Events
    #region
    public void OnStateRewardButtonClicked()
    {
        RewardScreen.Instance.EnableChooseStateRewardScreen();
    }    
  
    #endregion
}
