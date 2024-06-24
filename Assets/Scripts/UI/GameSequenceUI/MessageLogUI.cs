using System.Collections;
using TMPro;
using UnityEngine;

public class MessageLogUI : MonoBehaviour {

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI messageText;

    private Coroutine showMessageCoroutine;
    
    private void Start() {
        MessageLogManager.Instance.OnLogMessage += MessageLogManager_OnLogMessage;
        canvasGroup.alpha = 0f;
    }

    private void MessageLogManager_OnLogMessage(object sender, string e) {
        messageText.text = e;
        
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
