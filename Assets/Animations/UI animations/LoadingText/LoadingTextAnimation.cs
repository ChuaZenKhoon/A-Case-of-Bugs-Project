using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingTextAnimation : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI loadingText;

    private void Start() {
        StartCoroutine(AnimatedLoadingText());
    }


    private IEnumerator AnimatedLoadingText() {
        while (this.gameObject.activeSelf) {
            loadingText.text = "Loading";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading.";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading..";
            yield return new WaitForSeconds(0.5f);
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(0.5f);
        }

    }
}
