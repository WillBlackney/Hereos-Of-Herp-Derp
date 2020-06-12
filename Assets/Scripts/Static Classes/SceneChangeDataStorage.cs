using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeDataStorage : MonoBehaviour
{    
    public List<CharacterPresetData> chosenCharacters = new List<CharacterPresetData>();
    public static SceneChangeDataStorage Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
}
