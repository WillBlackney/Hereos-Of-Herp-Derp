using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{    
    void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            HandleMouseRightClicked();            
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if(DefenderManager.Instance.selectedDefender.mySpellBook.AbilityOne != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilityOne.OnButtonClick();
                }
                
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if (DefenderManager.Instance.selectedDefender.mySpellBook.AbilityTwo != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilityTwo.OnButtonClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if (DefenderManager.Instance.selectedDefender.mySpellBook.AbilityThree != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilityThree.OnButtonClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if (DefenderManager.Instance.selectedDefender.mySpellBook.AbilityFour != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilityFour.OnButtonClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if (DefenderManager.Instance.selectedDefender.mySpellBook.AbilityFive != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilityFive.OnButtonClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if (DefenderManager.Instance.selectedDefender.mySpellBook.AbilitySix != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilitySix.OnButtonClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if (DefenderManager.Instance.selectedDefender.mySpellBook.AbilitySeven != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilitySeven.OnButtonClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if (DefenderManager.Instance.selectedDefender.mySpellBook.AbilityEight != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilityEight.OnButtonClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if (DefenderManager.Instance.selectedDefender.mySpellBook.AbilityNine != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilityNine.OnButtonClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (DefenderManager.Instance.selectedDefender != null)
            {
                if (DefenderManager.Instance.selectedDefender.mySpellBook.AbilityTen != null)
                {
                    DefenderManager.Instance.selectedDefender.mySpellBook.AbilityTen.OnButtonClick();
                }
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

    public void HandleMouseRightClicked()
    {
        ConsumableManager.Instance.ClearAllConsumableOrders();

        if (DefenderManager.Instance.selectedDefender != null &&
            !DefenderManager.Instance.selectedDefender.IsAwaitingOrder())
        {
            DefenderManager.Instance.ClearSelectedDefender();
        }
        else if (DefenderManager.Instance.selectedDefender != null &&
           DefenderManager.Instance.selectedDefender.IsAwaitingOrder())
        {
            DefenderManager.Instance.selectedDefender.ClearAllOrders();
        }
    }
}
