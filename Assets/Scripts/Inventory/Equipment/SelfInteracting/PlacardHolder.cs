using System;
using UnityEngine;

/**
 * The class representing the placard holder equipment, which contains placards to be placed.
 */
public class PlacardHolder : SelfInteractingEquipment {

    public event EventHandler OnPickup;
    public event EventHandler OnPutDown;

    [SerializeField] private GameObject[] placardVisualList;
    [SerializeField] private GameObject[] placementVisual;

    private static int CURRENT_LOWEST_PLACARD_NUM = 0;

    private Placard[] placards;

    new public static void ResetStaticData() {
        CURRENT_LOWEST_PLACARD_NUM = 0;
    }

    private void Start() {
        GameInput.Instance.OnInteract2Action += GameInput_OnInteract2Action;

        placards = EquipmentStorageManager.Instance.GetPlacardList();

        //Reset visual
        foreach (GameObject visual in placardVisualList) {
            visual.SetActive(false);
        }

        UpdateNextPlacard();
        UpdateVisual();
    }

    private void OnDestroy() {
        GameInput.Instance.OnInteract2Action -= GameInput_OnInteract2Action;
    }

    /**
     * Interacts with a placard if the player is looking at one to pick it up, putting it back in its correct position.
     */
    private void GameInput_OnInteract2Action(object sender, EventArgs e) {
        InteractableObject currentStareAt = Player.Instance.GetStareAt();

        if (currentStareAt is Placard) {
            Placard currentPlacardStaringAt = currentStareAt as Placard;
            int num = currentPlacardStaringAt.GetPlacardNumber();
            
            Placard placardToAdd = currentPlacardStaringAt.GetInventoryObjectSO().prefab.GetComponent<Placard>();
            EquipmentStorageManager.Instance.SetPlacard(num, placardToAdd);
            Destroy(currentStareAt.gameObject);
            UpdateNextPlacard();
            UpdateVisual();

            OnPickup?.Invoke(this, EventArgs.Empty);
        } else {
            MessageLogManager.Instance.LogMessage("Cannot pick up normal items with the placard holder.");
        }
    }

    /**
     * Player puts down a placard at the highlighted location if in interact distance, else places it on the floor
     * at the player's position. No placard is placed if there are no more available.
     */
    public override void Interact() {
        if (CURRENT_LOWEST_PLACARD_NUM < placards.Length) {
            PlacePlacard();
            UpdateNextPlacard();
            UpdateVisual();
        } else {
            MessageLogManager.Instance.LogMessage("No more placards available");
        }
    }

    //Places a placard down.
    private void PlacePlacard() {
        GameObject placardPlaced = Instantiate(placards[CURRENT_LOWEST_PLACARD_NUM].GetInventoryObjectSO().prefab);

        //Adjust placement position
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        placardPlaced.transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
        placardPlaced.transform.position = Player.Instance.GetStareAtPosition();
        
        //Update storage
        EquipmentStorageManager.Instance.SetPlacard(CURRENT_LOWEST_PLACARD_NUM, null);

        OnPutDown?.Invoke(this, EventArgs.Empty);
    }

    /**
     * Updates the next placard to be placed based on what placard numbers are available.
     */
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
    }

    private void UpdateVisual() {
        foreach (GameObject visual in placardVisualList) {
            visual.SetActive(false);
        }

        if (CURRENT_LOWEST_PLACARD_NUM < placards.Length) {
            placardVisualList[CURRENT_LOWEST_PLACARD_NUM].SetActive(true);
        }
    }


    /**
     * Highlights to the player the position where a placard can be placed in distance.
     * 
     * @param placementPosition The available position.
     * @param isShowing Whether to show the placement position or not.
     */
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

}