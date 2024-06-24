using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableInteractionsUI : MonoBehaviour {

    [SerializeField] private Transform container;
    [SerializeField] private Transform template;

    private void Start () {
        AvailableInteractionsManager.Instance.OnEquipmentHold += AvailableInteractionsManager_OnEquipmentHold;
        template.gameObject.SetActive(false);
    }

    private void AvailableInteractionsManager_OnEquipmentHold(object sender, EquipmentSO e) {
        UpdateVisual(e);
    }

    private void UpdateVisual(EquipmentSO equipmentSO) {
        foreach (Transform child in container) {
            if (child == template) {
                continue;
            }
            Destroy(child.gameObject);
        } 

        if (equipmentSO == null) {
            return;
        }

        List<EquipmentSO.EquipmentInteraction> equipmentInteractions = equipmentSO.interactions;
        if (equipmentInteractions.Count % 2 == 0) {
            container.GetComponent<HorizontalLayoutGroup>().padding.left = -100;
        } else {
            container.GetComponent<HorizontalLayoutGroup>().padding.left = 0;
        }

        foreach (EquipmentSO.EquipmentInteraction equipmentInteraction in equipmentInteractions) {
            Transform singleInteraction = Instantiate(template, container);
            singleInteraction.gameObject.SetActive(true);
            singleInteraction.GetComponent<AvailableInteractionsSingleUI>().SetUpInteraction(equipmentInteraction);
        }
    }

}
