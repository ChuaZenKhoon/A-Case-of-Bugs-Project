using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcetoneKillJar : EvidenceInteractingEquipment {

    [SerializeField] private GameObject megacephalaMaleFlyVisual;
    [SerializeField] private GameObject megacephalaFemaleFlyVisual;
    [SerializeField] private GameObject scalarisFlyVisual;
    [SerializeField] private GameObject ruficornisFlyVisual;

    private List<AdultFly> killedFliesCollected;


    private void Awake() {
        megacephalaMaleFlyVisual.SetActive(false);
        megacephalaFemaleFlyVisual.SetActive(false);
        scalarisFlyVisual.SetActive(false);
        ruficornisFlyVisual.SetActive(false);
    }

    private void Start() {
        killedFliesCollected = EvidenceStorageManager.Instance.GetKillingFlies();
        SetCorrectVisual();
    }

    public override void Interact() {
        MessageLogManager.Instance.LogMessage("I have to put captured flies in here to kill them for pinning later...");
    }


    private void SetCorrectVisual() {
        if (killedFliesCollected.Count > 0) {
            foreach (AdultFly adultFly in killedFliesCollected) {
                string flyType = adultFly.GetInventoryObjectSO().objectName;
                if (flyType == "Green Fly with dark eyes") {
                    megacephalaFemaleFlyVisual.SetActive(true);
                }

                if (flyType == "Green Fly with bright orange eyes") {
                    megacephalaMaleFlyVisual.SetActive(true);
                }

                if (flyType == "Small Brown Fly") {
                    scalarisFlyVisual.SetActive(true);
                }

                if (flyType == "Grey Fly") {
                    ruficornisFlyVisual.SetActive(true);
                }
            }

        }
    }

    public List<AdultFly> GetKilledFlies() {
        return killedFliesCollected;
    }
    
}
