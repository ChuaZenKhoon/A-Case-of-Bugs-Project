using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : EvidenceInteractingEquipment {

    [SerializeField] private GameObject megacephalaFlyVisual;
    [SerializeField] private GameObject spinigeraFlyVisual;

    private List<AdultFly> deadFliesCollected;


    private void Awake() {
        megacephalaFlyVisual.SetActive(false);
        spinigeraFlyVisual.SetActive(false);
    }

    private void Start() {
        deadFliesCollected = EquipmentStorageManager.Instance.GetDeadFlies();
        SetCorrectVisual();
    }

    public override void Interact() {
        
        InventoryObject currentStareAt = Player.Instance.GetStareAt();

        if (currentStareAt is AdultFly) {
            AdultFly currentAdultFlyStaringAt = currentStareAt as AdultFly;

            AdultFly adultFlyToCollect = currentAdultFlyStaringAt.GetInventoryObjectSO().prefab.GetComponent<AdultFly>();
            EquipmentStorageManager.Instance.AddDeadFly(adultFlyToCollect);
            Destroy(currentStareAt.gameObject);

            SetCorrectVisual();


            MessageLogManager.Instance.LogMessage("Adult fly successfully collected!");
        } else {
            MessageLogManager.Instance.LogMessage("I don't think I should put such items in this container.");
        }
    
    }

    private void SetCorrectVisual() {
        if (deadFliesCollected.Count > 0) {
            foreach (AdultFly adultFly in deadFliesCollected) {
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