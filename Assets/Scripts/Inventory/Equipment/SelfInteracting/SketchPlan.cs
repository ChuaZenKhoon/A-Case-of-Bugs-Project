using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SketchPlan : SelfInteractingEquipment {

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;

    new public static void ResetStaticData() {
        OnChangeInteractActionDetails = null;
    }

    [SerializeField] private SketchPlanUI sketchPlanUI;

    public override void Interact() {
        if (InventoryManager.Instance.IsInventoryOpen()) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before opening sketch plan.");
            return;
        }

        if (SketchPlanUI.isInSketchMode) {
            MessageLogManager.Instance.LogMessage("Close sketch screen first before keeping sketch plan.");
            return;
        }

        if (!SketchPlanUI.IsShown()) {
            Equipment.isInAction = true;
            sketchPlanUI.Show();
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Close Sketch Plan", 0);
            }

            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        } else {
            Equipment.isInAction = false;
            sketchPlanUI.Hide();
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Open Sketch Plan", 0);
            }

            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        }
    }
}
