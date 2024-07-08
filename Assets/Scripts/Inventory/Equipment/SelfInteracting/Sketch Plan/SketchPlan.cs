using System;
using UnityEngine;

/**
 * The class representing the sketch plan equipment, encompasses its components.
 * Note: This equipment has 2 layers of UI screens. Hence, the components are only referenced
 * in the main UI to remove any redundancy of references.
 */
public class SketchPlan : SelfInteractingEquipment {

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;
    public static event EventHandler OnOpenSketchPlan;

    new public static void ResetStaticData() {
        OnChangeInteractActionDetails = null;
        OnOpenSketchPlan = null;
        isInSketchMode = false;
    }

    [SerializeField] private SketchPlanUI sketchPlanUI;

    private bool isSketchPlanShown;
    private static bool isInSketchMode;

    private void Start() {
        isSketchPlanShown = false;
        isInSketchMode = false;
    }

    public override void Interact() {
        if (InventoryManager.Instance.IsInventoryOpen()) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before opening sketch plan.");
            return;
        }

        if (isInSketchMode) {
            return;
        }

        if (!isSketchPlanShown) {
            OpenSketchPlan();
        } else {
            CloseSketchPlan();
        }
    }

    private void OpenSketchPlan() {
        Equipment.isInAction = true;
        isSketchPlanShown = true;
        
        UpdateSketchImages();
        sketchPlanUI.Show();
        
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Close Sketch Plan", 0);
        }

        OnOpenSketchPlan?.Invoke(this, EventArgs.Empty);
        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
    }

    public void CloseSketchPlan() {
        Equipment.isInAction = false;
        isSketchPlanShown = false;
        sketchPlanUI.Hide();

        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Open Sketch Plan", 0);
        }

        OnOpenSketchPlan?.Invoke(this, EventArgs.Empty);
        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
    }

    /**
     * Reset interaction details if restart level in the middle of interaction
     * that creates change in interaction details
     */
    private void OnDestroy() {
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Open Sketch Plan", 0);
        }
    }
    public static bool IsInSketchMode() {
        return isInSketchMode;
    }
    public void SetSketchMode(bool isActive) {
        isInSketchMode = isActive;
    }

    //To show drawing on UI opening
    public void UpdateSketchImages() {
        Sprite savedImage = EquipmentStorageManager.Instance.GetSavedSketchImages();
        sketchPlanUI.UpdateSketchImage(savedImage);
    }
}
