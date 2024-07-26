using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * The class representing the fly net equipment that is used to collect live adult flies.
 */
public class FlyNet : EvidenceInteractingEquipment, IHasProgress {

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;
    public event EventHandler<float> OnActionProgress;
    public event EventHandler OnSweepNet;
    public event EventHandler OnStartNetSweeping;
    new public static void ResetStaticData() {
        OnChangeInteractActionDetails = null;
    }

    private const float CATCHING_RADIUS = 2f;
    private const float CAPTURE_DURATION = 3f;

    [SerializeField] private GameObject megacephalaMaleFlyVisual;
    [SerializeField] private GameObject megacephalaFemaleFlyVisual;
    [SerializeField] private GameObject scalarisFlyVisual;
    [SerializeField] private GameObject ruficornisFlyVisual;
    [SerializeField] private GameObject killingJarVisual;

    //To interact with flying flies
    [SerializeField] private LayerMask flyLayer;

    private List<AdultFly> fliesCollected;

    private Coroutine captureFlyCoroutine;
    private bool isCapturingFlies;

    private void Awake() {
        SetCorrectVisual();
        isCapturingFlies = false;
        captureFlyCoroutine = null;
    }

    private void Start() {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        fliesCollected = EvidenceStorageManager.Instance.GetCapturedFlies();
        SetCorrectVisual();
    }

    /**
     * Reset interaction details if restart level in the middle of interaction
     * that creates change in interaction details
     */
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
        EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
        if (equipmentSO != null) {
            equipmentSO.ChangeInteractionText("Capture Flies", 0);
        }
    }

    public override void Interact() {
        if (isCapturingFlies) {
            MessageLogManager.Instance.LogMessage("Swinging net in progress. Wait for action to be done.");
            return;
        }

        if (fliesCollected.Count == 0) {
            captureFlyCoroutine = StartCoroutine(CaptureFlyCoroutine());
            MessageLogManager.Instance.LogMessage("Swinging net to capture flies...");
        } else {
            EvidenceStorageManager.Instance.AddToKillingJar(fliesCollected);
            EvidenceStorageManager.Instance.ClearCapturedFlies();
            isCapturingFlies = false;
            SetCorrectVisual();

            MessageLogManager.Instance.LogMessage("Flies transferred to acetone killing jar in inventory.");

            //Reset interaction details
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Capture Flies", 0);
            }
            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        }
    }

    /**
     * Tracks the progress of the fly net catching process while the fly net is in use.
     */
    private IEnumerator CaptureFlyCoroutine() {
        float elapsedTime = 0f;
        isCapturingFlies = true;
        OnStartNetSweeping?.Invoke(this, EventArgs.Empty);
        while (elapsedTime < CAPTURE_DURATION) {
            elapsedTime += 0.1f;
            OnActionProgress?.Invoke(this, elapsedTime / CAPTURE_DURATION);
            OnSweepNet?.Invoke(this, EventArgs.Empty);
            
            yield return new WaitForSeconds(0.1f); 
        }

        SweepNet();

        captureFlyCoroutine = null;
        isCapturingFlies = false;

        CheckNet();
    }

    /**
     * Attempts to capture adult flies around area player is looking at.
     */
    private void SweepNet() {

        Vector3 currentStareAt = Player.Instance.GetStareAtPosition();
        Collider[] hitColliders = Physics.OverlapSphere(currentStareAt, CATCHING_RADIUS, flyLayer);

        foreach (Collider hitCollider in hitColliders) {
            FlyingGroupOfFlies groupOfFlies = hitCollider.GetComponent<FlyingGroupOfFlies>();

            List<AdultFly> flyCaptured = groupOfFlies.CaptureFlies();

            EvidenceStorageManager.Instance.CaptureFlies(flyCaptured);
        }
    }

    /**
     * Updates next step based on results of fly catching.
     */
    private void CheckNet() {
        if (fliesCollected.Count > 0) {
            SetCorrectVisual();
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Transfer Flies", 0);
            }

            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        }
    }

    private void SetCorrectVisual() {
        if (fliesCollected == null || fliesCollected.Count <= 0) {
            megacephalaMaleFlyVisual.SetActive(false);
            megacephalaFemaleFlyVisual.SetActive(false);
            scalarisFlyVisual.SetActive(false);
            ruficornisFlyVisual.SetActive(false);
            killingJarVisual.SetActive(false); ;
            return;
        }

        killingJarVisual.SetActive(true);

        foreach (AdultFly adultFly in fliesCollected) {
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

    //Reset any coroutine if swapped to another equipment.
    private void OnDestroy() {
        if (captureFlyCoroutine != null) {
            StopCoroutine(captureFlyCoroutine);
            captureFlyCoroutine = null;
        }
    }

}
