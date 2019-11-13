using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Enemy : LivingEntity
{
    [Header("Enemy Components")]
    public CharacterInfoPanel myInfoPanel;
    [Header("Enemy Properties")]
    public string myName;

    // Initialization + Setup
    #region
    public override void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {              
        EnemyManager.Instance.allEnemies.Add(this);        
        base.InitializeSetup(startingGridPosition, startingTile);
        myInfoPanel.InitializeSetup(this);
        
    }
    #endregion

    // Activation + Related
    #region
    public virtual void StartMyActivation()
    {       
        StartCoroutine(StartMyActivationCoroutine());
    }
    public virtual IEnumerator StartMyActivationCoroutine()
    {
        yield return null;
    }
    public Action EndMyActivation()
    {
        Action action = new Action();
        StartCoroutine(EndMyActivationCoroutine(action));
        return action;

    }
    public IEnumerator EndMyActivationCoroutine(Action action)
    {
        Action endActivation = ActivationManager.Instance.EndEntityActivation(this);
        yield return new WaitUntil(() => endActivation.ActionResolved() == true);
        action.actionResolved = true;
        ActivationManager.Instance.ActivateNextEntity();
    }

    public bool currentlyActivated = false;
    public bool ActivationFinished()
    {
        if (currentlyActivated == false)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    #endregion

    // AI Targeting Logic
    #region
    public void SetTargetDefender(LivingEntity target)
    {
        myCurrentTarget = target;
    }
    
    public Defender GetMostVulnerableDefender()
    {
        Defender bestTarget = null;
        int pointScore = 0;

        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            int myPointScore = 1;


            if(myPointScore > pointScore)
            {
                pointScore = myPointScore;
                bestTarget = defender;
            }
        }

        return bestTarget;
    }
    public Defender GetDefenderWithLowestCurrentHP()
    {
        Defender bestTarget = null;
        int lowestHP = 1000;

        // Declare new temp list for storing defender 
        List<Defender> defenders = new List<Defender>();

        // Add all active defenders to the temp list
        foreach (Defender defender in DefenderManager.Instance.allDefenders)
        {
            defenders.Add(defender);
        }

        foreach (Defender defender in defenders)
        {            
            if (defender.currentHealth < lowestHP)
            {
                bestTarget = defender;
                lowestHP = defender.currentHealth;                
            }            
        }

        if(bestTarget == null)
        {
            Debug.Log("GetDefenderWithLowestCurrentHP() returning null !!...");
        }

        return bestTarget;
    }
    #endregion    

    // Mouse + Click Events
    #region
    public void OnMouseDown()
    {
        Debug.Log("Enemy click detected");
        EnemyManager.Instance.SelectEnemy(this);
    }
    public void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            myInfoPanel.EnablePanelView();
        }
    }
    #endregion

}
