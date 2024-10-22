using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * The class representing the hot water cup equipment used to kill larvae and pupae.
 */
public class HotWaterCup : EvidenceInteractingEquipment, IHasProgress {
    
    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;
    public event EventHandler<float> OnActionProgress;
    public event EventHandler OnPourWater;
    public event EventHandler OnCollectSpecimen;
    new public static void ResetStaticData() {
        OnChangeInteractActionDetails = null;
    }

    private const float KILL_DURATION = 30f;

    [SerializeField] private GameObject megacephalaMaggotVisual;
    [SerializeField] private GameObject scalarisPupaVisual;
    [SerializeField] private GameObject ethanolContainerVisual;
    [SerializeField] private GameObject waterVisual;

    private List<Larvae> larvaePupaeCollected;
    private List<Larvae> larvaePupaeKilled;

    private Coroutine killMaggotCoroutine;
    private bool isKillingDone;
    private float savedKillingMaggotDuration;

    private void Awake() {
        isKillingDone = false;
        killMaggotCoroutine = null;
        savedKillingMaggotDuration = 0f;
    }

    private void Start() {
        GameInput.Instance.OnInteract2Action += GameInput_OnInteract2Action;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        larvaePupaeCollected = EvidenceStorageManager.Instance.GetMaggots();
        larvaePupaeKilled = EvidenceStorageManager.Instance.GetReadyToTransferMaggots();
        savedKillingMaggotDuration = EvidenceStorageManager.Instance.GetKillingMaggotProgressDuration();
        isKillingDone = savedKillingMaggotDuration >= 30f;

        SetCorrectVisual();
    }

    /**
     * Reset interaction details if restart level in the middle of interaction
     * that creates change in interaction details
     */
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Kill specimens", 1);
        }
    }

    public override void Interact() {
        if (larvaePupaeKilled.Count > 0) {
            MessageLogManager.Instance.LogMessage("There is a batch of specimens inside. Transfer first before collecting a new batch.");
            return;
        }

        if (savedKillingMaggotDuration > 0f) {
            MessageLogManager.Instance.LogMessage("Killing batch of specimen in progress. Wait for batch to be done and transferred.");
            return;
        }

        InteractableObject currentStareAt = Player.Instance.GetStareAt();

        if (currentStareAt is Larvae) {
            CollectLarvaePupae(currentStareAt);
            MessageLogManager.Instance.LogMessage("Specimen successfully collected!");
        } else {
            MessageLogManager.Instance.LogMessage("Unsuitable equipment for picking up such items.");
        }
    }

    private void CollectLarvaePupae(InteractableObject currentStareAt) {
        Larvae currentMaggotStaringAt = currentStareAt as Larvae;

        Larvae maggotToCollect = currentMaggotStaringAt.GetInventoryObjectSO().prefab.GetComponent<Larvae>();

        EvidenceStorageManager.Instance.CollectMaggots(maggotToCollect);

        Destroy(currentStareAt.gameObject);

        SetCorrectVisual();

        OnCollectSpecimen?.Invoke(this, EventArgs.Empty);
    }

    /**
     * Handles the killing of collected larvae and pupae. 
     * Allows for continuation from previous saved progress duration.
     */
    private void GameInput_OnInteract2Action(object sender, System.EventArgs e) {
        
        if (!isKillingDone) {
            AttemptNewKillProcedure();
        } else {
            TransferKilledLarvaePupae();
        }
    }

    private void AttemptNewKillProcedure() {
        if (larvaePupaeCollected.Count > 0) {
            if (killMaggotCoroutine != null) {
                //Current killing process in action
                return;
            }

            killMaggotCoroutine = StartCoroutine(KillMaggotCoroutine());
            waterVisual.SetActive(true);
            OnPourWater?.Invoke(this, EventArgs.Empty);
            MessageLogManager.Instance.LogMessage("Poured hot water. Waiting for specimens to be killed...");
        } else {
            MessageLogManager.Instance.LogMessage("There is no specimens collected to kill.");
        }
    }

    /**
     * Once killing process is done, player can transfer specimens to ethanol container.
     */
    private void TransferKilledLarvaePupae() {
        savedKillingMaggotDuration = 0f;
        isKillingDone = false;
        EvidenceStorageManager.Instance.SetKillingMaggotProgressDuration(savedKillingMaggotDuration);
        EvidenceStorageManager.Instance.AddKilledMaggots(larvaePupaeKilled);
        EvidenceStorageManager.Instance.ClearReadyToTransferMaggots();
        
        SetCorrectVisual();


        MessageLogManager.Instance.LogMessage("Specimens transferred to ethanol container in inventory.");

        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Kill specimens", 1);
        }
        OnChangeInteractActionDetails?.Invoke(this, this.GetInventoryObjectSO() as EquipmentSO);
    }

    /**
     * Tracks the killing process while the hot water cup is in use.
     */
    private IEnumerator KillMaggotCoroutine() {
        while (savedKillingMaggotDuration < KILL_DURATION) {
            yield return new WaitForSeconds(0.1f);

            savedKillingMaggotDuration += 0.1f;
            OnActionProgress?.Invoke(this, savedKillingMaggotDuration / KILL_DURATION);
        }

        //Killing done, move to killed list
        EvidenceStorageManager.Instance.SetKillingMaggotProgressDuration(savedKillingMaggotDuration);
        EvidenceStorageManager.Instance.AddKilledMaggotReadyToTransfer(larvaePupaeCollected);
        EvidenceStorageManager.Instance.ClearMaggotCollection();
        killMaggotCoroutine = null;
        isKillingDone = true;
        SetCorrectVisual();
        
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Transfer Specimens", 1);
        }
        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);

        MessageLogManager.Instance.LogMessage("Killing done. Okay to transfer batch of specimen now!");
    }

    private void SetCorrectVisual() {
        if (larvaePupaeKilled.Count > 0) {
            waterVisual.SetActive(true);
            ethanolContainerVisual.SetActive(true);

            foreach (Larvae maggot in larvaePupaeKilled) {
                string maggotType = maggot.GetInventoryObjectSO().objectName;

                if (maggotType == "Long thin cylinder") {
                    scalarisPupaVisual.SetActive(true);
                }

                if (maggotType == "Cylindrical Maggot") {
                    megacephalaMaggotVisual.SetActive(true);
                }
            }

        } else if (larvaePupaeCollected.Count <= 0) {
            megacephalaMaggotVisual.SetActive(false);
            scalarisPupaVisual.SetActive(false);
            ethanolContainerVisual.SetActive(false);
            waterVisual.SetActive(false);
            return;
        }
        
        foreach (Larvae maggot in larvaePupaeCollected) {
            string maggotType = maggot.GetInventoryObjectSO().objectName;

            if (maggotType == "Long thin cylinder") {
                scalarisPupaVisual.SetActive(true);
            }

            if (maggotType == "Cylindrical Maggot") {
                megacephalaMaggotVisual.SetActive(true);
            }
        }

    }

    private void OnDestroy() {
        if (killMaggotCoroutine != null) {
            EvidenceStorageManager.Instance.SetKillingMaggotProgressDuration(savedKillingMaggotDuration);
            StopCoroutine(killMaggotCoroutine);
            killMaggotCoroutine = null;
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Kill specimens", 1);
            }
            OnChangeInteractActionDetails?.Invoke(this, this.GetInventoryObjectSO() as EquipmentSO);
        }

        GameInput.Instance.OnInteract2Action -= GameInput_OnInteract2Action;

    }

}
