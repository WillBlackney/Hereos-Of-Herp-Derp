using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventLibrary : MonoBehaviour
{
    [Header("Properties")]
    public List<StoryDataSO> allStories;
    public List<StoryDataSO> viableStories;

    // Singleton Pattern
    #region
    public static StoryEventLibrary Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void GetRandomViableStory()
    {

    }
}
