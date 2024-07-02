using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTestStation : LabEquipment {

    public static event EventHandler OnUseBloodTestStation;

    new public static void ResetStaticData() {
        OnUseBloodTestStation = null;
    }

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
        if (InventoryScreenUI.isInAction) {
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
        if (swabToCheck.CannotBeUsed()) {
            MessageLogManager.Instance.LogMessage("This swab has already undergone wrong testing and cannot be tested again.");
            return false;
        }

        if (!swabToCheck.IsUsed()) {
            MessageLogManager.Instance.LogMessage("This swab has not been used. There is nothing to test.");
            return false;
        } else {
            if (swabToCheck.IsPositiveTestedAlready()) {
                MessageLogManager.Instance.LogMessage("This swab has already been positively tested. There is no need to test again.");
                return false;
            } else {
                return true;
            }
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
    }

    public bool CheckEthanolIsAddedFirst() {
        if (!isEthanolAddedFirst && !isphenolphthaleinAddedSecond && !isHydrogenPeroxideAddedThird) {
            isEthanolAddedFirst = true;
            return true;
        } else {
            return false;
        }
    }

    public bool CheckPhenophthaleinIsAddedSecond() {
        if (isEthanolAddedFirst && !isphenolphthaleinAddedSecond && !isHydrogenPeroxideAddedThird) {
            isphenolphthaleinAddedSecond = true;
            return true;
        } else {
            return false;
        }
    }

    public bool CheckHydrogenPeroxideIsAddedThird() {
        if (isEthanolAddedFirst && isphenolphthaleinAddedSecond && !isHydrogenPeroxideAddedThird) {
            isHydrogenPeroxideAddedThird = true;
            return true;
        } else {
            return false;
        }
    }

    public bool CheckHasBeenTestedHalfway() {
        if (isEthanolAddedFirst || isphenolphthaleinAddedSecond || isHydrogenPeroxideAddedThird) {
            return true;
        } else {
            return false;
        }
    }

    public void WrongTestProcedure() {
        swabToTest.ImproperTestAdministered();
        swabToTest = null;
    }

    public void CorrectTestProcedure() {
        swabToTest.PositiveTestAdministered();
        swabToTest = null;
    }
}
