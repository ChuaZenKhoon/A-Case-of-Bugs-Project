using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {


    [SerializeField] private Image bar;
    [SerializeField] private GameObject hasProgressGameObject;

    private IHasProgress hasProgress;
    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.Log("GameObject has no IHasProgress Component");
        }

        hasProgress.OnActionProgress += UpdateProgressBar;
        bar.fillAmount = 0f;
        Hide();
    }

    private void UpdateProgressBar(object sender, float e) {
        bar.fillAmount = e;

        if (e == 0f || e == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
