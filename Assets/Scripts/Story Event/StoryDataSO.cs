using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StoryDataSO", menuName = "StoryDataSO", order = 52)]
public class StoryDataSO : ScriptableObject
{
    [Header("General Properties")]
    public string storyName;
    [TextArea(10, 10)]
    public string storyInitialDescription;
    public Sprite storyInitialSprite;

    [Header("Story Pages")]
    public List<StoryPage> storyPages;
}
