using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InGameLogger))]
public class DebuggerSettings : MonoBehaviour {
    private const string TOOL_MENU = "Debug Logger /";

    private static bool isLogginActive;
    private static bool isPersistantOverSceneEnabled;

    [SerializeField] private LoggerCanvas loggerUIprefab;
    [SerializeField] private bool persistantOverScenes;

    [SerializeField, HideInInspector] private static DebuggerSettings Instance;

    public bool IsLoggingActive => isLogginActive;

    public Action<bool> OnActivationChanged;


    #region InEditorMethods
#if UNITY_EDITOR
    [MenuItem(TOOL_MENU+"Add in Game UI")]
    public static void AddUI() {

        if(!FindInstance()) {
            return;
        }
        var instancedUI = FindObjectOfType<LoggerCanvas>();

        if(instancedUI == null && Instance.loggerUIprefab != null) {
            PrefabUtility.InstantiatePrefab(Instance.loggerUIprefab, Instance.transform);
        }
        else {
            Debug.Log("Game UI is allready present in current scene or no prefab is set on the gameobject" + Instance.gameObject);
        }
    }

    [MenuItem(TOOL_MENU + "Add output to file")]
    public static void AddOutput() {
        if(!FindInstance()) {
            return;
        }
        if(Instance.GetComponent<LoggerOutput>() == null) {
            Instance.gameObject.AddComponent<LoggerOutput>();
        }
        else {
            Debug.Log("Game log output is already present in current scene");
        }
    }
    [MenuItem(TOOL_MENU + "Select output Folder")]
    public static void SelectOutputFolder() {
        string path = EditorUtility.OpenFolderPanel("Select save folder", Application.dataPath, "DebugLogOutput");
        var loggerOutput = FindObjectOfType<LoggerOutput>();
        if(loggerOutput != null && !string.IsNullOrEmpty(path)) {
            loggerOutput.SetOutputFolder(path);
        }
    }

    private static bool TogglePersitsOverScenes() {
        bool results = false;


        if(FindInstance()) {
            Instance.persistantOverScenes = !Instance.persistantOverScenes;
            results= Instance.persistantOverScenes;
        }

        return results;
    }
    [MenuItem(TOOL_MENU + "Perist over scenes")]
    private static void ToggleAction() {
        isPersistantOverSceneEnabled = TogglePersitsOverScenes();
    }

    [MenuItem(TOOL_MENU + "Perist over scenes", true)]
    private static bool ToggleActionValidate() {

        Menu.SetChecked("DebugLogger/Perist over scenes", isPersistantOverSceneEnabled);
        return true;
    }

    [MenuItem(TOOL_MENU + "Activate Logging")]
    private static void ToggleActivation() {
        isLogginActive = ToggleLoggingActive();
    }

    [MenuItem(TOOL_MENU + "Activate Logging", true)]
    private static bool ToggleActivationValidate() {

        Menu.SetChecked(TOOL_MENU + "Activate Logging", isLogginActive);
        return true;
    }

#endif
    #endregion
    private static bool FindInstance() {
        if(Instance == null) {
            Instance = FindObjectOfType<DebuggerSettings>();

            // Re-check, to see if this was successfully:
            //
            if(Instance == null) {
                Debug.Log("No CustomDebugger in scene, Drag out a \"GameLogger.prefab\" into the scene to use this feature");
                return false;
            }
        }
        return true;
    }
    private static bool ToggleLoggingActive() {
        bool results = false;
        if(FindInstance()) {
            Instance.OnActivationChanged?.Invoke(true);
            results = !isLogginActive;
        }
        return results;
    }

    private void Awake() {
        if(persistantOverScenes) {
            DontDestroyOnLoad(gameObject);
        }
    }
    #region PublicMethods
    public void ActivateLogging(bool value) {
        isLogginActive= value;
        if(FindInstance() ) {
            Instance.OnActivationChanged?.Invoke(value);
        }
    }
    #endregion
}
