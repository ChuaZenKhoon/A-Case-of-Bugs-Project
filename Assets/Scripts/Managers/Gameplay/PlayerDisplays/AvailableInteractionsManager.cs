using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A manager that handles the interaction buttons that the player can press based on what item is held.
 */
public class AvailableInteractionsManager : MonoBehaviour {

    public static AvailableInteractionsManager Instance { get; private set; }

    [SerializeField] private AvailableInteractionsUI availableInteractionsUI;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Player.Instance.OnUpdateHeldItemToEquipment += Player_OnUpdateHeldItemToEquipment;
        
        //Individual events where description is changed
        HotWaterCup.OnChangeInteractActionDetails += HotWaterCup_OnChangeInteractActionDetails;
        FlyNet.OnChangeInteractActionDetails += FlyNet_OnChangeInteractActionDetails;
        SketchPlan.OnChangeInteractActionDetails += SketchPlan_OnChangeInteractActionDetails;
        MeasuringTool.OnChangeInteractActionDetails += MeasuringTool_OnChangeInteractActionDetails;
        Phone.OnChangeInteractActionDetails += Phone_OnChangeInteractActionDetails;
    }

    /**
     * Dissects the interactions needed to be updated and passes the info to the UI to put up.
     * 
     * @param equipmentSO The equipment involved that has interactions to work with.
     */
    private void UpdateAvailableInteractions(EquipmentSO equipmentSO) {  
        availableInteractionsUI.ClearInteractions();

        if (equipmentSO != null) {
            List<EquipmentSO.EquipmentInteraction> interactions = equipmentSO.interactions;
            if (interactions.Count <= 0) {
                return;
            } else {
                foreach (EquipmentSO.EquipmentInteraction interaction in interactions) {
                    availableInteractionsUI.AddInteraction(interaction.equipmentInteractionButtonText,
                        interaction.equipmentInteractionDescriptionText);
                }
            }
        }
    }


    private void SketchPlan_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        UpdateAvailableInteractions(e);
    }

    private void Phone_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        UpdateAvailableInteractions(e);
    }

    private void MeasuringTool_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        UpdateAvailableInteractions(e);
    }

    private void FlyNet_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        UpdateAvailableInteractions(e);
    }

    private void HotWaterCup_OnChangeInteractActionDetails(object sender, EquipmentSO e) {
        UpdateAvailableInteractions(e);
    }

    private void Player_OnUpdateHeldItemToEquipment(object sender, EquipmentSO e) {
        UpdateAvailableInteractions(e);
    }

}
