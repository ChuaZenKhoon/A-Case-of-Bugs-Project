using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SketchDrawSpaceUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    [SerializeField] private SketchDrawSpace drawSpace;

    [SerializeField] private SketchPlanUI planUI;

    [SerializeField] private RawImage sketch;

    [SerializeField] private Button backButton;

    [SerializeField] private CanvasGroup canvasVisibility;
    [SerializeField] private Canvas toggleViewCanvas;

    private Texture2D canvasTexture;
    private RectTransform canvasRectTransform;

    private Color brushColor = Color.black;
    private float brushSize = 4f;

    private Vector2 previousMousePosition;
    private Vector2 resettedMousePosition;

    private Stack<ICommand> drawingHistory;
    private DrawLineCommand currentCommand;

    private bool isViewToggled;
    private void Awake() {

        drawingHistory = new Stack<ICommand>();

        canvasRectTransform = sketch.GetComponent<RectTransform>();

        Texture2D savedSketch = EquipmentStorageManager.Instance.GetSavedSketchPlan();
        InitializeCanvas(savedSketch);

        backButton.onClick.AddListener(() => {
            drawSpace.SaveDetails(canvasTexture);
        });

        isViewToggled = false;
    }
    private void InitializeCanvas(Texture2D savedSketch) {
        int width = (int)canvasRectTransform.rect.width;
        int height = (int)canvasRectTransform.rect.height;
        if (savedSketch != null) {
            canvasTexture = savedSketch;
        } else {
            canvasTexture = new Texture2D(width, height);
            ClearCanvas();
        }

        sketch.texture = canvasTexture;
    }
    private void Start() {
        GameInput.Instance.OnClearSketch += GameInput_OnClearSketch;
        GameInput.Instance.OnSketchMouseMove += GameInput_OnSketchMouseMove;
        GameInput.Instance.OnInteract2Action += GameInput_OnInteract2Action;
        GameInput.Instance.OnToggleSketchView += GameInput_OnToggleSketchView;
        Hide();
    }

    private void GameInput_OnToggleSketchView(object sender, System.EventArgs e) {
        if (gameObject.activeSelf) {
            isViewToggled = !isViewToggled;

            if (isViewToggled) {
                canvasVisibility.alpha = 0f;
                toggleViewCanvas.gameObject.SetActive(true);
            } else {
                canvasVisibility.alpha = 1f;
                toggleViewCanvas.gameObject.SetActive(false);
            }

        }
    }

    private void GameInput_OnInteract2Action(object sender, System.EventArgs e) {
        UndoLastLine();
    }

    private void GameInput_OnSketchMouseMove(object sender, Vector2 e) {
        resettedMousePosition = e;
    }

    private void OnDestroy() {
        GameInput.Instance.OnClearSketch -= GameInput_OnClearSketch;
        GameInput.Instance.OnInteract2Action -= GameInput_OnInteract2Action;
        GameInput.Instance.OnSketchMouseMove -= GameInput_OnSketchMouseMove;
        GameInput.Instance.OnToggleSketchView -= GameInput_OnToggleSketchView;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void GameInput_OnClearSketch(object sender, System.EventArgs e) {
        if (gameObject.activeSelf) {
            ClearCanvas();
        }
    }


    public void OnBeginDrag(PointerEventData eventData) {
        if (isViewToggled) {
            return;
        }
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, null, out previousMousePosition);
        currentCommand = new DrawLineCommand(canvasTexture, brushColor, brushSize);
    }
    public void OnDrag(PointerEventData eventData) {
        if (isViewToggled) {
            return;
        }

        Vector2 currentMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, null, out currentMousePosition);

        DrawToCanvas(previousMousePosition, currentMousePosition);

        previousMousePosition = currentMousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (isViewToggled) {
            return;
        }

        drawingHistory.Push(currentCommand);
        currentCommand = null;
    }

    private void DrawToCanvas(Vector2 start, Vector2 end) {
        int startX = Mathf.RoundToInt((start.x + canvasRectTransform.rect.width * 0.5f) / canvasRectTransform.rect.width * canvasTexture.width);
        int startY = Mathf.RoundToInt((start.y + canvasRectTransform.rect.height * 0.5f) / canvasRectTransform.rect.height * canvasTexture.height);
        int endX = Mathf.RoundToInt((end.x + canvasRectTransform.rect.width * 0.5f) / canvasRectTransform.rect.width * canvasTexture.width);
        int endY = Mathf.RoundToInt((end.y + canvasRectTransform.rect.height * 0.5f) / canvasRectTransform.rect.height * canvasTexture.height);

        currentCommand.AddLineSegment(startX, startY, endX, endY);
        currentCommand.ExecuteBatch();
    }

    public void UndoLastLine() {
        if (drawingHistory.Count > 0 && !isViewToggled) {
            ICommand lastCommand = drawingHistory.Pop();
            lastCommand.Undo();
        }
    }

    private void ClearCanvas() {
        if (isViewToggled) {
            return;
        }
        
        canvasTexture.SetPixels32(new Color32[canvasTexture.width * canvasTexture.height]);
        canvasTexture.Apply();
        drawingHistory.Clear();
    }



    public interface ICommand {
        public void Execute();
        public void Undo();
    }


    private class DrawLineCommand : ICommand {
        private List<Vector2Int> line;
        private Texture2D canvasTexture;
        private Color brushColor;
        private float brushSize;

        public DrawLineCommand(Texture2D canvasTexture, Color brushColor, float brushSize) {
            line = new List<Vector2Int>();
            this.canvasTexture = canvasTexture;
            this.brushColor = brushColor;
            this.brushSize = brushSize;
        }

        public void AddLineSegment(int startX, int startY, int endX, int endY) {
            int steps = Mathf.Max(Mathf.Abs(endX - startX), Mathf.Abs(endY - startY));
            for (int i = 0; i <= steps; i += Mathf.CeilToInt(brushSize)) {  // Increase step size
                float t = i / (float)steps;
                int x = Mathf.RoundToInt(Mathf.Lerp(startX, endX, t));
                int y = Mathf.RoundToInt(Mathf.Lerp(startY, endY, t));
                line.Add(new Vector2Int(x, y));
            }
        }

        private void DrawCircle(int centerX, int centerY, float radius, Color color) {
            int minX = Mathf.Max(0, centerX - Mathf.CeilToInt(radius));
            int maxX = Mathf.Min(canvasTexture.width - 1, centerX + Mathf.CeilToInt(radius));
            int minY = Mathf.Max(0, centerY - Mathf.CeilToInt(radius));
            int maxY = Mathf.Min(canvasTexture.height - 1, centerY + Mathf.CeilToInt(radius));

            for (int x = minX; x <= maxX; x++) {
                for (int y = minY; y <= maxY; y++) {
                    float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                    if (distance <= radius) {
                        canvasTexture.SetPixel(x, y, color);
                    }
                }
            }
        }

        public void Execute() {
            foreach (Vector2Int pixel in line) {
                DrawCircle(pixel.x, pixel.y, brushSize, brushColor);
            }
            canvasTexture.Apply();
        }

        public void Undo() {
            foreach (Vector2Int pixel in line) {
                DrawCircle(pixel.x, pixel.y, brushSize, Color.clear);
            }
            canvasTexture.Apply();
        }

        public void ExecuteBatch() {
            Color[] pixels = canvasTexture.GetPixels();
            foreach (Vector2Int pixel in line) {
                DrawCircleBatch(pixel.x, pixel.y, brushSize, brushColor, pixels);
            }
            canvasTexture.SetPixels(pixels);
            canvasTexture.Apply();
        }

        private void DrawCircleBatch(int centerX, int centerY, float radius, Color color, Color[] pixels) {
            int minX = Mathf.Max(0, centerX - Mathf.CeilToInt(radius));
            int maxX = Mathf.Min(canvasTexture.width - 1, centerX + Mathf.CeilToInt(radius));
            int minY = Mathf.Max(0, centerY - Mathf.CeilToInt(radius));
            int maxY = Mathf.Min(canvasTexture.height - 1, centerY + Mathf.CeilToInt(radius));

            for (int x = minX; x <= maxX; x++) {
                for (int y = minY; y <= maxY; y++) {
                    float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                    if (distance <= radius) {
                        pixels[y * canvasTexture.width + x] = color;
                    }
                }
            }
        }
    }

}
