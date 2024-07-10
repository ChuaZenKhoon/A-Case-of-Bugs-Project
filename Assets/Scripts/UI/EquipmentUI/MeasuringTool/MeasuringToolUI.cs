using TMPro;
using UnityEngine;

/**
 * A UI component representing the measurement distance displayed to the player
 * as the player uses the measuring tool equipment.
 */
public class MeasuringToolUI : MonoBehaviour {

    [SerializeField] private MeasuringTool measuringTool;

    [SerializeField] private TextMeshProUGUI distanceText;

    private void Start() {
        measuringTool.OnMeasuringDistanceChange += MeasuringTool_OnMeasuringDistanceChange;
    }

    private void MeasuringTool_OnMeasuringDistanceChange(object sender, float e) {
        distanceText.text = e.ToString("F2") + "m";
    }

    private void OnDestroy() {
        measuringTool.OnMeasuringDistanceChange -= MeasuringTool_OnMeasuringDistanceChange;
    }
}
