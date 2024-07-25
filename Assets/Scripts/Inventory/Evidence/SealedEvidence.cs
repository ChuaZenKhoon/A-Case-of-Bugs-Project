using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealedEvidence : Evidence {

    public event EventHandler OnPickUp;

    [SerializeField] private GameObject sealedVisual;
    [SerializeField] private BoxCollider sealedCollider;

    [SerializeField] private GameObject evidenceVisual;
    [SerializeField] private BoxCollider evidenceCollider;

    public override void Interact() {
        //Add to inventory, then delete this object instance in the world
        if (InventoryManager.Instance.HasSpaceInInventory()) {
            InventoryManager.Instance.AddToInventory(this.GetInventoryObjectSO());
            OnPickUp?.Invoke(this, EventArgs.Empty);
            Destroy(this.gameObject);
        }
    }



    //When evidence is picked up, it is put in a sealed bag. Adjust dropped evidence into sealed bag visual.
    public void SealEvidence() {
        evidenceVisual.SetActive(false);
        evidenceCollider.enabled = false;
        sealedVisual.SetActive(true);
        sealedCollider.enabled = true;
    }

    public bool IsSealed() {
        return sealedVisual.activeSelf;
    }

}
