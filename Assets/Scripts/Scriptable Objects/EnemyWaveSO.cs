using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyWaveSO", menuName = "EnemyWaveSO", order = 51)]
public class EnemyWaveSO : ScriptableObject
{
    public string waveName;
    public List<EnemyGroup> enemyGroups;
}

[System.Serializable]
public class EnemyGroup
{
    public List<GameObject> enemyList;
}
