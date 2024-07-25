using UnityEngine;

/**
 * A component of the measuring tool that handles its sound effects.
 */
public class MeasuringToolSFX : SFX {

    [SerializeField] private MeasuringTool measuringTool;

    private void Awake() {
        volMultiplier = 3f;
    }

    private void Start() {
        measuringTool.OnMeasuringStart += MeasuringTool_OnMeasuringStart;
        measuringTool.OnMeasuringStop += MeasuringTool_OnMeasuringStop;
    }

    private void MeasuringTool_OnMeasuringStop(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayMeasuringToolStopSound(volMultiplier);
    }

    private void MeasuringTool_OnMeasuringStart(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayMeasuringToolStartSound(volMultiplier);
    }
}
