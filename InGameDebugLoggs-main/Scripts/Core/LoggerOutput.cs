using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using TMPro;

public class LoggerOutput : MonoBehaviour {
    static string DEFAULT_PATH = "/Logger";
    static string FILENAME = "output";
    static string FILE_EXTENSION = ".txt";

    [SerializeField, HideInInspector] private string customOutputPath;

    public string outputPath => customOutputPath;

    private void AddDebugMessage(LogMessage message) {
        string newLogEntryAsJson = JsonUtility.ToJson(message, true);
        string path = GetSavePath();
        
        if(!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        string filePath = $"{path}/{FILENAME}{FILE_EXTENSION}";
        try {
            File.AppendAllText(filePath, newLogEntryAsJson);
        }
        catch(Exception e) {
            Debug.LogException(e);
        }
    }

    private void OnEnable() {
        InGameLogger.OnLogRecived += AddDebugMessage;
    }
    private void OnDisable() {
        InGameLogger.OnLogRecived -= AddDebugMessage;
    }

    public void SetOutputFolder(string path) {
        customOutputPath = path;
    }

    public string GetSavePath() {
        string path = $"{Application.persistentDataPath}{DEFAULT_PATH}{FILENAME}{FILE_EXTENSION}";
        if(!string.IsNullOrEmpty(customOutputPath)) {
            path= customOutputPath;
        }
        return path;
    }
}
