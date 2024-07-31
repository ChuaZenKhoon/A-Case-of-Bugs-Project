using System;
using UnityEngine;

/**
 * The class representing the measuring tool equipment, which allows the player
 * to measure distances.
 */
public class MeasuringTool : SelfInteractingEquipment {

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;
    new public static void ResetStaticData() {
        OnChangeInteractActionDetails = null;
    }

    public event EventHandler<float> OnMeasuringDistanceChange;
    public event EventHandler OnMeasuringStart;
    public event EventHandler OnMeasuringStop;

    private static Vector3 OFFSET = new Vector3(0f, 1.3f, 0f);

    private const float distanceMultiplier = 0.75f;


    [SerializeField] GameObject marker;
    [SerializeField] LineRenderer measuringLine;

    private bool isInMeasuringMode;


    private Vector3 startPosition;
    private Vector3 currentPosition;

    private Transform holdPosition;
    private Transform measuringToolAtStomachPosition;

    private void Awake() {
        isInMeasuringMode = false;
        marker.transform.position = this.transform.position;
        marker.SetActive(false);
        holdPosition = Player.Instance.GetHoldPosition();
        measuringToolAtStomachPosition = Player.Instance.GetMeasuringToolUsePosition();
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
        Equipment.IS_IN_ACTION = true;
        isInMeasuringMode = true;
        MessageLogManager.Instance.LogMessage("Starting measurement...");

        //Update equipment use text
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Stop Measuring", 0);
        }

        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        OnMeasuringStart?.Invoke(this, EventArgs.Empty);

        startPosition = measuringToolAtStomachPosition.position;
        this.gameObject.transform.SetParent(measuringToolAtStomachPosition, false);
        this.gameObject.transform.localPosition = Vector3.zero;

        SetUpMarker();
    }

    /**
     * Display marker at eye level and anchor tape line points.
     */
    private void SetUpMarker() {
        marker.SetActive(true);
        marker.transform.SetParent(null, true);
        marker.transform.position = startPosition;
        marker.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        measuringLine.positionCount = 2;
        measuringLine.SetPosition(0, marker.transform.position);
        measuringLine.SetPosition(1, marker.transform.position);
    }
    private void Update() {
        if (isInMeasuringMode) {
            currentPosition = this.gameObject.transform.position;
            measuringLine.SetPosition(1, currentPosition);
            float updatedDistance = GetMeasuringDistance();
            OnMeasuringDistanceChange?.Invoke(this, updatedDistance);
        }
    }

    private void StopMeasuring() {
        Equipment.IS_IN_ACTION = false;
        isInMeasuringMode = false;
        MessageLogManager.Instance.LogMessage("Measurement stopped.");

        //Update equipment use text
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Start Measuring", 0);
        }

        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        OnMeasuringStop?.Invoke(this, EventArgs.Empty);

        startPosition = Vector3.zero;
        this.gameObject.transform.SetParent(holdPosition, false);
        this.gameObject.transform.localPosition = Vector3.zero;
        ResetMarker();
    }

    private void ResetMarker() {
        measuringLine.SetPosition(0, currentPosition);
        marker.transform.SetParent(this.transform, false);
        marker.transform.position = this.transform.position;
        marker.SetActive(false);
    }

    /**
     * Reset interaction details if restart level in the middle of interaction
     * that creates change in interaction details
     */
    private void OnDestroy() {
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Start Measuring", 0);
        }
    }

    public float GetMeasuringDistance() {
        return (currentPosition - startPosition).magnitude * distanceMultiplier;
    }
}