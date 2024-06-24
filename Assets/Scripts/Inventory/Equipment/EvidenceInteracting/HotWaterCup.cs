using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HotWaterCup : EvidenceInteractingEquipment, IHasProgress {


    [SerializeField] private GameObject megacephalaMaggotVisual;
    [SerializeField] private GameObject spinigeraMaggotVisual;
    [SerializeField] private List<GameObject> ethanolContainerVisual;

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;

    new public static void ResetStaticData() {
        OnChangeInteractActionDetails = null;
    }

    private List<Maggot> maggotsCollected;

    private Coroutine killMaggotCoroutine;
    private bool isKillingDone;
    private bool isKillingMaggots;

    public event EventHandler<float> OnActionProgress;

    private void Awake() {
        megacephalaMaggotVisual.SetActive(false);
        spinigeraMaggotVisual.SetActive(false);
        maggotsCollected = new List<Maggot>();
        SetUpContainer(false);
        isKillingDone = false;
        isKillingMaggots = false;
        killMaggotCoroutine = null;
    }

    private void Start() {
        maggotsCollected = EquipmentStorageManager.Instance.GetMaggots();
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
                MessageLogManager.Instance.LogMessage("Killing maggots with hot water...");
            }
        } else {
           
            EquipmentStorageManager.Instance.AddKilledMaggots(maggotsCollected);
            EquipmentStorageManager.Instance.ClearMaggotCollection();
            SetCorrectVisual();

            SetUpContainer(false);
            isKillingDone = false;
            isKillingMaggots = false;
            MessageLogManager.Instance.LogMessage("Maggots transferred to ethanol container in inventory.");

            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Kill maggots", 1);
            }
            OnChangeInteractActionDetails?.Invoke(this, this.GetInventoryObjectSO() as EquipmentSO);
            
        }
    }

    private IEnumerator KillMaggotCoroutine() {
        float killDuration = 30f;
        float elapsedTime = 0f;

        isKillingMaggots = true;

        while (elapsedTime < killDuration) {
            yield return new WaitForSeconds(0.5f);

            elapsedTime += 0.5f;
            OnActionProgress?.Invoke(this, elapsedTime / killDuration);
        }
        killMaggotCoroutine = null;
        SetUpContainer(true);
        isKillingDone = true;
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Transfer Maggot", 1);
        }
        OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
    }

    private void SetUpContainer(bool state) {
        foreach (GameObject gameObject in ethanolContainerVisual) {
            gameObject.SetActive(state);
        }
    }
    public override void Interact() {

        if (isKillingMaggots) {
            MessageLogManager.Instance.LogMessage("Killing batch of maggots in progress. Wait for batch to be done and transferred.");
            return;
        }

        InventoryObject currentStareAt = Player.Instance.GetStareAt();

        if (currentStareAt is Maggot) {
            Maggot currentMaggotStaringAt = currentStareAt as Maggot;

            Maggot maggotToCollect = currentMaggotStaringAt.GetInventoryObjectSO().prefab.GetComponent<Maggot>();
            
            //maggotsCollected.Add(maggotToCollect);
            EquipmentStorageManager.Instance.CollectMaggots(maggotToCollect);

            Destroy(currentStareAt.gameObject);

            SetCorrectVisual();

            MessageLogManager.Instance.LogMessage("Maggot successfully collected!");
        } else {
            MessageLogManager.Instance.LogMessage("Unsuitable equipment for picking up such items.");
        }
    }

    private void SetCorrectVisual() {
        if (maggotsCollected.Count > 0) {
            foreach (Maggot maggot in maggotsCollected) {
                string maggotType = maggot.GetInventoryObjectSO().objectName;

                if (maggotType == "Long Thin Maggot") {
                    spinigeraMaggotVisual.SetActive(true);
                }

                if (maggotType == "Cylindrical Maggot") {
                    megacephalaMaggotVisual.SetActive(true);
                }
            } 
        } else {
            spinigeraMaggotVisual.SetActive(false);
            megacephalaMaggotVisual.SetActive(false);
        }
    }

    private void OnDestroy() {
        if (killMaggotCoroutine != null) {
            StopCoroutine(killMaggotCoroutine);
            killMaggotCoroutine = null;
        }

        GameInput.Instance.OnInteract2Action -= GameInput_OnInteract2Action;

    }

}
