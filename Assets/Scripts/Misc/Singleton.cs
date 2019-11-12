using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T>  : MonoBehaviour where T : Singleton<T>
{
    // Classes that inherit from this will implement a 'Lazy Singleton Pattern'
    // Used by most of the manager/logic operator classes

    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
            }

            return instance;
        }
    }
	
}
