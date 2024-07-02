using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EthanolContainer : EvidenceInteractingEquipment {

    [SerializeField] private GameObject megacephalaMaggotVisual;
    [SerializeField] private GameObject scalarisPupaVisual;

    private List<Larvae> larvaeKilled;

    private void Awake() {
        megacephalaMaggotVisual.SetActive(false);
        scalarisPupaVisual.SetActive(false);
    }

    private void Start() {
        larvaeKilled = EquipmentStorageManager.Instance.GetKilledMaggots();
        SetCorrectVisual();
    }
    public override void Interact() {
        if (larvaeKilled.Count == 0) {
            MessageLogManager.Instance.LogMessage("I need to preserve killed larvae and pupae in this.");
        } else {
            MessageLogManager.Instance.LogMessage("I need to examine the larvae and pupae in the laboratory.");
        }
    }


    private void SetCorrectVisual() {
        if (larvaeKilled != null) {
            foreach (Larvae maggot in larvaeKilled) {
                string maggotType = maggot.GetInventoryObjectSO().objectName;

                if (maggotType == "Long thin cylinder") {
                    scalarisPupaVisual.SetActive(true);
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
