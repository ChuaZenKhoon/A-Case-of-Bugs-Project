using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A manager in charge of handling messages to be sent to the player in the game.
 */
public class MessageLogManager : MonoBehaviour {

    public static MessageLogManager Instance { get; private set; }

    [SerializeField] private MessageLogUI messageLogUI;

    private void Awake() {
        Instance = this;
    }

    public void LogMessage(string message) {
        messageLogUI.DisplayMessage(message);
    }

}
