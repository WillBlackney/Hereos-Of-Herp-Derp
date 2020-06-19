using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PersistencyManager : MonoBehaviour
{
    public string SAVE_FOLDER_CUSTOM_CHARACTERS { get; private set; }

    // Singleton Pattern
    #region
    public static PersistencyManager Instance;
    private void Awake()
    {
        BuildDirectories();

        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            // Check if save folder directory exists
            if (!Directory.Exists(SAVE_FOLDER_CUSTOM_CHARACTERS))
            {
                Debug.Log("Custom characters save folder does not exist, creating...");
                // it doesnt, create save folder
                //SAVE_FOLDER_CUSTOM_CHARACTERS = Application.dataPath + "/Save Folders/Custom Characters";
                Directory.CreateDirectory(SAVE_FOLDER_CUSTOM_CHARACTERS);                
            }
            else
            {
                Debug.Log("Custom characters save folder already exists at: " + SAVE_FOLDER_CUSTOM_CHARACTERS);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    // Initialization + Setup
    public void BuildDirectories()
    {
        SAVE_FOLDER_CUSTOM_CHARACTERS = Application.dataPath + "/Save Folders/Custom Characters";
    }
    // JSON Logic
    #region
    public string ConvertObjectToJsonString(object t, bool printToLog = true)
    {
        Debug.Log("PersistencyManager.ConvertObjectToJsonString() called...");

        string jsonString = JsonUtility.ToJson(t);
        if (printToLog)
        {
            Debug.Log("PersistencyManager.ConvertObjectToJsonString() converted object(" +
                t.ToString() + ") to the following JSON string: ");
            Debug.Log(jsonString);
        }

        return jsonString;
    }
    public T ConvertJsonStringToObject<T>(string jsonString, bool printToLog = true)
    {
        Debug.Log("PersistencyManager.ConvertJsonStringToObject() called...");
        Type t = typeof(T);
        if (printToLog)
        {
            Debug.Log("Creating a " + t.ToString() + " object from the following JSON string: ");
            Debug.Log(jsonString);
        }
        return JsonUtility.FromJson<T>(jsonString);
    }
    #endregion

    // Character Preset Data Save + Load Logic
    public void SaveCharacterPresetDataToPersistency(CharacterPresetData charData)
    {
        Debug.Log("PersistencyManager.SaveCharacterPresetDataToPersistency() called...");

        string jsonString = ConvertObjectToJsonString(charData);
        File.WriteAllText(SAVE_FOLDER_CUSTOM_CHARACTERS + "/" + charData.characterName + ".txt", jsonString);
    }
    public List<CharacterPresetData> LoadAllCharacterPresetDataFromPersistency()
    {
        Debug.Log("PersistencyManager.LoadAllCharacterPresetDataFromPersistency() called...");

        List<string> allCharacterJsonStrings = new List<string>();
        List<CharacterPresetData> allCharacterDataPresets = new List<CharacterPresetData>(); 

        // get JSon strings from the text files
        foreach (string textFilePath in Directory.GetFiles(SAVE_FOLDER_CUSTOM_CHARACTERS, "*.txt"))
        {
            Debug.Log("Checking text file at directory: " + textFilePath);

            string jsonS = File.ReadAllText(textFilePath);
            Debug.Log("Json string at directory: " + jsonS);

            allCharacterJsonStrings.Add(jsonS);            
        }

        // Convert the json strings into characterDataPreset objects
        foreach(string jsonString in allCharacterJsonStrings)
        {
            allCharacterDataPresets.Add(ConvertJsonStringToObject<CharacterPresetData>(jsonString));
        }

        // return all the data
        return allCharacterDataPresets;
        
    } 
}
