using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * The class representing the container equipment for collecting dead adult flies. 
 */
public class Container : EvidenceInteractingEquipment {

    public event EventHandler OnCollectFly;

    [SerializeField] private GameObject megacephalaMaleFlyVisual;
    [SerializeField] private GameObject megacephalaFemaleFlyVisual;
    [SerializeField] private GameObject scalarisFlyVisual;
    [SerializeField] private GameObject ruficornisFlyVisual;

    private List<AdultFly> deadFliesCollected;

    private void Awake() {
        SetCorrectVisual();
    }

    private void Start() {
        deadFliesCollected = EvidenceStorageManager.Instance.GetDeadFlies();
        SetCorrectVisual();
    }

    public override void Interact() {
        
        InteractableObject currentStareAt = Player.Instance.GetStareAt();

        if (currentStareAt is AdultFly) {
            CollectDeadFly(currentStareAt);
            MessageLogManager.Instance.LogMessage("Adult fly successfully collected!");
        } else {
            MessageLogManager.Instance.LogMessage("I don't think I should put such items in this container.");
        }
    
    }

    private void CollectDeadFly(InteractableObject currentStareAt) {
        AdultFly currentAdultFlyStaringAt = currentStareAt as AdultFly;

        AdultFly adultFlyToCollect = currentAdultFlyStaringAt.GetInventoryObjectSO().prefab.GetComponent<AdultFly>();
        EvidenceStorageManager.Instance.AddDeadFly(adultFlyToCollect);
        Destroy(currentStareAt.gameObject);

        SetCorrectVisual();
        OnCollectFly?.Invoke(this, EventArgs.Empty);
    }

    private void SetCorrectVisual() {
        if (deadFliesCollected == null || deadFliesCollected.Count <= 0) {
            megacephalaMaleFlyVisual.SetActive(false);
            megacephalaFemaleFlyVisual.SetActive(false);
            scalarisFlyVisual.SetActive(false);
            ruficornisFlyVisual.SetActive(false);
            return;
        }
 
        foreach (AdultFly adultFly in deadFliesCollected) {
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

    public List<AdultFly> GetDeadFlies() {
        return deadFliesCollected;
    }
}