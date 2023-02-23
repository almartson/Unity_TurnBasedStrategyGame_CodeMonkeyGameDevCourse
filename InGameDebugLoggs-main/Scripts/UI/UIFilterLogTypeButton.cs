using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIFilterLogTypeButton : MonoBehaviour {
    [SerializeField] private LogType logType;
    [SerializeField] private LoggerCanvas loggerCanvas;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() =>loggerCanvas.ToggleType(logType));
    }

    
}
