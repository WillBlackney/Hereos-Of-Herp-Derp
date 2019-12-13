using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntityManager : MonoBehaviour
{
    public List<LivingEntity> allLivingEntities = new List<LivingEntity>();

    // Initialization + Singleton Pattern
    #region
    public static LivingEntityManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Call this when the player loses the game
    public void StopAllEntityCoroutines()
    {
        foreach(LivingEntity entity in allLivingEntities)
        {
            entity.StopAllCoroutines();
        }
    }
    #endregion



}
