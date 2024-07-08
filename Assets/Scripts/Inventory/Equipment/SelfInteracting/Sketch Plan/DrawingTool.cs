using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * A logic component that handles the drawing logic in the drawing section of the sketch plan.
 */
public class DrawingTool : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    [SerializeField] private SketchDrawSpace drawSpace;
    [SerializeField] private SketchDrawSpaceUI parentUI;
    [SerializeField] private RawImage sketch;
    
    private RectTransform canvasRectTransform;
    private Texture2D canvasTexture;

    private Vector2 previousMousePosition;

    private Stack<ICommand> drawingHistory;
    private DrawLineCommand currentCommand;

    private void Awake() {
        //For undo
        drawingHistory = new Stack<ICommand>();

        canvasRectTransform = sketch.GetComponent<RectTransform>();

        Texture2D savedSketch = EquipmentStorageManager.Instance.GetSavedSketchPlan();
        InitializeCanvas(savedSketch);
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

    /**
     * Undoes the latest line drawn by the player.
     * Upon opening an old sketch drawing, cannot undo any line.
     * Cannot be done when drawing is not open
     */
    public void UndoLastLine() {
        if (drawingHistory.Count > 0 && !drawSpace.IsViewToggled()) {
            ICommand lastCommand = drawingHistory.Pop();
            lastCommand.Undo();
        }
    }

    /**
     * Wipes the sketch drawing to a clean state.
     * Cannot be done when drawing is not open
     */
    public void ClearCanvas() {
        if (drawSpace.IsViewToggled()) {
            return;
        }

        canvasTexture.SetPixels32(new Color32[canvasTexture.width * canvasTexture.height]);
        canvasTexture.Apply();
        drawingHistory.Clear();
    }


    public void OnBeginDrag(PointerEventData eventData) {
        if (drawSpace.IsViewToggled()) {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, null, out previousMousePosition);
        currentCommand = new DrawLineCommand(canvasTexture, drawSpace.GetBrushColor(), drawSpace.GetBrushSize());
    }
    public void OnDrag(PointerEventData eventData) {
        if (drawSpace.IsViewToggled()) {
            return;
        }

        Vector2 currentMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, null, out currentMousePosition);

        DrawToCanvas(previousMousePosition, currentMousePosition);

        previousMousePosition = currentMousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (drawSpace.IsViewToggled()) {
            return;
        }

        drawingHistory.Push(currentCommand);
        currentCommand = null;
    }

    /**
     * Draws a line in segments for performance optimisations.
     */
    private void DrawToCanvas(Vector2 start, Vector2 end) {
        int startX = Mathf.RoundToInt((start.x + canvasRectTransform.rect.width * 0.5f) / canvasRectTransform.rect.width * canvasTexture.width);
        int startY = Mathf.RoundToInt((start.y + canvasRectTransform.rect.height * 0.5f) / canvasRectTransform.rect.height * canvasTexture.height);
        int endX = Mathf.RoundToInt((end.x + canvasRectTransform.rect.width * 0.5f) / canvasRectTransform.rect.width * canvasTexture.width);
        int endY = Mathf.RoundToInt((end.y + canvasRectTransform.rect.height * 0.5f) / canvasRectTransform.rect.height * canvasTexture.height);

        currentCommand.AddLineSegment(startX, startY, endX, endY);
        currentCommand.ExecuteBatch();
    }


    public Texture2D GetCanvasTexture() {
        return canvasTexture;
    }


    public interface ICommand {
        public void Execute();
        public void Undo();
    }


    //Command pattern to improve efficiency and allow extension in the future
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
