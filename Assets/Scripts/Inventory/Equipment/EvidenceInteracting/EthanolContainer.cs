using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthanolContainer : EvidenceInteractingEquipment {

    [SerializeField] private GameObject megacephalaMaggotVisual;
    [SerializeField] private GameObject spinigeraMaggotVisual;

    private List<Larvae> larvaeKilled;

    private void Awake() {
        megacephalaMaggotVisual.SetActive(false);
        spinigeraMaggotVisual.SetActive(false);
    }

    private void Start() {
        larvaeKilled = EquipmentStorageManager.Instance.GetKilledMaggots();
        SetCorrectVisual();
    }
    public override void Interact() {
        if (larvaeKilled.Count == 0) {
            MessageLogManager.Instance.LogMessage("I need to preserve killed maggots in this.");
        } else {
            MessageLogManager.Instance.LogMessage("I need to examine the maggots in the laboratory.");
        }
    }


    private void SetCorrectVisual() {
        if (larvaeKilled != null) {
            foreach (Larvae maggot in larvaeKilled) {
                string maggotType = maggot.GetInventoryObjectSO().objectName;

                if (maggotType == "Long Thin Maggot") {
                    spinigeraMaggotVisual.SetActive(true);
                }

                if (maggotType == "Cylindrical Maggot") {
                    megacephalaMaggotVisual.SetActive(true);
                }
            }
        }
    }

    public List<Larvae> GetKilledLarvae() {
        return larvaeKilled;
    }

}
