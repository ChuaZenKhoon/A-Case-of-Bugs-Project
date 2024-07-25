using System;
using UnityEngine;

/**
 * The class representing the swab equipment. 
 * As of the current iteration, swabs can only collect red stains, not DNA.
 */
public class Swab : EvidenceInteractingEquipment {

    public event EventHandler OnSwabUse;

    [SerializeField] private GameObject canBeTestedVisual;
    [SerializeField] private GameObject usedVisual; //is part of the above

    [SerializeField] private GameObject positiveResultVisual;
    
    [SerializeField] private GameObject cannotBeTestedVisual;

    public enum TestState {
        Unused,
        Used,
        PositiveResult,
        CannotBeTestedAnymore
    }

    private Bloodstain bloodStain;

    private TestState state;

    private void Awake() {
        state = TestState.Unused;
        UpdateVisual(state);
    }

    private void Start() {
        bloodStain = EvidenceStorageManager.Instance.GetBloodStain(this.GetEquipmentID(), out TestState testState);
        state = testState;
        UpdateVisual(testState);
    }


    public override void Interact() {
        if (bloodStain == null) {
            InteractableObject currentStareAt = Player.Instance.GetStareAt();

            if (currentStareAt is Bloodstain) {
                CollectStain(currentStareAt);
                MessageLogManager.Instance.LogMessage("Red stain successfully collected!");
            } else {
                //Not bloodstain
                MessageLogManager.Instance.LogMessage("Cannot pick up normal items with the swab.");
            }
        } else {
            //Used
            MessageLogManager.Instance.LogMessage("This swab has already been used.");
        }
    }

    private void CollectStain(InteractableObject currentStareAt) {
        Bloodstain currentBloodStainStaringAt = currentStareAt as Bloodstain;

        Bloodstain liquidToSwab = currentBloodStainStaringAt.GetInventoryObjectSO().prefab.GetComponentInChildren<Bloodstain>();
        bloodStain = liquidToSwab;
        state = TestState.Used;
        EvidenceStorageManager.Instance.SetBloodStain(this.GetEquipmentID(), bloodStain, state);

        UpdateVisual(state);
        OnSwabUse?.Invoke(this, EventArgs.Empty);
    }

    //Updates the shown swab based on the state of it
    private void UpdateVisual(TestState state) {
        switch (state) {
            case TestState.Unused:
                cannotBeTestedVisual.SetActive(false);
                usedVisual.SetActive(false);
                positiveResultVisual.SetActive(false);
                break;
            case TestState.Used:
                canBeTestedVisual.SetActive(true);
                cannotBeTestedVisual.SetActive(false);
                usedVisual.SetActive(true);
                positiveResultVisual.SetActive(false);
                break;
            case TestState.PositiveResult:
                cannotBeTestedVisual.SetActive(false);
                positiveResultVisual.SetActive(true);
                break;
            case TestState.CannotBeTestedAnymore:
                cannotBeTestedVisual.SetActive(true);
                canBeTestedVisual.SetActive(false);
                break;
        }
    }

    //The next 3 methods deal with the interaction with the blood test station lab equipment.

    /**
     * Updates state from blood test station use when test gives positive result.
     */
    public void AdministerCorrectTest() {
        state = TestState.PositiveResult;
        EvidenceStorageManager.Instance.SetBloodStain(this.GetEquipmentID(), bloodStain, TestState.PositiveResult);
        UpdateVisual(state);
        MessageLogManager.Instance.LogMessage("Cotton swab turns pink! Positive test result obtained. This sample could be human or animal blood!");
    }

    /**
     * Updates state from blood test station use when test is wrongly performed.
     */
    public void AdministerIncorrectTest() {
        state = TestState.CannotBeTestedAnymore;
        EvidenceStorageManager.Instance.SetBloodStain(this.GetEquipmentID(), null, TestState.CannotBeTestedAnymore);
        UpdateVisual(state);
        MessageLogManager.Instance.LogMessage("Improper test administered. Swab is stored and to be thrown later.");
    }

    /**
     * Updates state from blood test station use when test is incomplete.
     */
    public void AdministerIncompleteTest() {
        state = TestState.CannotBeTestedAnymore;
        EvidenceStorageManager.Instance.SetBloodStain(this.GetEquipmentID(), null, TestState.CannotBeTestedAnymore);
        UpdateVisual(state);
        MessageLogManager.Instance.LogMessage("Incomplete test administered. Try to finish the blood test without exiting next time.");
    }

    public TestState CurrentState() {
        return state;
    }
}
