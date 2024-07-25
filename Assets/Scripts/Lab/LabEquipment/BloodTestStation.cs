using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTestStation : LabEquipment {

    public static event EventHandler OnUseBloodTestStation;

    new public static void ResetStaticData() {
        OnUseBloodTestStation = null;
    }

    public event EventHandler OnCorrectTest;
    public event EventHandler OnWrongTest;

    [SerializeField] private BloodTestStationUI bloodTestStationUI;
    [SerializeField] private Camera equipmentCamera;
    [SerializeField] private CanvasGroup gameplayCanvas;
    [SerializeField] private BoxCollider boxCollider;

    private Camera playerCamera;

    private bool isUsingEquipmentMode;

    private Swab swabToTest;

    private bool isEthanolAddedFirst;
    private bool isphenolphthaleinAddedSecond;
    private bool isHydrogenPeroxideAddedThird;


    private void Start() {
        isEthanolAddedFirst = false;
        isphenolphthaleinAddedSecond = false;
        isHydrogenPeroxideAddedThird = false;

        isUsingEquipmentMode = false;
        equipmentCamera.enabled = false;
        playerCamera = Camera.main;
        GameObject canvas = GameObject.Find("Gameplay Canvas");
        gameplayCanvas = canvas.GetComponent<CanvasGroup>();
    }

    public override void Interact() {
        if (InventoryManager.Instance.IsInventoryOpen()) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before using the lab equipment.");
            return;
        }

        InventoryObject playerHeldItem = Player.Instance.GetHeldItem();


        bool canTest = false;
        Swab swabToCheck = null;
        if (playerHeldItem is Swab) {
            swabToCheck = playerHeldItem as Swab;
            canTest = CheckTestability(swabToCheck);
        } else {
            MessageLogManager.Instance.LogMessage("Unable to use blood test station with current held item.");
        }

        if (canTest) {
            swabToTest = swabToCheck;
            if (!isUsingEquipmentMode) {
                GoIntoEquipmentScreen();
            } else {
                ExitFromEquipmentScreen();
            }
        }
    }

    private bool CheckTestability(Swab swabToCheck) {
        Swab.TestState state = swabToCheck.CurrentState();

        switch (state) {
            case Swab.TestState.Unused:
                MessageLogManager.Instance.LogMessage("This swab has not been used. There is nothing to test.");
                return false;
            case Swab.TestState.Used:
                return true;
            case Swab.TestState.PositiveResult:
                MessageLogManager.Instance.LogMessage("This swab has already been positively tested. There is no need to test again.");
                return false;
            case Swab.TestState.CannotBeTestedAnymore:
                MessageLogManager.Instance.LogMessage("This swab has already undergone wrong testing and cannot be tested again.");
                return false;
            default:
                return false;
        }
    }

    private void GoIntoEquipmentScreen() {
        boxCollider.enabled = false;
        isUsingEquipmentMode = true;
        LabEquipment.isInAction = true;
        OnUseBloodTestStation?.Invoke(this, EventArgs.Empty);
        playerCamera.enabled = false;
        equipmentCamera.enabled = true;

        gameplayCanvas.alpha = 0f;
        gameplayCanvas.blocksRaycasts = false;

        bloodTestStationUI.Show();
    }

    public void ExitFromEquipmentScreen() {
        isEthanolAddedFirst = false;
        isphenolphthaleinAddedSecond = false;
        isHydrogenPeroxideAddedThird = false;

        boxCollider.enabled = true;
        isUsingEquipmentMode = false;
        LabEquipment.isInAction = false;
        OnUseBloodTestStation?.Invoke(this, EventArgs.Empty);
        equipmentCamera.enabled = false;
        playerCamera.enabled = true;

        gameplayCanvas.alpha = 1f;
        gameplayCanvas.blocksRaycasts = true;
    }

    //The following 4 methods deal with using the blood test station.


    public void ExitStation() {
        bool isTestedHalfWay = CheckHasBeenTestedHalfway();

        if (isTestedHalfWay) {
            IncompleteTestProcedure();
        }

        ExitFromEquipmentScreen();
    }

    public void AddEthanol() {
        bool isCorrect = CheckEthanolIsAddedFirst();
        if (isCorrect) {
            MessageLogManager.Instance.LogMessage("Correct step! What next?");
        } else {
            bloodTestStationUI.Hide();
            ExitFromEquipmentScreen();
            WrongTestProcedure();
        }
    }

    public void AddPhenophtalein() {
        bool isCorrect = CheckPhenophthaleinIsAddedSecond();
        if (isCorrect) {
            MessageLogManager.Instance.LogMessage("Correct step! What next?");
        } else {
            bloodTestStationUI.Hide();
            ExitFromEquipmentScreen();
            WrongTestProcedure();
        }
    }

    public void AddHydrogenPeroxide() {
        bool isCorrect = CheckHydrogenPeroxideIsAddedThird();
        if (isCorrect) {
            CorrectTestProcedure();  
        } else {
            WrongTestProcedure();
        }
        bloodTestStationUI.Hide();
        ExitFromEquipmentScreen();
    }

    private bool CheckEthanolIsAddedFirst() {
        if (!isEthanolAddedFirst && !isphenolphthaleinAddedSecond && !isHydrogenPeroxideAddedThird) {
            isEthanolAddedFirst = true;
            return true;
        } else {
            return false;
        }
    }

    private bool CheckPhenophthaleinIsAddedSecond() {
        if (isEthanolAddedFirst && !isphenolphthaleinAddedSecond && !isHydrogenPeroxideAddedThird) {
            isphenolphthaleinAddedSecond = true;
            return true;
        } else {
            return false;
        }
    }

    private bool CheckHydrogenPeroxideIsAddedThird() {
        if (isEthanolAddedFirst && isphenolphthaleinAddedSecond && !isHydrogenPeroxideAddedThird) {
            isHydrogenPeroxideAddedThird = true;
            return true;
        } else {
            return false;
        }
    }

    private bool CheckHasBeenTestedHalfway() {
        if (isEthanolAddedFirst || isphenolphthaleinAddedSecond || isHydrogenPeroxideAddedThird) {
            return true;
        } else {
            return false;
        }
    }

    public void WrongTestProcedure() {
        swabToTest.AdministerIncorrectTest();
        swabToTest = null;
        OnWrongTest?.Invoke(this, EventArgs.Empty);
    }

    public void CorrectTestProcedure() {
        swabToTest.AdministerCorrectTest();
        swabToTest = null;
        OnCorrectTest?.Invoke(this, EventArgs.Empty);
    }

    public void IncompleteTestProcedure() {
        swabToTest.AdministerIncompleteTest();
        swabToTest = null;
        OnWrongTest?.Invoke(this, EventArgs.Empty);
    }

}
