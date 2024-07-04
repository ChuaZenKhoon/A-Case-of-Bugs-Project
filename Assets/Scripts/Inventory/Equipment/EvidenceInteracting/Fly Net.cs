using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyNet : EvidenceInteractingEquipment, IHasProgress {
    
    [SerializeField] private GameObject megacephalaFlyVisual;
    [SerializeField] private GameObject spinigeraFlyVisual;
    [SerializeField] private GameObject killingJarVisual;

    [SerializeField] private LayerMask flyLayer;

    public static event EventHandler<EquipmentSO> OnChangeInteractActionDetails;
    public event EventHandler<float> OnActionProgress;
    new public static void ResetStaticData() {
        OnChangeInteractActionDetails = null;
    }

    private const float CATCHING_RADIUS = 2f;

    private List<AdultFly> fliesCollected;

    private Coroutine captureFlyCoroutine;
    private bool isCapturingFlies;


    private void Awake() {
        megacephalaFlyVisual.SetActive(false);
        spinigeraFlyVisual.SetActive(false);
        SetUpContainer(false);
        isCapturingFlies = false;
        captureFlyCoroutine = null;
    }

    private void Start() {
        fliesCollected = EvidenceStorageManager.Instance.GetCapturedFlies();
        SetCorrectVisual();
    }

    private IEnumerator CaptureFlyCoroutine() {
        float captureDuration = 3f;
        float elapsedTime = 0f;

        isCapturingFlies = true;

        while (elapsedTime < captureDuration) {
            yield return new WaitForSeconds(0.25f);

            elapsedTime += 0.25f;
            OnActionProgress?.Invoke(this, elapsedTime / captureDuration);
        }

        //Capture flies here

        Vector3 currentStareAt = Player.Instance.GetStareAtPosition();

        //dusts area around lookat spot
        Collider[] hitColliders = Physics.OverlapSphere(currentStareAt, CATCHING_RADIUS, flyLayer);

        foreach (Collider hitCollider in hitColliders) {
            FlyingGroupOfFlies groupOfFlies = hitCollider.GetComponent<FlyingGroupOfFlies>();

            List<AdultFly> flyCaptured = groupOfFlies.CaptureFlies();

            EvidenceStorageManager.Instance.CaptureFlies(flyCaptured);
        }

        captureFlyCoroutine = null;
        isCapturingFlies = false;


        if (fliesCollected.Count > 0) {
            SetUpContainer(true);
            SetCorrectVisual();
            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Transfer Flies", 0);
            }
            
            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);
        } 
    }

    private void SetUpContainer(bool state) {
        killingJarVisual.SetActive(state);
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
            SetCorrectVisual();

            SetUpContainer(false);
            isCapturingFlies = false;
            MessageLogManager.Instance.LogMessage("Flies transferred to acetone killing jar in inventory.");

            EquipmentSO equipmentSO = this.GetInventoryObjectSO() as EquipmentSO;
            if (equipmentSO != null) {
                equipmentSO.ChangeInteractionText("Capture Flies", 0);
            }
            OnChangeInteractActionDetails?.Invoke(this, equipmentSO);

        }
    }

    private void SetCorrectVisual() {
        if (fliesCollected.Count > 0) {
            foreach (AdultFly adultFly in fliesCollected) {
                string flyType = adultFly.GetInventoryObjectSO().objectName;

                if (flyType == "Black Fly") {
                    spinigeraFlyVisual.SetActive(true);
                }

                if (flyType == "Green Fly") {
                    megacephalaFlyVisual.SetActive(true);
                }
            }
        } else {
            spinigeraFlyVisual.SetActive(false);
            megacephalaFlyVisual.SetActive(false);
        }
    }

    private void OnDestroy() {
        if (captureFlyCoroutine != null) {
            StopCoroutine(captureFlyCoroutine);
            captureFlyCoroutine = null;
        }
    }
}
