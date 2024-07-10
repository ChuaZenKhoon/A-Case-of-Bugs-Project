using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing the drawing section of the sketch plan.
 */
public class SketchDrawSpaceUI : MonoBehaviour {

    [SerializeField] private SketchDrawSpace drawSpace;
    [SerializeField] private DrawingTool drawingTool;

    [SerializeField] private Button backButton;

    private void Awake() {
        backButton.onClick.AddListener(() => {
            drawSpace.SaveDetails(drawingTool.GetCanvasTexture());
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


}
