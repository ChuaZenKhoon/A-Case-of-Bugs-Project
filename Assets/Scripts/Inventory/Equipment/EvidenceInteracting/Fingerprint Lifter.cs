using UnityEngine;

/**
 * The class representing the fingerprint lifter equipment.
 */
public class FingerprintLifter : EvidenceInteractingEquipment {

    [SerializeField] private GameObject fingerprintLiftedVisual;

    private Fingerprint fingerprintLifted;

    private void Awake() {
        fingerprintLiftedVisual.SetActive(false);
    }

    private void Start() {
        fingerprintLifted = EvidenceStorageManager.Instance.GetFingerprint(this.GetEquipmentID());

        if (fingerprintLifted != null ) {
            fingerprintLiftedVisual.SetActive(true);
        }
    }

    public override void Interact() {
        if (fingerprintLifted == null) {
            InteractableObject currentStareAt = Player.Instance.GetStareAt();

            if (currentStareAt is Fingerprint) {
                LiftFingerprint(currentStareAt);
                MessageLogManager.Instance.LogMessage("Fingerprint successfully lifted!");
            } else {
                //Not fingerprint
                MessageLogManager.Instance.LogMessage("Cannot pick up normal items with the fingerprint lifter.");
            }
        } else {
            //Already have fingerprint
            MessageLogManager.Instance.LogMessage("There is already a fingerprint on this lifter.");
        }
    }
    
    /**
     * Transfer fingerprint to storage, update visual
     */
    private void LiftFingerprint(InteractableObject currentStareAt) {
        Fingerprint currentFingerprintStaringAt = currentStareAt as Fingerprint;

        //Have to find prefab fingerprint component as this component will be destroyed with gameobject
        Fingerprint fingerprintToLift = currentFingerprintStaringAt.GetInventoryObjectSO().prefab.GetComponent<Fingerprint>();
        fingerprintLifted = fingerprintToLift;

        EvidenceStorageManager.Instance.SetFingerprint(this.GetEquipmentID(), fingerprintLifted);
        
        Destroy(currentStareAt.gameObject);

        fingerprintLiftedVisual.SetActive(true);
    }
}
