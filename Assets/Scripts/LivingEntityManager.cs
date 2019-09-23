using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntityManager : MonoBehaviour
{
    public static LivingEntityManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<LivingEntity> allLivingEntities = new List<LivingEntity>();

}
