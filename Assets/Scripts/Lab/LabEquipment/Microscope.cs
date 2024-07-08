using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * The class representing the microscope lab equipment used to examine flies and larvae/pupae.
 */
public class Microscope : LabEquipment {

    public static event EventHandler OnUseMicroscope;

    new public static void ResetStaticData() {
        OnUseMicroscope = null;
    }

    [SerializeField] private MicroscopeUI microscopeUI;

    private List<AdultFly> fliesToExamine;
    private List<Larvae> larvaeToExamine;

    private AdultFly currentFlyExamined;
    private Larvae currentLarvaExamined;
    private int currentItemIndex;
    private string currentItemName;

    private List<Sprite> currentImagesOfExamined;
    private int currentImageIndex;

    public override void Interact() {
        if (InventoryManager.Instance.IsInventoryOpen()) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before using the lab equipment.");
            return;
        }

        InventoryObject playerHeldItem = Player.Instance.GetHeldItem();

        if (!(playerHeldItem is Container || 
            playerHeldItem is AcetoneKillJar || 
            playerHeldItem is EthanolContainer)) {
            MessageLogManager.Instance.LogMessage("Unable to examine contents of held item with the microscope.");
        } else {
            AttemptExamination(playerHeldItem);
        }
    }

    /*
     * Sets up microscope to examine fly evidence in the correct format.
     */
    private void AttemptExamination(InventoryObject playerHeldItem) {
        switch (playerHeldItem) {
            case Container:
                Container container = playerHeldItem as Container;
                ExamineDeadFlies(container);
                break;
            case AcetoneKillJar:
                AcetoneKillJar acetoneKillJar = playerHeldItem as AcetoneKillJar;
                ExamineKilledFlies(acetoneKillJar);
                break;
            case EthanolContainer:
                EthanolContainer ethanolContainer = playerHeldItem as EthanolContainer;
                ExamineLarvae(ethanolContainer);
                break;
        }
    }

    /**
     * Examines the first dead fly in the container.
     */
    private void ExamineDeadFlies(Container container) {
        fliesToExamine = container.GetDeadFlies();

        if (fliesToExamine.Count > 0) {
            currentFlyExamined = fliesToExamine[0];
            currentItemName = currentFlyExamined.GetInventoryObjectSO().name;
            currentItemIndex = 0;

            currentImagesOfExamined = currentFlyExamined.GetSpritesForExamination();
            currentImageIndex = 0;

            microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
            microscopeUI.UpdateText(currentItemName);
            microscopeUI.Show();
            LabEquipment.isInAction = true;
            OnUseMicroscope?.Invoke(this, EventArgs.Empty);
        } else {
            MessageLogManager.Instance.LogMessage("There is nothing contained to examine");
        }
    }

    /**
     * Examines the first killed fly in the acetone kill jar.
     */
    private void ExamineKilledFlies(AcetoneKillJar acetoneKillJar) {
        fliesToExamine = acetoneKillJar.GetKilledFlies();

        if (fliesToExamine.Count > 0) { 
            currentFlyExamined = fliesToExamine[0];
            currentItemName = currentFlyExamined.GetInventoryObjectSO().name;
            currentItemIndex = 0;

            currentImagesOfExamined = currentFlyExamined.GetSpritesForExamination();
            currentImageIndex = 0;

            microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
            microscopeUI.UpdateText(currentItemName);
            microscopeUI.Show();
            LabEquipment.isInAction = true;
            OnUseMicroscope?.Invoke(this, EventArgs.Empty);
        } else {
            MessageLogManager.Instance.LogMessage("There is nothing contained to examine");
        }
    }

    /**
     * Examines the first killed larvae/pupae in the ethanol container.
     */
    private void ExamineLarvae(EthanolContainer ethanolContainer) {
        larvaeToExamine = ethanolContainer.GetKilledLarvae();

        if (larvaeToExamine.Count > 0) {
            currentLarvaExamined = larvaeToExamine[0];
            currentItemName = currentLarvaExamined.GetInventoryObjectSO().name;
            currentItemIndex = 0;

            currentImagesOfExamined = currentLarvaExamined.GetSpritesForExamination();
            currentImageIndex = 0;

            microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
            microscopeUI.UpdateText(currentItemName);
            microscopeUI.Show();
            LabEquipment.isInAction = true;
            OnUseMicroscope?.Invoke(this, EventArgs.Empty);
        } else {
            MessageLogManager.Instance.LogMessage("There is nothing contained to examine");
        }
    }

    /**
     * Examines the next item under the microscope, updating the relevant details.
     */
    public void NextItemToExamine() {
        if (fliesToExamine != null) {
            int nextItemNumber = (currentItemIndex + 1) % fliesToExamine.Count;
            currentItemIndex = nextItemNumber;
            currentFlyExamined = fliesToExamine[currentItemIndex];
            currentImagesOfExamined = currentFlyExamined.GetSpritesForExamination();
            currentItemName = currentFlyExamined.GetInventoryObjectSO().name;
            UpdateVisual();
        } else if (larvaeToExamine != null) {
            int nextItemNumber = (currentItemIndex + 1) % larvaeToExamine.Count;
            currentItemIndex = nextItemNumber;
            currentLarvaExamined = larvaeToExamine[currentItemIndex];
            currentImagesOfExamined = currentLarvaExamined.GetSpritesForExamination();
            currentItemName = currentLarvaExamined.GetInventoryObjectSO().name;
            UpdateVisual();
        }
    }

    private void UpdateVisual() {
        currentImageIndex = 0;
        microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
        microscopeUI.UpdateText(currentItemName);
    }

    /**
     * Examines the previous item under the microscope, updating the relevant details.
     */
    public void PreviousItemToExamine() {
        if (fliesToExamine != null) {
            int previousItemNumber = (currentItemIndex - 1 + fliesToExamine.Count) % fliesToExamine.Count;
            currentItemIndex = previousItemNumber;
            currentFlyExamined = fliesToExamine[currentItemIndex];
            currentImagesOfExamined = currentFlyExamined.GetSpritesForExamination();
            currentItemName = currentFlyExamined.GetInventoryObjectSO().name;
            UpdateVisual();
        } else if (larvaeToExamine != null) {
            int previousItemNumber = (currentItemIndex - 1 + larvaeToExamine.Count) % larvaeToExamine.Count;
            currentItemIndex = previousItemNumber;
            currentLarvaExamined = larvaeToExamine[currentItemIndex];
            currentImagesOfExamined = currentLarvaExamined.GetSpritesForExamination();
            currentItemName = currentLarvaExamined.GetInventoryObjectSO().name;
            UpdateVisual();
        }
    }

    //Shows the next photograph of the same specimen examined.
    public void NextPhotograph() {
        int nextPhotographNumber = (currentImageIndex + 1) % currentImagesOfExamined.Count;
        currentImageIndex = nextPhotographNumber;
        microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
    }

    //Shows the previous photograph of the same specimen examined.
    public void PreviousPhotograph() {
        int previousPhotographNumber = (currentImageIndex - 1 + currentImagesOfExamined.Count) % currentImagesOfExamined.Count;
        currentImageIndex = previousPhotographNumber;
        microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
    }

    //Resets the microscope upon exiting it.
    public void ClearItems() {
        fliesToExamine = null;
        larvaeToExamine = null;
        LabEquipment.isInAction = false;
        OnUseMicroscope?.Invoke(this, EventArgs.Empty);
    }
}
