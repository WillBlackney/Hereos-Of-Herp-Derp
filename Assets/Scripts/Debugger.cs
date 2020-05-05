using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Debugger
{
    public static bool loggingEnabled;
    public static void SetLoggingState(bool onOrOff)
    {
        if (onOrOff)
        {
            loggingEnabled = true;
            Debug.unityLogger.logEnabled = true;
        }
        else
        {
            loggingEnabled = false;
            Debug.unityLogger.logEnabled = false;
        }
    }
    public static void Log(string logMessage)
    {
        if (loggingEnabled)
        {
            Debug.Log(logMessage);
        }
        
    }
}
