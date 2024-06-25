using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : SelfInteractingEquipment {

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;

    [SerializeField] private PhoneUI phoneUI;

    public static event EventHandler OnPhoneOpen;

    new public static void ResetStaticData() {
        OnPhoneOpen = null;
        OnChangeInteractActionDetails = null;
    }
    public override void Interact() {
        if (InventoryScreenUI.isInAction) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before using phone.");
            return;
        }

        if (!phoneUI.IsShown()) {
            Equipment.isInAction = true;
            phoneUI.Show();
            OnPhoneOpen?.Invoke(this, EventArgs.Empty);
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Close Phone", 0);
            }

            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        } else {
            Equipment.isInAction = false;
            phoneUI.Hide();
            OnPhoneOpen?.Invoke(this, EventArgs.Empty);
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Check Local Weather", 0);
            }
            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        }
    }



}