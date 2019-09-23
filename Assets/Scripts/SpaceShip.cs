using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : Defender
{
    public override IEnumerator HandleDeath()
    {
        GameSessionManager.Instance.StartGameOverEvent();
        StartCoroutine(base.HandleDeath());
        yield return null;

    }
}
