using System.Collections.Generic;
using UnityEngine;

/**
 * The class representing the acetone kill jar equipment that stores captured adult flies.
 */
public class AcetoneKillJar : EvidenceInteractingEquipment {

    [SerializeField] private GameObject megacephalaMaleFlyVisual;
    [SerializeField] private GameObject megacephalaFemaleFlyVisual;
    [SerializeField] private GameObject scalarisFlyVisual;
    [SerializeField] private GameObject ruficornisFlyVisual;

    private List<AdultFly> killedFliesCollected;

    private void Awake() {
        SetCorrectVisual();
    }

    private void Start() {
        killedFliesCollected = EvidenceStorageManager.Instance.GetFliesToKill();
        SetCorrectVisual();
    }

    public override void Interact() {
        if (killedFliesCollected.Count == 0) {
            MessageLogManager.Instance.LogMessage("I have to put captured flies in here to kill them for pinning later...");
        } else {
            MessageLogManager.Instance.LogMessage("I need to examine the flies under the microscope in the laboratory.");
        }
    }


    private void SetCorrectVisual() {
        if (killedFliesCollected == null || killedFliesCollected.Count <= 0) {
            megacephalaMaleFlyVisual.SetActive(false);
            megacephalaFemaleFlyVisual.SetActive(false);
            scalarisFlyVisual.SetActive(false);
            ruficornisFlyVisual.SetActive(false);
            return;
        }

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

    public List<AdultFly> GetKilledFlies() {
        return killedFliesCollected;
    }
    
}
