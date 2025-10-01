using System;
using UnityEngine;

[Serializable]
public class LogEntry
{
    public string timestamp;
    public string type;
    public int level;
    public string context;
    public string message;
    public string stacktrace;
}
