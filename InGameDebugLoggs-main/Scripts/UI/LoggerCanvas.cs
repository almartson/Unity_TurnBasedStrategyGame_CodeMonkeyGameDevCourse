using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoggerCanvas : MonoBehaviour {
    [SerializeField] private DebugMessage debugMessagePrefab;
    [SerializeField] private DetailedMessageInfo detailedMessageInfo;
    [SerializeField] private Transform contenTransform;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ToggleAbleCanvasGroup toggleAbleCavasGroup;
    [SerializeField] private bool showOnMessageRecived;

    private Dictionary<LogType,bool> showByType= new Dictionary<LogType,bool>();

    private void Awake() {
        AddLogTypesWithDefaultShowValue();
    }

    private void AddDebugMessage(LogMessage message) {
        bool shouldShowMessage = showByType[message.type];
        DebugMessage debugMessage = Instantiate(debugMessagePrefab, contenTransform);
        debugMessage.SetDebugMessageInfo(message);
        debugMessage.SetDetailedMessageInfoPanel(detailedMessageInfo);
        debugMessage.gameObject.SetActive(shouldShowMessage);
        scrollRect.verticalNormalizedPosition = 0f;
        if(showOnMessageRecived && shouldShowMessage && !toggleAbleCavasGroup.IsActive) {
            toggleAbleCavasGroup.ToggleCanvas();
        }
    }

    private void ActivateTypeOfLogMessages(bool setActive, LogType affectedType) {
        DebugMessage[] displayedMessages =  contenTransform.GetComponentsInChildren<DebugMessage>(true);
        
        for(int i = 0; i < displayedMessages.Length; i++) {
            var displayedMessage = displayedMessages[i];
            if(displayedMessage.MessageType == affectedType) {
                displayedMessage.gameObject.SetActive(setActive);
            }
            //displayedMessages[i].HideType(affectedType, setActive);
        }
    }
    private void AddLogTypesWithDefaultShowValue() {
        showByType.Add(LogType.Log, true);
        showByType.Add(LogType.Error, true);
        showByType.Add(LogType.Warning, true);
        showByType.Add(LogType.Exception, true);
        showByType.Add(LogType.Assert, true);
    }

    private void OnEnable() {
        InGameLogger.OnLogRecived += AddDebugMessage;
    }
    private void OnDisable() {
        InGameLogger.OnLogRecived -= AddDebugMessage;
    }
    
    public void ToggleType(LogType logType) {
        showByType[logType] = !showByType[logType];
        ActivateTypeOfLogMessages(showByType[logType], logType);
    }
}
