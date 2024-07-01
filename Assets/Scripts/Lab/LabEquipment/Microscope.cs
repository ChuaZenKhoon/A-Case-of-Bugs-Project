using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

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

    private List<Sprite> currentImagesOfExamined;
    private int currentImageIndex;

    private string currentItemName;
    public override void Interact() {
        InventoryObject playerHeldItem = Player.Instance.GetHeldItem();

        if (!(playerHeldItem is Container || playerHeldItem is AcetoneKillJar || playerHeldItem is EthanolContainer)) {
            MessageLogManager.Instance.LogMessage("Unable to examine contents of held item with the microscope.");
        } else {
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
    }

    private void ExamineDeadFlies(Container container) {
        fliesToExamine = container.GetDeadFlies();

        if (fliesToExamine.Count > 0) {
            currentFlyExamined = fliesToExamine[0];
            currentItemIndex = 0;
            currentImagesOfExamined = currentFlyExamined.GetSpritesForExamination();
            currentItemName = currentFlyExamined.GetInventoryObjectSO().name;
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

    private void ExamineKilledFlies(AcetoneKillJar acetoneKillJar) {
        fliesToExamine = acetoneKillJar.GetKilledFlies();

        if (fliesToExamine.Count > 0) { 
            currentFlyExamined = fliesToExamine[0];
            currentItemIndex = 0;
            currentImagesOfExamined = currentFlyExamined.GetSpritesForExamination();
            currentItemName = currentFlyExamined.GetInventoryObjectSO().name;
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

    private void ExamineLarvae(EthanolContainer ethanolContainer) {
        larvaeToExamine = ethanolContainer.GetKilledLarvae();

        if (larvaeToExamine.Count > 0) {
            currentLarvaExamined = larvaeToExamine[0];
            currentItemIndex = 0;
            currentImagesOfExamined = currentLarvaExamined.GetSpritesForExamination();
            currentItemName = currentLarvaExamined.GetInventoryObjectSO().name;
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

    public void NextItemToExamine() {
        if (fliesToExamine != null) {
            int nextItemNumber = (currentItemIndex + 1) % fliesToExamine.Count;
            currentItemIndex = nextItemNumber;
            currentFlyExamined = fliesToExamine[currentItemIndex];
            currentImagesOfExamined = currentFlyExamined.GetSpritesForExamination();
            currentItemName = currentFlyExamined.GetInventoryObjectSO().name;
            currentImageIndex = 0;
            microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
            microscopeUI.UpdateText(currentItemName);
        } else if (larvaeToExamine != null) {
            int nextItemNumber = (currentItemIndex + 1) % larvaeToExamine.Count;
            currentItemIndex = nextItemNumber;
            currentLarvaExamined = larvaeToExamine[currentItemIndex];
            currentImagesOfExamined = currentLarvaExamined.GetSpritesForExamination();
            currentItemName = currentLarvaExamined.GetInventoryObjectSO().name;
            currentImageIndex = 0;
            microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
            microscopeUI.UpdateText(currentItemName);
        }
    }

    public void PreviousItemToExamine() {
        if (fliesToExamine != null) {
            int previousItemNumber = (currentItemIndex - 1 + fliesToExamine.Count) % fliesToExamine.Count;
            currentItemIndex = previousItemNumber;
            currentFlyExamined = fliesToExamine[currentItemIndex];
            currentImagesOfExamined = currentFlyExamined.GetSpritesForExamination();
            currentItemName = currentFlyExamined.GetInventoryObjectSO().name;
            currentImageIndex = 0;
            microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
            microscopeUI.UpdateText(currentItemName);
        } else if (larvaeToExamine != null) {
            int previousItemNumber = (currentItemIndex - 1 + larvaeToExamine.Count) % larvaeToExamine.Count;
            currentItemIndex = previousItemNumber;
            currentLarvaExamined = larvaeToExamine[currentItemIndex];
            currentImagesOfExamined = currentLarvaExamined.GetSpritesForExamination();
            currentItemName = currentLarvaExamined.GetInventoryObjectSO().name;
            currentImageIndex = 0;
            microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
            microscopeUI.UpdateText(currentItemName);
        }
    }

    public void NextPhotograph() {
        int nextPhotographNumber = (currentImageIndex + 1) % currentImagesOfExamined.Count;
        currentImageIndex = nextPhotographNumber;
        microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
    }

    public void PreviousPhotograph() {
        int previousPhotographNumber = (currentImageIndex - 1 + currentImagesOfExamined.Count) % currentImagesOfExamined.Count;
        currentImageIndex = previousPhotographNumber;
        microscopeUI.UpdateImage(currentImagesOfExamined[currentImageIndex]);
    }

    public void ClearItems() {
        fliesToExamine = null;
        larvaeToExamine = null;
        LabEquipment.isInAction = false;
        OnUseMicroscope?.Invoke(this, EventArgs.Empty);

    }
}
