using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EquipmentSO : InventoryObjectSO {

    [Serializable]
    public struct EquipmentInteraction {
        public string equipmentInteractionButtonText;
        public string equipmentInteractionDescriptionText;
    }

    public List<EquipmentInteraction> interactions;

    public void ChangeInteractionText(string text, int index) {
        EquipmentInteraction equipmentInteraction = interactions[index];
        equipmentInteraction.equipmentInteractionDescriptionText = text;
        interactions[index] = equipmentInteraction;
    }
}
