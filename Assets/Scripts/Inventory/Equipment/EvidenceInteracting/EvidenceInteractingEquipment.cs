using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceInteractingEquipment : Equipment {

    private int equipmentID;

    public void SetEquipmentID(int equipmentID) {
        this.equipmentID = equipmentID;
    }

    public int GetEquipmentID() {
        return equipmentID;
    }
}
