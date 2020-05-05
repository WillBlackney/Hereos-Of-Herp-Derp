using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Debug Properties")]
    public bool enableDebugLog;
    #endregion

    // Singleton Pattern 
    #region
    public static GlobalSettings Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Initialization + Start
    #region
    public void OnAwake()
    {
        Debugger.SetLoggingState(enableDebugLog);
    }
    #endregion

}
