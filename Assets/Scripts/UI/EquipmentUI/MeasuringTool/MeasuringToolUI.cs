using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
