using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StoryEventDataSO", menuName = "StoryEventDataSO", order = 53)]

public class StoryEventDataSO : ScriptableObject 
{
    public Sprite eventImageOne;
    public string eventName;
    [TextArea]
    public string eventDescription;

    [Header("Button One")]
    public bool actionButtonOneActive;
    public string actionButtonOneName;
    public string actionButtonOneDescription;

    [Header("Button Two")]
    public bool actionButtonTwoActive;
    public string actionButtonTwoName;
    public string actionButtonTwoDescription;

    [Header("Button Three")]
    public bool actionButtonThreeActive;
    public string actionButtonThreeName;
    public string actionButtonThreeDescription;
}
