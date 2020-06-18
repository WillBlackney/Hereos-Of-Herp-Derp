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
        Debug.Log(Application.dataPath);

        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            // Check if save folder directory exists
            if (!Directory.Exists(Application.dataPath + "/Save Folders/Custom Characters"))
            {
                Debug.Log("Custom characters save folder does not exist, creating...");
                // it doesnt, create save folder
                SAVE_FOLDER_CUSTOM_CHARACTERS = Application.dataPath + "/Save Folders/Custom Characters";
                Directory.CreateDirectory(SAVE_FOLDER_CUSTOM_CHARACTERS);                
            }
            else
            {
                Debug.Log("Custom characters save folder already exists at: " + Application.dataPath + "/Save Folders/Custom Characters");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

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

}
