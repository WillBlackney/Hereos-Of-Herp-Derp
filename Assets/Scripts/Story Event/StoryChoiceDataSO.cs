using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "New StoryChoiceDataSO", menuName = "StoryChoiceDataSO", order = 52)]
public class StoryChoiceDataSO : ScriptableObject
{
    [Header("General Properties")]
    public string description;
    public int baseSuccessChance;

    [Header("Requirements To Unlock Choice")]
    public List<ChoiceRequirment> choiceRequirements;

    [Header("Rewards On Success")]
    public List<ChoiceConsequence> onSuccessConsequences;

    [Header("Consequences On Failure")]
    public List<ChoiceConsequence> onFailureConsequences;

}

[Serializable]
public class ChoiceRequirment
{
    public enum RequirementType { None, HasEnoughGold, HasBackground, HasRace};
    [Header("General Properties")]
    public RequirementType requirementType;
    public int requirementTypeValue;

    [Header("Specific Properties")]
    public UniversalCharacterModel.ModelRace raceRequirement;
    public CharacterData.Background backgroundRequirement;
}

/*
[CustomPropertyDrawer(typeof(ChoiceRequirment))]
public class ChoiceRequirmentPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var raceRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var backgroundRect = new Rect(position.x, position.y + 20f, position.width, EditorGUIUtility.singleLineHeight);

        var race = property.FindPropertyRelative("raceRequirement");
        var name = property.FindPropertyRelative("backgroundRequirement");

        race.intValue = EditorGUI.Popup(raceRect, "raceRequirement", race.intValue, race.enumNames);

        switch ((UniversalCharacterModel.ModelRace)race.intValue)
        {
            case UniversalCharacterModel.ModelRace.None:
                name.stringValue = EditorGUI.TextField(backgroundRect, "Action Name", name.stringValue);
                //Anything else you want to display
                break;
            case UniversalCharacterModel.ModelRace.SimpleDamage:
                name.stringValue = EditorGUI.TextField(backgroundRect, "Action Name", name.stringValue);
                //Anything else you want to display
                break;
            case ActionEffects.PushSingleTarget:
                name.stringValue = EditorGUI.TextField(backgroundRect, "Action Name", name.stringValue);
                //Anything else you want to display
                break;
        }

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    //This will need to be adjusted based on what you are displaying
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (20 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight * 2);
    }
}

*/


/*
[CustomEditor(typeof(ChoiceRequirment))]
public class ChoiceRequirmentEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as ChoiceRequirment;

        myScript.flag = GUILayout.Toggle(myScript.flag, "Flag");

        if (myScript.flag)
            myScript.i = EditorGUILayout.IntSlider("I field:", myScript.i, 1, 100);
    }
}
*/

[Serializable]
public class ChoiceConsequence
{
    public enum ConsequenceType { None, EventEnds, GainGold, AllCharactersGainXP, TriggerCombatEvent};

    [Header("General Properties")]
    public ConsequenceType consequenceType;
    public int consequenceTypeValue;

    [Header("Trigger Combat Properties")]
    public EnemyWaveSO combatEvent;
}
