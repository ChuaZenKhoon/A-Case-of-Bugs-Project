using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A logic component that is used to reset static data on scene loads.
 */
public class ResetStaticDataOnSceneChange : MonoBehaviour {

    private void Awake() {
        InventoryDropItemUI.ResetStaticData();
        InventorySingleUI.ResetStaticData();
        Equipment.ResetStaticData();
        PlacardHolder.ResetStaticData();
        FingerprintDuster.ResetStaticData();
        NextLocationArrow.ResetStaticData();
        Phone.ResetStaticData();
        GameInstructionsUI.ResetStaticData();
        SketchPlan.ResetStaticData();
        LabSceneMessageUI.ResetStaticData();
        LabEquipment.ResetStaticData();
        Microscope.ResetStaticData();
    }
}
