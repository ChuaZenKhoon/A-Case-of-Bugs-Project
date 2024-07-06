using System;
using UnityEngine;

/**
 * The class representing the measuring tool equipment, which allows the player
 * to measure distances.
 */
public class MeasuringTool : SelfInteractingEquipment {

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;

    public event EventHandler<float> OnMeasuringDistanceChange;

    private static Vector3 OFFSET = new Vector3(0f, 1.3f, 0f);

    private const float distanceMultiplier = 0.75f;


    [SerializeField] GameObject marker;
    [SerializeField] LineRenderer measuringLine;

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
        if (InventoryManager.Instance.IsInventoryOpen()) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before using measuring tool.");
            return;
        }

        if (!isInMeasuringMode) {
            StartMeasuring();
        } else {       
            StopMeasuring();
        }
    }

    /**
     * Activates the equipment, starting tracking of distance from start point to player.
     */
    private void StartMeasuring() {
        Equipment.isInAction = true;
        isInMeasuringMode = true;
        MessageLogManager.Instance.LogMessage("Starting measurement...");

        //Update equipment use text
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Stop Measuring", 0);
        }

        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);


        startPosition = Player.Instance.transform.position;
        SetUpMarker();
    }

    /**
     * Display marker at eye level and anchor tape line points.
     */
    private void SetUpMarker() {
        marker.SetActive(true);
        marker.transform.SetParent(null, true);
        marker.transform.position = startPosition + OFFSET;
        marker.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        measuringLine.positionCount = 2;
        measuringLine.SetPosition(0, marker.transform.position);
        measuringLine.SetPosition(1, marker.transform.position);
    }
    private void StopMeasuring() {
        Equipment.isInAction = false;
        isInMeasuringMode = false;
        MessageLogManager.Instance.LogMessage("Measurement stopped.");

        //Update equipment use text
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Start Measuring", 0);
        }

        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);

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