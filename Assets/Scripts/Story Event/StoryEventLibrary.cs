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

    public StoryDataSO GetRandomViableStory()
    {
        Debug.Log("StoryEventLibrary.GetRandomViableStory() called...");

        StoryDataSO storyReturned = null;
        storyReturned = viableStories[Random.Range(0, viableStories.Count)];

        if (storyReturned)
        {
            Debug.Log("StoryEventLibrary.GetRandomViableStory() randomly chose story event: " + storyReturned.storyName);
        }
        else
        {
            Debug.Log("StoryEventLibrary.GetRandomViableStory() could not find a random viable story event, returning null...");
        }

        return storyReturned;

    }
}
