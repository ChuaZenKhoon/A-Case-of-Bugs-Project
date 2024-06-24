using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageLogManager : MonoBehaviour {

    public static MessageLogManager Instance { get; private set; }

    public event EventHandler<string> OnLogMessage;

    private void Awake() {
        Instance = this;
    }

    public void LogMessage(string message) {
        OnLogMessage?.Invoke(this, message);
    }

}
