using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing the main screen of the sketch plan.
 * Note: The logic components are referenced here due to each part of the sketch plan
 * being activated by clicking on the respective sections.
 */
public class SketchPlanUI : MonoBehaviour {

    [SerializeField] private SketchDrawSpace drawSpace;
    [SerializeField] private SketchDetails details;
    [SerializeField] private SketchLegend legend;

    [SerializeField] private Button drawSpaceButton;
    [SerializeField] private Button detailsButton;
    [SerializeField] private Button legendButton;

    [SerializeField] private Image sketchPlanImage;

    private void Awake() {
        drawSpaceButton.onClick.AddListener(() => {
            drawSpace.Show();
        });
        detailsButton.onClick.AddListener(() => {
            details.Show();
        });
        legendButton.onClick.AddListener(() => {
            legend.Show();
        });
    }

    private void Start() {
        Hide();
    }

    public void Hide() {
        gameObject.SetActive(false);   
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void UpdateSketchImage(Sprite savedImage) {
        if (savedImage != null) {
            sketchPlanImage.sprite = savedImage;
        }
    }

}
