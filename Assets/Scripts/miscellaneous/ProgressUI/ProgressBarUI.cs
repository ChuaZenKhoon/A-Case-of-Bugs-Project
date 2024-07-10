using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing a progress bar display. It can be part of anything
 * that has some form of progress in its logic.
 */
public class ProgressBarUI : MonoBehaviour {


    [SerializeField] private Image bar;
    [SerializeField] private GameObject hasProgressGameObject;

    private const float EMPTY_PROGRESS_BAR = 0f;
    private const float FULL_PROGRESS_BAR = 0.99f;


    private IHasProgress hasProgress;
    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.Log("GameObject has no IHasProgress Component");
        }

        hasProgress.OnActionProgress += UpdateProgressBar;
        bar.fillAmount = EMPTY_PROGRESS_BAR;
        Hide();
    }

    private void UpdateProgressBar(object sender, float e) {
        bar.fillAmount = e;

        if (e == EMPTY_PROGRESS_BAR || e >= FULL_PROGRESS_BAR) {
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
