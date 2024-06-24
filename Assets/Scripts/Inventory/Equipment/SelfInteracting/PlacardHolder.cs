using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacardHolder : SelfInteractingEquipment {

    [SerializeField] private GameObject[] placardVisualList;
    [SerializeField] private GameObject[] placementVisual;

    private static int CURRENT_LOWEST_PLACARD_NUM = 0;

    private Placard[] placards;


    new public static void ResetStaticData() {
        CURRENT_LOWEST_PLACARD_NUM = 0;
    }


    private void Start() {
        GameInput.Instance.OnInteract2Action += GameInput_OnInteract2Action;

        foreach (GameObject visual in placardVisualList) {
            visual.SetActive(false);
        }

        placards = EquipmentStorageManager.Instance.GetPlacardList();

        UpdateNextPlacard();
    }

    private void GameInput_OnInteract2Action(object sender, EventArgs e) {
        InventoryObject currentStareAt = Player.Instance.GetStareAt();

        if (currentStareAt is Placard) {
            Placard currentPlacardStaringAt = currentStareAt as Placard;
            int num = currentPlacardStaringAt.GetPlacardNumber();
            
            Placard placardToAdd = currentPlacardStaringAt.GetInventoryObjectSO().prefab.GetComponent<Placard>();
            EquipmentStorageManager.Instance.SetPlacard(num, placardToAdd);
            Destroy(currentStareAt.gameObject);
            UpdateNextPlacard();
        } else {
            MessageLogManager.Instance.LogMessage("Cannot pick up normal items with the placard holder.");
        }
    }

    public override void Interact() {
        if (CURRENT_LOWEST_PLACARD_NUM < placards.Length) {
            GameObject placardPlaced = Instantiate(placards[CURRENT_LOWEST_PLACARD_NUM].GetInventoryObjectSO().prefab);
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            placardPlaced.transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
            placardPlaced.transform.position = Player.Instance.GetStareAtPosition();
            EquipmentStorageManager.Instance.SetPlacard(CURRENT_LOWEST_PLACARD_NUM, null);
            UpdateNextPlacard();
        } else {
            MessageLogManager.Instance.LogMessage("No more placards available");
        }
    }

    private void UpdateNextPlacard() {
        int currentIndex;
        for (currentIndex = 0; currentIndex < placards.Length; currentIndex++) {
            if (placards[currentIndex] != null) {
                CURRENT_LOWEST_PLACARD_NUM = currentIndex;
                break;
            }
        }

        if (currentIndex >= placards.Length) {
            CURRENT_LOWEST_PLACARD_NUM = currentIndex;
        }

        foreach (GameObject visual in placardVisualList) {
            visual.SetActive(false);
        }

        if (CURRENT_LOWEST_PLACARD_NUM < placards.Length) {
            placardVisualList[CURRENT_LOWEST_PLACARD_NUM].SetActive(true);
        }
    }

    public void ShowPlacementPosition(Vector3 placementPosition, bool isShowing) {
        foreach (GameObject visual in placementVisual) {
            visual.SetActive(false);
        }

        if (isShowing && CURRENT_LOWEST_PLACARD_NUM < placementVisual.Length) {
            GameObject placementVisualToShow = placementVisual[CURRENT_LOWEST_PLACARD_NUM];
            placementVisualToShow.gameObject.SetActive(true);
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            placementVisualToShow.transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
            placementVisualToShow.transform.position = placementPosition;
        }
    }
    private void OnDestroy() {
        GameInput.Instance.OnInteract2Action -= GameInput_OnInteract2Action;
    }
}