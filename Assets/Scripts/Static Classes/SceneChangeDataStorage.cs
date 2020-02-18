using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeDataStorage : MonoBehaviour
{    
    public List<string> chosenCharacters = new List<string>();
    public static SceneChangeDataStorage Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
}
