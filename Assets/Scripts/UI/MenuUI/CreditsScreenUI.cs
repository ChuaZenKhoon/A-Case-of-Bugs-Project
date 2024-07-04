using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element representing the credits screen in the main menu
 */
public class CreditsScreenUI : MonoBehaviour {

    [SerializeField] private Image assetsCredits;
    
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private void Awake() {
        nextButton.onClick.AddListener(() => {
            NextPage();
        });
        prevButton.onClick.AddListener(() => {
            PrevPage();
        });
    }

    private void Start() {
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void NextPage() {
        assetsCredits.gameObject.SetActive(true);
        nextButton.interactable = false;
    }

    private void PrevPage() {
        if (!assetsCredits.gameObject.activeSelf) {
            Hide();
            return;
        }

        assetsCredits.gameObject.SetActive(false);
        nextButton.interactable = true;
        
    }
}
