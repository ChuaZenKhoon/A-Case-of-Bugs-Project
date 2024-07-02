using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swab : EvidenceInteractingEquipment {

    [SerializeField] private GameObject usedVisual;
    [SerializeField] private GameObject positiveResultVisual;
    [SerializeField] private GameObject cannotBeUsedVisual;
    [SerializeField] private GameObject canBeUsedVisual;

    private Bloodstain bloodStain;

    private void Awake() {
        usedVisual.SetActive(false);
        positiveResultVisual.SetActive(false);
        cannotBeUsedVisual.SetActive(false);
    }

    private void Start() {
        bloodStain = EquipmentStorageManager.Instance.GetBloodStain(this.GetEquipmentID(), out bool positive, out bool cannotBeUsed);

        if (cannotBeUsed) {
            cannotBeUsedVisual.SetActive(true);
            canBeUsedVisual.SetActive(false);
            return;
        }

        if (bloodStain != null) {
            cannotBeUsedVisual.SetActive(false);
            usedVisual.SetActive(true);
        }

        if (positive) {
            positiveResultVisual.SetActive(true);
        }
    }


    public override void Interact() {
        if (bloodStain == null) {
            InteractableObject currentStareAt = Player.Instance.GetStareAt();

            if (currentStareAt is Bloodstain) {
                Bloodstain currentBloodStainStaringAt = currentStareAt as Bloodstain;

                Bloodstain liquidToSwab = currentBloodStainStaringAt.GetInventoryObjectSO().prefab.GetComponentInChildren<Bloodstain>();
                bloodStain = liquidToSwab;
                EquipmentStorageManager.Instance.SetBloodStain(this.GetEquipmentID(), bloodStain, false, false);

                usedVisual.SetActive(true);


                MessageLogManager.Instance.LogMessage("Red stain successfully collected!");
            } else {
                MessageLogManager.Instance.LogMessage("Cannot pick up normal items with the swab.");
            }
        } else {
            MessageLogManager.Instance.LogMessage("This swab has already been used.");
        }
    }

    public void PositiveTestAdministered() {
        EquipmentStorageManager.Instance.SetBloodStain(this.GetEquipmentID(), bloodStain, true, false);
        positiveResultVisual.SetActive(true);
        MessageLogManager.Instance.LogMessage("Cotton swab turns pink! Positive test result obtained. This sample could be human or animal blood!");
    }

    public void ImproperTestAdministered() {
        EquipmentStorageManager.Instance.SetBloodStain(this.GetEquipmentID(), null, false, true);
        cannotBeUsedVisual.SetActive(true);
        canBeUsedVisual.SetActive(false);
        MessageLogManager.Instance.LogMessage("Improper test administered. Swab is stored and to be thrown later.");
    }

    public bool IsUsed() {
        return bloodStain != null;
    }

    public bool IsPositiveTestedAlready() {
        return positiveResultVisual.activeSelf;
    }

    public bool CannotBeUsed() {
        return cannotBeUsedVisual.activeSelf;
    }
}
