using System.Collections;
using TMPro;
using UnityEngine;

/**
 * A UI element that represents the message log displayed to players in the game.
 */
public class MessageLogUI : MonoBehaviour {

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI messageText;

    private Coroutine showMessageCoroutine;
    
    private void Start() {
        canvasGroup.alpha = 0f;
    }

    /**
     * Displays the latest message to the player, removing the previous message if it is still shown.
     * 
     * @param message The message to display.
     */
    public void DisplayMessage(string message) {
        messageText.text = message;
        
        if (showMessageCoroutine != null) {
            StopCoroutine(showMessageCoroutine);
        }

        showMessageCoroutine = StartCoroutine(ShowMessageCoroutine());

    }

    private IEnumerator ShowMessageCoroutine() {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(3);
        canvasGroup.alpha = 0f;
    }

}
