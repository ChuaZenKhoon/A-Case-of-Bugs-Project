using System;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing the display shown to the user when entering the tutorial.
 */
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
