using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * A component of the Sketch Plan that handles its sound effects.
 */
public class SketchPlanSFX : SFX {

    [SerializeField] private SketchDrawSpace drawSpace;
    [SerializeField] private DrawingTool drawTool;

    [SerializeField] private Button drawSpaceButton;
    [SerializeField] private Button detailsButton;
    [SerializeField] private Button legendButton;

    private Coroutine drawCoroutine;

    private void Awake() {
        volMultiplier = 1f;
        drawCoroutine = null;
        drawSpaceButton.onClick.AddListener(() => {
            PlayPaperSound();
        });
        detailsButton.onClick.AddListener(() => {
            PlayPaperSound();
        });
        legendButton.onClick.AddListener(() => {
            PlayPaperSound();
        });
    }

    private void Start() {
        drawSpace.OnClearSketchDone += DrawSpace_OnClearSketchDone;
        drawSpace.OnSketchViewToggled += DrawSpace_OnSketchViewToggled;
        drawSpace.OnUndoLineDone += DrawSpace_OnUndoLineDone;
        drawTool.OnDrawing += DrawTool_OnDrawing;
    }

    private void DrawTool_OnDrawing(object sender, System.EventArgs e) {
        if (drawCoroutine != null) {
            return;
        }
        drawCoroutine = StartCoroutine(DrawingCoroutine());          
    }

    private void DrawSpace_OnUndoLineDone(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlaySketchPlanUndoLineSound(volMultiplier);
    }

    private void DrawSpace_OnSketchViewToggled(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlaySketchPlanToggleViewSound(volMultiplier);
    }

    private void DrawSpace_OnClearSketchDone(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlaySketchClearSound(volMultiplier);
    }

    private void PlayPaperSound() {
        GameElementSFXPlayer.Instance.PlaySketchPlanSelectSectionSound(volMultiplier);
    }

    private IEnumerator DrawingCoroutine() {
        GameElementSFXPlayer.Instance.PlaySketchDrawingSound(volMultiplier * 3f);
        yield return new WaitForSeconds(1f);
        drawCoroutine = null;
    }
}
