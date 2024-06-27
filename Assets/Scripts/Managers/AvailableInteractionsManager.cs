using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableInteractionsManager : MonoBehaviour {

    public static AvailableInteractionsManager Instance { get; private set; }

    public event EventHandler<EquipmentSO> OnEquipmentHold;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Player.Instance.OnUpdateHeldItemToEquipment += Player_OnUpdateHeldItemToEquipment;
        HotWaterCup.OnChangeInteractActionDetails += HotWaterCup_OnChangeInteractActionDetails;
        FlyNet.OnChangeInteractActionDetails += FlyNet_OnChangeInteractActionDetails;
        SketchPlan.OnChangeInteractActionDetails += SketchPlan_OnChangeInteractActionDetails;
        MeasuringTool.OnChangeInteractActionDetails += MeasuringTool_OnChangeInteractActionDetails;
        Phone.OnChangeInteractActionDetails += Phone_OnChangeInteractActionDetails;
    }

    private void SketchPlan_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        OnEquipmentHold?.Invoke(this, e);
    }

    private void Phone_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        OnEquipmentHold?.Invoke(this, e);
    }

    private void MeasuringTool_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        OnEquipmentHold?.Invoke(this, e);
    }

    private void FlyNet_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        OnEquipmentHold?.Invoke(this, e);
    }

    private void HotWaterCup_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        OnEquipmentHold?.Invoke(this, e);
    }

    private void Player_OnUpdateHeldItemToEquipment(object sender, EquipmentSO e) {
        OnEquipmentHold?.Invoke(this, e);
    }

}
