using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing the display of the microscope to the player.
 */
public class MicroscopeUI : MonoBehaviour {

    [SerializeField] private Microscope microscope;

    [SerializeField] private Button nextItemButton;
    [SerializeField] private Button previousItemButton;

    [SerializeField] private Button nextImageButton;
    [SerializeField] private Button previousImageButton;

    [SerializeField] private Button closeMicroscopeButton;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI speciesName;

    private void Awake() {
        nextImageButton.onClick.AddListener(() => {
            microscope.NextPhotograph();
        });

        previousImageButton.onClick.AddListener(() => {
            microscope.PreviousPhotograph();
        });

        nextItemButton.onClick.AddListener(() => {
            microscope.NextItemToExamine();
        });

        previousItemButton.onClick.AddListener(() => {
            microscope.PreviousItemToExamine();
        });

        closeMicroscopeButton.onClick.AddListener(() => {
            Hide();
            microscope.ClearItems();
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

    public void UpdateImage(Sprite imageSprite) {
        image.sprite = imageSprite;
    }

    public void UpdateText(string speciesName) {
        this.speciesName.text = speciesName;
    }
}
