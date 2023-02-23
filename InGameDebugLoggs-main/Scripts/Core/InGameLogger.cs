using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameLogger : MonoBehaviour {
    public static event Action<LogMessage> OnLogRecived;

    private DebuggerSettings settings;

    List<LogMessage> registredMessages = new List<LogMessage>();
    public List<LogMessage> GetAllMessages => registredMessages;
    private void Awake() {
        settings = GetComponent<DebuggerSettings>();

        if(settings == null) {
            ActivateLogTracking(true);
        }
        else {
            settings.OnActivationChanged += ActivateLogTracking;
            ActivateLogTracking(settings.IsLoggingActive);
        }
        
    }

    public void SetActive(bool setActive) {
        ActivateLogTracking(setActive);
    }

    private void ActivateLogTracking(bool activate) {
        Application.logMessageReceived -= HandleLogMessageReceived;
        if(activate) {
            Application.logMessageReceived += HandleLogMessageReceived;
        }
    }
    private void HandleLogMessageReceived(string condition, string stackTrace, LogType type) {

        DateTime time = DateTime.Now;
        var logMessage = new LogMessage(condition, stackTrace, type, time);
        registredMessages.Add(logMessage);
        OnLogRecived?.Invoke(logMessage);
    }

    [ContextMenu("Add one of each message type")]
    public void AddTestMessages() {
        Debug.Log("Hi, I am a log");
        Debug.LogWarning("I am a warning!");
        Debug.LogError("I AM ERROR!");
    }
    [ContextMenu("Add one log message")]
    public void AddTestLogMessages() {
        Debug.Log("Hi, I am a log");
    }
}
