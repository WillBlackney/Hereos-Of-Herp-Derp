using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionManager : Singleton<GameSessionManager>
{
    public void StartGameOverEvent()
    {
        StopAllCoroutines();
        UIManager.Instance.EnableGameOverCanvas();
    }
}
