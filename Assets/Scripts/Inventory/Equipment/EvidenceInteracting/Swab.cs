using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swab : EvidenceInteractingEquipment {

    [SerializeField] private GameObject usedVisual;

    private Bloodstain bloodStain;

    private void Awake() {
        usedVisual.SetActive(false);
    }

    private void Start() {
        bloodStain = EquipmentStorageManager.Instance.GetBloodStain(this.GetEquipmentID());

        if (bloodStain != null) {
            usedVisual.SetActive(true);
        }
    }


    public override void Interact() {
        if (bloodStain == null) {
            InteractableObject currentStareAt = Player.Instance.GetStareAt();

            if (currentStareAt is Bloodstain) {
                Bloodstain currentBloodStainStaringAt = currentStareAt as Bloodstain;

                Bloodstain liquidToSwab = currentBloodStainStaringAt.GetInventoryObjectSO().prefab.GetComponentInChildren<Bloodstain>();
                bloodStain = liquidToSwab;
                EquipmentStorageManager.Instance.SetBloodStain(this.GetEquipmentID(), bloodStain);

                usedVisual.SetActive(true);


                MessageLogManager.Instance.LogMessage("Red stain successfully collected!");
            } else {
                MessageLogManager.Instance.LogMessage("Cannot pick up normal items with the swab.");
            }
        } else {
            MessageLogManager.Instance.LogMessage("This swab has already been used.");
        }
    }

}
