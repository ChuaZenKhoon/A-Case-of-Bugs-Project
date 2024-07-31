using System;
using UnityEngine;

/**
 * The class representing the phone equipment.
 */
public class Phone : SelfInteractingEquipment {

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;
    public static event EventHandler OnPhoneOpen;

    private static int TUTORIAL_LEVEL_WEATHER_INDEX = 0;
    private static int GAME_LEVEL_WEATHER_INDEX = 1;


    [SerializeField] private PhoneUI phoneUI;

    private EquipmentStorageManager.WeatherRecord weatherRecord;

    new public static void ResetStaticData() {
        OnPhoneOpen = null;
        OnChangeInteractActionDetails = null;
    }

    private void Start() {
        if (Loader.targetScene == Loader.Scene.CrimeScene) {
            weatherRecord = EquipmentStorageManager.Instance.GetWeatherRecord(GAME_LEVEL_WEATHER_INDEX);
        } else {
            weatherRecord = EquipmentStorageManager.Instance.GetWeatherRecord(TUTORIAL_LEVEL_WEATHER_INDEX);
        }
        phoneUI.UpdateText(weatherRecord);
    }

    public override void Interact() {
        if (InventoryManager.Instance.IsInventoryOpen()) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before using phone.");
            return;
        }

        if (!phoneUI.IsShown()) {
            OpenPhone();
        } else {
            ClosePhone();
        }
    }

    private void OpenPhone() {
        Equipment.IS_IN_ACTION = true;
        phoneUI.Show();
        OnPhoneOpen?.Invoke(this, EventArgs.Empty);
        
        //Update interaction text
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Close Phone", 0);
        }

        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
    }

    private void ClosePhone() {
        Equipment.IS_IN_ACTION = false;
        phoneUI.Hide();
        OnPhoneOpen?.Invoke(this, EventArgs.Empty);

        //Update interaction text
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Check Local Weather", 0);
        }
        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
    }

    /**
     * Reset interaction details if restart level in the middle of interaction
     * that creates change in interaction details
     */
    private void OnDestroy() {
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Check Local Weather", 0);
        }
    }
}