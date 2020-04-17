using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("General Properties")]
    public bool applyActTwoModifiers;

    [Header("Act Difficulty Modifiers")]
    public int bonusStrength;
    public int bonusWisdom;
    public int bonusDexterity;
    public float bonusHealthPercentage;


    // Singleton Pattern
    #region
    public static DifficultyManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion


    public void ApplyActTwoModifiersToLivingEntity(LivingEntity entity)
    {
        Debug.Log("DifficultyManager.ApplyActTwoModifiersToLivingEntity() called for: " + entity.myName);

        if (applyActTwoModifiers && 
            WorldManager.Instance != null &&
            WorldManager.Instance.currentAct == 2)
        {
            entity.baseMaxHealth += (int)(entity.baseMaxHealth * bonusHealthPercentage);
            entity.baseStartingHealth += (int)(entity.baseMaxHealth * bonusHealthPercentage);
            entity.baseStrength += bonusStrength;
            entity.baseDexterity += bonusDexterity;
            entity.baseWisdom += bonusWisdom;
        }        
    }
}
