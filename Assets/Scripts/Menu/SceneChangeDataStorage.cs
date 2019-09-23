using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeDataStorage : Singleton<SceneChangeDataStorage>
{    

    public List<string> chosenCharacters = new List<string>();
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
