using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasuringTool : SelfInteractingEquipment {

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;

    public event EventHandler<float> OnMeasuringDistanceChange;

    [SerializeField] GameObject marker;
    [SerializeField] LineRenderer measuringLine;

    private const float distanceMultiplier = 0.8f;

    private bool isInMeasuringMode;

    private Vector3 startPosition;
    private Vector3 currentPosition;

    new public static void ResetStaticData() {
        OnChangeInteractActionDetails = null;
    }

    private void Awake() {
        isInMeasuringMode = false;
        marker.transform.position = this.transform.position;
        marker.SetActive(false);
    }

    public override void Interact() {
        if (InventoryScreenUI.isInAction) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before using measuring tool.");
            return;
        }

        if (!isInMeasuringMode) {
            Equipment.isInAction = true;
            MessageLogManager.Instance.LogMessage("Starting measurement...");
            StartMeasuring();
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Stop Measuring", 0);
            }

            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        } else {
            Equipment.isInAction = false;
            MessageLogManager.Instance.LogMessage("Measurement stopped.");
            StopMeasuring();

            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Start Measuring", 0);
            }

            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        }
    }

    private void StartMeasuring() {
        isInMeasuringMode = true;
        startPosition = Player.Instance.transform.position;
        SetUpMarker();
    }

    private void SetUpMarker() {
        marker.SetActive(true);
        marker.transform.SetParent(null, true);
        marker.transform.position = startPosition + new Vector3(0f, 1.5f, 0f);
        marker.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        measuringLine.positionCount = 2;
        measuringLine.SetPosition(0, marker.transform.position);
        measuringLine.SetPosition(1, marker.transform.position);
    }
    private void StopMeasuring() {
        isInMeasuringMode = false;
        startPosition = Vector3.zero;
        ResetMarker();
    }

    private void ResetMarker() {
        measuringLine.SetPosition(0, currentPosition);
        marker.transform.SetParent(this.transform, false);
        marker.transform.position = this.transform.position;
        marker.SetActive(false);
    }

    private void Update() {
        if (isInMeasuringMode) {
            currentPosition = Player.Instance.transform.position;
            measuringLine.SetPosition(1, currentPosition);
            float updatedDistance = GetMeasuringDistance();
            OnMeasuringDistanceChange?.Invoke(this, updatedDistance);
        }
    }

    public float GetMeasuringDistance() {
        return (currentPosition - startPosition).magnitude * distanceMultiplier;
    }
}