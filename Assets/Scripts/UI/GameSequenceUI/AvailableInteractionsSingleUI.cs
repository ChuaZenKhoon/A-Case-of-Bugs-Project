using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * A UI element representing a singular interaction key and its details that the player can press.
 */
public class AvailableInteractionsSingleUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI interactionKey;
    [SerializeField] private TextMeshProUGUI interactionText;

    public void SetUpInteraction(string buttonText, string descriptionText) {
        interactionKey.text = buttonText;
        interactionText.text = descriptionText;
    }
}
