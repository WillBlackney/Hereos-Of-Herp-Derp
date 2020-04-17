using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Enemy : LivingEntity
{
    [Header("Enemy Components")]
    public EnemyInfoPanel myInfoPanel;
    public GameObject freeStrikeIndicator;

    // Initialization + Setup
    #region
    public override void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {              
        EnemyManager.Instance.allEnemies.Add(this);        
        base.InitializeSetup(startingGridPosition, startingTile);
        myInfoPanel.InitializeSetup(this);
        
    }
    public override void SetBaseProperties()
    {
        DifficultyManager.Instance.ApplyActTwoModifiersToLivingEntity(this);
        base.SetBaseProperties();
    }
    #endregion

    // Activation + Related
    #region
    public virtual void StartMyActivation()
    {
        if (!inDeathProcess)
        {
            StartCoroutine(StartMyActivationCoroutine());
        }
        else
        {
            Debug.Log("Enemy.StartMyActivation() on " + enemy.myName + " detected that bool 'inDeathProcess' is true, stopping activation from starting...");
        }
    }
    public virtual IEnumerator StartMyActivationCoroutine()
    {
        yield return null;
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

    // View + UI Logic
    public void SetFreeStrikeIndicatorViewState(bool onOrOff)
    {
        if(onOrOff == true)
        {
            freeStrikeIndicator.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            freeStrikeIndicator.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    #endregion

}
