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
