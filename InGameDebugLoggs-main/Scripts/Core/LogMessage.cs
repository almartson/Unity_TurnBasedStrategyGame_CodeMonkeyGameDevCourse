using System;
using UnityEngine;

[Serializable]
public struct LogMessage {
    public readonly LogType type;
    public readonly string typeName;
    public readonly string timeRecived;
    public readonly string condition;
    public readonly string stackTrace;

    public LogMessage(string condition, string stackTrace, LogType type, DateTime time) {
        this.condition = condition;
        this.stackTrace = stackTrace;
        this.type = type;
        this.typeName = type.ToString();
        this.timeRecived = time.ToString();
    }
}
