using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SketchPlan : SelfInteractingEquipment {

    [SerializeField] private SketchPlanUI sketchPlanUI;

    public override void Interact() {
        if (InventoryScreenUI.isInAction) {
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
        } else {
            Equipment.isInAction = false;
            sketchPlanUI.Hide();
        }
    }
}
