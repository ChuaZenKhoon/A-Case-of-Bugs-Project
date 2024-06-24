using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerprintLifter : EvidenceInteractingEquipment {

    [SerializeField] private GameObject fingerprintLiftedVisual;

    private Fingerprint fingerprintLifted;

    private void Awake() {
        fingerprintLiftedVisual.SetActive(false);
    }

    private void Start() {
        fingerprintLifted = EquipmentStorageManager.Instance.GetFingerprint(this.GetEquipmentID());

        if (fingerprintLifted != null ) {
            fingerprintLiftedVisual.SetActive(true);
        }
    }

    public override void Interact() {
        if (fingerprintLifted == null) {
            InventoryObject currentStareAt = Player.Instance.GetStareAt();

            if (currentStareAt is Fingerprint) {
                Fingerprint currentFingerprintStaringAt = currentStareAt as Fingerprint;

                Fingerprint fingerprintToLift = currentFingerprintStaringAt.GetInventoryObjectSO().prefab.GetComponent<Fingerprint>();
                fingerprintLifted = fingerprintToLift;
                EquipmentStorageManager.Instance.SetFingerprint(this.GetEquipmentID(), fingerprintLifted);
                Destroy(currentStareAt.gameObject);
                
                fingerprintLiftedVisual.SetActive(true);
                
                
                MessageLogManager.Instance.LogMessage("Fingerprint successfully lifted!");
            } else {
                MessageLogManager.Instance.LogMessage("Cannot pick up normal items with the fingerprint lifter.");
            }
        } else {
            MessageLogManager.Instance.LogMessage("There is already a fingerprint on this lifter.");
        }
    }

    
}
