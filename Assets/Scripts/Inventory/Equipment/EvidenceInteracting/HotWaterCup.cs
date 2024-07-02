using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HotWaterCup : EvidenceInteractingEquipment, IHasProgress {


    [SerializeField] private GameObject megacephalaMaggotVisual;
    [SerializeField] private GameObject scalarisPupaVisual;
    [SerializeField] private List<GameObject> ethanolContainerVisual;

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;

    new public static void ResetStaticData() {
        OnChangeInteractActionDetails = null;
    }

    private List<Larvae> maggotsCollected;

    private Coroutine killMaggotCoroutine;
    private bool isKillingDone;
    private float savedKillingMaggotDuration;

    public event EventHandler<float> OnActionProgress;

    private void Awake() {
        megacephalaMaggotVisual.SetActive(false);
        scalarisPupaVisual.SetActive(false);
        maggotsCollected = new List<Larvae>();
        SetUpContainer(false);
        isKillingDone = false;
        killMaggotCoroutine = null;
    }

    private void Start() {
        maggotsCollected = EquipmentStorageManager.Instance.GetMaggots();
        savedKillingMaggotDuration = EquipmentStorageManager.Instance.GetKillingMaggotProgressDuration();
        GameInput.Instance.OnInteract2Action += GameInput_OnInteract2Action;
        SetCorrectVisual();
    }

    private void GameInput_OnInteract2Action(object sender, System.EventArgs e) {
        
        if (!isKillingDone) {
            if (maggotsCollected.Count > 0) {
                if (killMaggotCoroutine != null) {
                    return;
                }

                killMaggotCoroutine = StartCoroutine(KillMaggotCoroutine());
                MessageLogManager.Instance.LogMessage("Poured hot water. Waiting for specimens to be killed...");
            }
        } else {
           
            EquipmentStorageManager.Instance.AddKilledMaggots(maggotsCollected);
            EquipmentStorageManager.Instance.ClearMaggotCollection();
            EquipmentStorageManager.Instance.SetKillingMaggotProgressDuration(0f);
            SetCorrectVisual();

            SetUpContainer(false);
            isKillingDone = false;
            MessageLogManager.Instance.LogMessage("Specimens transferred to ethanol container in inventory.");

            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Kill specimens", 1);
            }
            OnChangeInteractActionDetails?.Invoke(this, this.GetInventoryObjectSO() as EquipmentSO);
            
        }
    }

    private IEnumerator KillMaggotCoroutine() {
        float killDuration = 30f;

        while (savedKillingMaggotDuration < killDuration) {
            yield return new WaitForSeconds(0.5f);

            savedKillingMaggotDuration += 0.5f;
            OnActionProgress?.Invoke(this, savedKillingMaggotDuration / killDuration);
        }
        savedKillingMaggotDuration = 0f;
        EquipmentStorageManager.Instance.SetKillingMaggotProgressDuration(savedKillingMaggotDuration);
        killMaggotCoroutine = null;
        SetUpContainer(true);
        isKillingDone = true;
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Transfer Specimens", 1);
        }
        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
    }

    private void SetUpContainer(bool state) {
        foreach (GameObject gameObject in ethanolContainerVisual) {
            gameObject.SetActive(state);
        }
    }
    public override void Interact() {

        if (savedKillingMaggotDuration > 0f) {
            MessageLogManager.Instance.LogMessage("Killing batch of specimen in progress. Wait for batch to be done and transferred.");
            return;
        }

        InteractableObject currentStareAt = Player.Instance.GetStareAt();

        if (currentStareAt is Larvae) {
            Larvae currentMaggotStaringAt = currentStareAt as Larvae;

            Larvae maggotToCollect = currentMaggotStaringAt.GetInventoryObjectSO().prefab.GetComponent<Larvae>();
            
            //maggotsCollected.Add(maggotToCollect);
            EquipmentStorageManager.Instance.CollectMaggots(maggotToCollect);

            Destroy(currentStareAt.gameObject);

            SetCorrectVisual();

            MessageLogManager.Instance.LogMessage("Specimen successfully collected!");
        } else {
            MessageLogManager.Instance.LogMessage("Unsuitable equipment for picking up such items.");
        }
    }

    private void SetCorrectVisual() {
        if (maggotsCollected.Count > 0) {
            foreach (Larvae maggot in maggotsCollected) {
                string maggotType = maggot.GetInventoryObjectSO().objectName;

                if (maggotType == "Long thin cylinder") {
                    scalarisPupaVisual.SetActive(true);
                }

                if (maggotType == "Cylindrical Maggot") {
                    megacephalaMaggotVisual.SetActive(true);
                }
            } 
        } else {
            scalarisPupaVisual.SetActive(false);
            megacephalaMaggotVisual.SetActive(false);
        }
    }

    private void OnDestroy() {
        if (killMaggotCoroutine != null) {
            EquipmentStorageManager.Instance.SetKillingMaggotProgressDuration(savedKillingMaggotDuration);
            StopCoroutine(killMaggotCoroutine);
            killMaggotCoroutine = null;
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Kill specimens", 1);
            }
            OnChangeInteractActionDetails?.Invoke(this, this.GetInventoryObjectSO() as EquipmentSO);
        } else if (isKillingDone) {
            EquipmentStorageManager.Instance.SetKillingMaggotProgressDuration(30f);
        }

        GameInput.Instance.OnInteract2Action -= GameInput_OnInteract2Action;

    }

}
