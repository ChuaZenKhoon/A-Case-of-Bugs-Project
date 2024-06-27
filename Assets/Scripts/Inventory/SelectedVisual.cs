using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour {

    [SerializeField] private InventoryObject inventoryObject;
    [SerializeField] private GameObject[] selectedVisualArray;

    private void Start() {
        Player.Instance.OnPlayerStareAtInventoryObjectChange += Player_OnPlayerStareAtInventoryObjectChange;
    }

    private void Player_OnPlayerStareAtInventoryObjectChange(object sender, Player.OnPlayerStareAtInventoryObjectChangeEventArgs e) {
        if (e.inventoryObject == inventoryObject) {
            Show();
        } else {
            Hide();
        }
    }

    //Object stops listening when it is destroyed
    private void OnDestroy() {
        Player.Instance.OnPlayerStareAtInventoryObjectChange -= Player_OnPlayerStareAtInventoryObjectChange;
    }

    private void Show() {
        foreach (GameObject selectedCounterVisual in selectedVisualArray) {
            selectedCounterVisual.SetActive(true);
        }
        
        if (inventoryObject is Evidence) {
            Evidence evidence = inventoryObject as Evidence;
            if (evidence.IsSealed()) {
                selectedVisualArray[0].SetActive(false);
            } else {
                if (selectedVisualArray.Length > 1) {
                    selectedVisualArray[1].SetActive(false);
                }
            }
        }
    }

    private void Hide() {
        foreach (GameObject selectedCounterVisual in selectedVisualArray) {
            selectedCounterVisual.SetActive(false);
        }
    }
}
