using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcetoneKillJar : EvidenceInteractingEquipment {

    [SerializeField] private GameObject megacephalaFlyVisual;
    [SerializeField] private GameObject spinigeraFlyVisual;

    private List<AdultFly> killedFliesCollected;


    private void Awake() {
        megacephalaFlyVisual.SetActive(false);
        spinigeraFlyVisual.SetActive(false);
    }

    private void Start() {
        killedFliesCollected = EquipmentStorageManager.Instance.GetKillingFlies();
        SetCorrectVisual();
    }

    public override void Interact() {
        MessageLogManager.Instance.LogMessage("I have to put captured flies in here to kill them for pinning later...");
    }


    private void SetCorrectVisual() {
        if (killedFliesCollected.Count > 0) {
            foreach (AdultFly adultFly in killedFliesCollected) {
                string flyType = adultFly.GetInventoryObjectSO().objectName;
                if (flyType == "Black Fly") {
                    spinigeraFlyVisual.SetActive(true);
                }

                if (flyType == "Green Fly") {
                    megacephalaFlyVisual.SetActive(true);
                }
            }

        }
    }
    
}
