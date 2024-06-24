using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItemsManager : MonoBehaviour {

    public static TutorialItemsManager Instance { get; private set; }

    [SerializeField] private List<InventoryObjectSO> CanItems;
    [SerializeField] private List<InventoryObjectSO> selfInteractingItems;
    [SerializeField] private List<InventoryObjectSO> evidenceInteractingItems;



    private void Awake() {
        Instance = this;
    }

    private void Start() {
        TutorialLevelManager.Instance.OnStateChange += TutorialLevelManager_OnStateChange;
    }

    private void TutorialLevelManager_OnStateChange(object sender, System.EventArgs e) {
        if (TutorialLevelManager.Instance.IsStartingInventory()) {
            AddToInventory(CanItems);
        }

        if (TutorialLevelManager.Instance.IsStartingSelfInteracting()) {
            AddToInventory(selfInteractingItems);
        }

        if (TutorialLevelManager.Instance.IsStartingEvidenceInteracting()) {
            AddToInventory(evidenceInteractingItems);
        }
    }

    private void AddToInventory(List<InventoryObjectSO> items) {
        foreach (InventoryObjectSO item in items) {
            InventoryManager.Instance.AddToInventory(item);
        }
    }
}
