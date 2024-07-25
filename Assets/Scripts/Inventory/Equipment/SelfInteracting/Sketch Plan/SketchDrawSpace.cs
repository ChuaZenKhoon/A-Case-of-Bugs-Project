using System;
using UnityEngine;

/**
 * The logic component of the sketch drawing section in the sketch plan equipment.
 * Note: The drawing logic can be found in the Drawing Tool class.
 */
public class SketchDrawSpace : MonoBehaviour {

    public event EventHandler OnSketchViewToggled;
    public event EventHandler OnClearSketchDone;
    public event EventHandler OnUndoLineDone;


    [SerializeField] private SketchPlan sketchPlan;
    [SerializeField] private SketchDrawSpaceUI drawSpaceUI;
    [SerializeField] private DrawingTool drawingTool;

    [SerializeField] private CanvasGroup canvasVisibility;
    [SerializeField] private Canvas toggleViewCanvas;

    private static Color BRUSH_DRAW_COLOR = Color.black;
    private const float BRUSH_SIZE = 3f;

    private bool isViewToggled;

    private void Awake() {
        isViewToggled = false;
    }

    private void Start() {
        GameInput.Instance.OnToggleSketchView += GameInput_OnToggleSketchView;
        GameInput.Instance.OnClearSketch += GameInput_OnClearSketch;
        GameInput.Instance.OnInteract2Action += GameInput_OnInteract2Action;
    }

    private void OnDestroy() {
        GameInput.Instance.OnToggleSketchView -= GameInput_OnToggleSketchView;
        GameInput.Instance.OnClearSketch -= GameInput_OnClearSketch;
        GameInput.Instance.OnInteract2Action -= GameInput_OnInteract2Action;
    }

    private void GameInput_OnToggleSketchView(object sender, System.EventArgs e) {
        if (drawSpaceUI.gameObject.activeSelf) {
            isViewToggled = !isViewToggled;

            if (isViewToggled) {
                canvasVisibility.alpha = 0f;
                toggleViewCanvas.gameObject.SetActive(true);
            } else {
                canvasVisibility.alpha = 1f;
                toggleViewCanvas.gameObject.SetActive(false);
            }

            OnSketchViewToggled?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnClearSketch(object sender, System.EventArgs e) {
        if (drawSpaceUI.gameObject.activeSelf) {
            drawingTool.ClearCanvas();
            OnClearSketchDone?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnInteract2Action(object sender, System.EventArgs e) {
        if (drawSpaceUI.gameObject.activeSelf) {
            drawingTool.UndoLastLine();
            OnUndoLineDone?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Show() {
        sketchPlan.SetSketchMode(true);
        drawSpaceUI.Show();
    }

    public void Hide() {
        sketchPlan.SetSketchMode(false);
        drawSpaceUI.Hide();
    }
    public void UpdateSketchImage() {
        sketchPlan.UpdateSketchImages();
    }
    public void SaveDetails(Texture2D canvasTexture) {
        EquipmentStorageManager.Instance.UpdateSketchPlan(canvasTexture);
        SaveSketchImage(canvasTexture);
        UpdateSketchImage();
        Hide();
    }
    private void SaveSketchImage(Texture2D canvasTexture) {
        Sprite sprite = Sprite.Create(canvasTexture, new Rect(0, 0, canvasTexture.width, canvasTexture.height), new Vector2(0.5f, 0.5f));
        EquipmentStorageManager.Instance.UpdateSavedSketchImages(sprite);
    }

    public float GetBrushSize() {
        return BRUSH_SIZE;
    }

    public Color GetBrushColor() {
        return BRUSH_DRAW_COLOR;
    }

    public bool IsViewToggled() {
        return isViewToggled;
    }
}
