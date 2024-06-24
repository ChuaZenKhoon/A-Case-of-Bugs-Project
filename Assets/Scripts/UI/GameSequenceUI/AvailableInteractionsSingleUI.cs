using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AvailableInteractionsSingleUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI interactionKey;
    [SerializeField] private TextMeshProUGUI interactionText;

    public void SetUpInteraction(EquipmentSO.EquipmentInteraction equipmentInteraction) {
        interactionKey.text = equipmentInteraction.equipmentInteractionButtonText;
        interactionText.text = equipmentInteraction.equipmentInteractionDescriptionText;
    }
}
