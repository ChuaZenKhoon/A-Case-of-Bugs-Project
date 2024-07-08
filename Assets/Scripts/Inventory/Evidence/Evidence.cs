using UnityEngine;

/**
 * A class representing a piece of evidence that can be collected.
 */
public class Evidence : InventoryObject {

    [SerializeField] private GameObject sealedEvidence;
    [SerializeField] private BoxCollider sealedCollider;

    [SerializeField] private GameObject evidenceVisual;
    [SerializeField] private BoxCollider evidenceCollider;

    public override void Interact() {
        //Add to inventory, then delete this object instance in the world
        if (InventoryManager.Instance.HasSpaceInInventory()) {
            InventoryManager.Instance.AddToInventory(this.GetInventoryObjectSO());
            Destroy(this.gameObject);
        }
    }
    
    //When evidence is picked up, it is put in a sealed bag. Adjust dropped evidence into sealed bag visual.
    public void SealEvidence() {
        if (sealedEvidence == null) {
            return;
        }
        evidenceVisual.SetActive(false);
        evidenceCollider.enabled = false;
        sealedEvidence.SetActive(true);
        sealedCollider.enabled = true;
    }

    public bool IsSealed() {
        if (sealedEvidence == null) {
            return false; 
        }

        return sealedEvidence.activeSelf;
    }
}
