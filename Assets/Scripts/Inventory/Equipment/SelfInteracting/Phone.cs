using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : SelfInteractingEquipment {

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;


    [SerializeField] private PhoneUI phoneUI;

    public static event EventHandler OnPhoneOpen;

    private EquipmentStorageManager.WeatherRecord weatherRecord;

    new public static void ResetStaticData() {
        OnPhoneOpen = null;
        OnChangeInteractActionDetails = null;
    }

    private void Start() {

        if (Loader.targetScene == Loader.Scene.CrimeScene) {
            weatherRecord = EquipmentStorageManager.Instance.GetWeatherRecord(1);
        } else {
            weatherRecord = EquipmentStorageManager.Instance.GetWeatherRecord(0);
        }
        phoneUI.UpdateText(weatherRecord);
    }

    public override void Interact() {
        if (InventoryManager.Instance.IsInventoryOpen()) {
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