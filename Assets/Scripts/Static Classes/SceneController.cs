using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneController : MonoBehaviour
{
    [Header("Component References")]
    public Slider loadingScreenSlider;
    public TextMeshProUGUI loadingValueText;
    public GameObject loadScreenVisualParent;

    // Singleton Set up
    #region
    public static SceneController Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion

    // Load Scene Logic
    #region
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }
    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        bool fadeOutStarted = false;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        float startTime = Time.time;

        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingScreenSlider.value = progress;
            loadingValueText.text = (progress * 100).ToString();

            if(operation.progress >= 0.9f && fadeOutStarted == false)
            {
                fadeOutStarted = true;

                // brief wait before fade out
                yield return new WaitForSeconds(1);

                // Start screen fade transistion
                Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 2, 1, true);
                yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

                // disable load screen view
                loadScreenVisualParent.SetActive(false);

            }
            yield return null;
        }

        Debug.Log("Scene load duration: " + (Time.time - startTime).ToString() + " seconds");
        
    }
    #endregion
}
