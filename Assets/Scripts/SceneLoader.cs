using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    int currentSceneIndex;

    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game Scene"); 
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu Scene");
    }
}
