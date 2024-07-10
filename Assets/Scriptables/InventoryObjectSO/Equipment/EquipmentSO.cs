using System;
using System.Collections.Generic;
using UnityEngine;


/**
 * A scriptable object representing the information that an equipment should have.
 */
[CreateAssetMenu()]
public class EquipmentSO : InventoryObjectSO {

    [Serializable]
    public struct EquipmentInteraction {
        public string equipmentInteractionButtonText;
        public string equipmentInteractionDescriptionText;
    }

    public List<EquipmentInteraction> interactions;

    //Method to change interaction text to update to players.
    public void ChangeInteractionText(string text, int index) {
        EquipmentInteraction equipmentInteraction = interactions[index];
        equipmentInteraction.equipmentInteractionDescriptionText = text;
        interactions[index] = equipmentInteraction;
    }
}
