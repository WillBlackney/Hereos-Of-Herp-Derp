using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{    
    void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            UnselecteDefender();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                DefenderManager.Instance.selectedDefender.mySpellBook.AbilityOne.OnButtonClick();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                DefenderManager.Instance.selectedDefender.mySpellBook.AbilityTwo.OnButtonClick();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                DefenderManager.Instance.selectedDefender.mySpellBook.AbilityThree.OnButtonClick();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                DefenderManager.Instance.selectedDefender.mySpellBook.AbilityFour.OnButtonClick();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                DefenderManager.Instance.selectedDefender.mySpellBook.AbilityFive.OnButtonClick();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                DefenderManager.Instance.selectedDefender.mySpellBook.AbilitySix.OnButtonClick();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                DefenderManager.Instance.selectedDefender.mySpellBook.AbilitySeven.OnButtonClick();
            }
        }
    }
    public void UnselecteDefender()
    {
        if (DefenderManager.Instance.selectedDefender != null)
        {
            DefenderManager.Instance.ClearSelectedDefender();
        }
    }
}
