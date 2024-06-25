using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A class representing a piece of evidence that can be collected.
 */
public class Evidence : InventoryObject {

    [SerializeField] private GameObject sealedEvidence;
    [SerializeField] private GameObject evidenceVisual;
    [SerializeField] private BoxCollider sealedCollider;
    [SerializeField] private BoxCollider evidenceCollider;

    public override void Interact() {
        //Add to inventory, then delete this object instance in the world
        if (InventoryManager.Instance.HasSpaceInInventory()) {
            InventoryManager.Instance.AddToInventory(this.GetInventoryObjectSO());
            Destroy(this.gameObject);
        }
    }
    
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
