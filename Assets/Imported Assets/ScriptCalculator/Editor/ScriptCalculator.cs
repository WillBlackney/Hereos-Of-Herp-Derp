#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class ScriptCalculator : EditorWindow
{
    #region Variables

    public static EditorWindow _window;

    private Texture2D _backgroundtexture;
    private Texture2D backgroundTexture
    {
        get
        {
            if (_backgroundtexture == null)
            {
                _backgroundtexture = Resources.Load<Texture2D>("ScriptCalculator/Backgorund");
            }
            return _backgroundtexture;
        }
    }

    private string _searchInDirectoryText = "";
    private bool _searchInDirectory = false;
    private bool _scriptExtensionsCS = true;
    private bool _scriptExtensionsJS = true;
    private bool _passBlankLines = false;
    private bool _isActiveFilesArea = false;
    private Vector2 _scrollPos = Vector2.zero;
    private int _countScript = -1;
    private int _totalLineCount = -1;
    private List<string> _fileNames = new List<string>();
    #endregion
    
    #region For Init Window
    [MenuItem("Tools/Script And Code Line Calculator")]
    static void Init()
    {
        if (_window != null)
        {
            _window.Close();
        }
        _window = CreateInstance<ScriptCalculator>();
        _window.wantsMouseMove = false;
        _window.position = new Rect(Screen.width, Screen.height / 2, 300, 380);
        CenterOnMainWin(_window);
        _window.ShowPopup();

    }
    #endregion

    #region For Draw Window
    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 300, 380), backgroundTexture, ScaleMode.StretchToFill);

        var style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;

        var style2 = new GUIStyle();
        style2.alignment = TextAnchor.MiddleCenter;


        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUILayout.LabelField("Script And Code Line Calculator", style);
        if (GUILayout.Button("X", GUILayout.Width(25)))
        {
            this.Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        _searchInDirectory = EditorGUILayout.Toggle("Search In Directory ?", _searchInDirectory);
        if (_searchInDirectory)
        {
            _searchInDirectoryText = EditorGUILayout.TextField("Search In : ", _searchInDirectoryText);
        }



        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        _scriptExtensionsCS = EditorGUILayout.ToggleLeft("CS", _scriptExtensionsCS);
        _scriptExtensionsJS = EditorGUILayout.ToggleLeft("JS", _scriptExtensionsJS);
        _passBlankLines = EditorGUILayout.ToggleLeft("Pass the blank lines", _passBlankLines);
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical(style2);

        if (GUILayout.Button("Search", GUILayout.Width(290)))
        {
            _isActiveFilesArea = false;
            _fileNames.Clear();
            _countScript = 0;
            _totalLineCount = 0;

            if (!_scriptExtensionsJS && !_scriptExtensionsCS)
            {
                EditorUtility.DisplayDialog("Error", "You must to choose at least one script type", "Ok");
                return;
            }
            
            int _assetCount = AssetDatabase.FindAssets("t:Script").Length;

            for (int i = 0; i < _assetCount; i++)
            {
                float _progreassValue = 1f / (float)_assetCount;
                EditorUtility.DisplayProgressBar("Please Wait", "Calculating...", _progreassValue);

                string _file = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:Script")[i]);


                if ((_searchInDirectory && _searchInDirectoryText.Length > 0) ?
                    ((_file.ToLower().Contains("/" + _searchInDirectoryText.ToLower()) || _file.ToLower().Contains(_searchInDirectoryText.ToLower() + "/")) &&
                    ((_scriptExtensionsCS && (_file.Substring(_file.Length - 3).ToLower() == ".cs")) ||
                    (_scriptExtensionsJS && (_file.Substring(_file.Length - 3).ToLower() == ".js"))
                    )) :
                    ((_scriptExtensionsCS && (_file.Substring(_file.Length - 3).ToLower() == ".cs")) ||
                    (_scriptExtensionsJS && (_file.Substring(_file.Length - 3).ToLower() == ".js"))))
                {
                    _countScript++;

                    TextAsset TA = (TextAsset)AssetDatabase.LoadAssetAtPath(_file, typeof(TextAsset));
                    _fileNames.Add(_file);

                    if (_passBlankLines)
                    {
                        string[] _lines = TA.text.Split('\n');
                        _totalLineCount += _lines.Count(_line => (_line.Any(char.IsLetterOrDigit)));

                    }
                    else
                    {
                        
                         _totalLineCount += TA.text.Split('\n').Length;
                    }

                    
                }



            }
            EditorUtility.ClearProgressBar();

            if (_countScript == 0)
            {
                EditorUtility.DisplayDialog("Info", "There were no results", "Ok");
            }

        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Script Count : " + (_countScript == -1 ? "Not calculated" : _countScript.ToString()), MessageType.Info);
        EditorGUILayout.HelpBox("Total Line Count : " + (_totalLineCount == -1 ? "Not calculated" : _totalLineCount.ToString()), MessageType.Info);


        EditorGUILayout.Space();


        _isActiveFilesArea = EditorGUILayout.Foldout(_isActiveFilesArea, "Files");

        if (_isActiveFilesArea)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);

            if (_fileNames.Count > 0)
            {
                for (int i = 0; i < _fileNames.Count; i++)
                {
                    EditorGUILayout.LabelField(_fileNames[i], GUILayout.Width(700));

                }
            }
            else
            {
                EditorGUILayout.LabelField("There were no results", GUILayout.Width(130));
            }



            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

        }


    }
    #endregion

    #region For Window Center
    private static void CenterOnMainWin(EditorWindow aWin)
    {
        var main = GetEditorMainWindowPos();
        var pos = aWin.position;
        float w = (main.width - pos.width) * 0.5f;
        float h = (main.height - pos.height) * 0.5f;
        pos.x = main.x + w;
        pos.y = main.y + h;
        aWin.position = pos;
    }
    private static Rect GetEditorMainWindowPos()
    {
        var containerWinType = GetAllDerivedTypes().Where(t => t.Name == "ContainerWindow").FirstOrDefault();

        if (containerWinType == null)
            throw new System.MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
        var showModeField = containerWinType.GetField("m_ShowMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var positionProperty = containerWinType.GetProperty("position", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        if (showModeField == null || positionProperty == null)
            throw new System.MissingFieldException("Can't find internal fields 'm_ShowMode' or 'position'. Maybe something has changed inside Unity");
        var windows = Resources.FindObjectsOfTypeAll(containerWinType);
        foreach (var win in windows)
        {
            var showmode = (int)showModeField.GetValue(win);
            if (showmode == 4) // main window
            {
                var pos = (Rect)positionProperty.GetValue(win, null);
                return pos;
            }
        }
        throw new System.NotSupportedException("Can't find internal main window. Maybe something has changed inside Unity");
    }
    private static System.Type[] GetAllDerivedTypes()
    {
        var result = new List<System.Type>();
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        System.Type aType = typeof(ScriptableObject);


        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(aType))
                    result.Add(type);
            }
        }
        return result.ToArray();
    }
    #endregion
}
#endif