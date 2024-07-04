using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroMessageUI : MonoBehaviour {

    public event EventHandler OnFinishReading;

    [SerializeField] private Button closeButton;

    private void Awake() {
        closeButton.onClick.AddListener(() => {
            OnFinishReading?.Invoke(this, EventArgs.Empty);
            Hide();
        });
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
