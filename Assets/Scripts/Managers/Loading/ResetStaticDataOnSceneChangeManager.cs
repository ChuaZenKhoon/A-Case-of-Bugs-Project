using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * A logic Manager that handles resetting static data on scene loads to prevent unintended/lack of certain behaviour.
 */
public class ResetStaticDataOnSceneChangeManager : MonoBehaviour {

    public static ResetStaticDataOnSceneChangeManager Instance {  get; private set; }

    //Persistent Singleton
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        ResetData();
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
        ResetData();
    }


    //Resets all static data from classes that have them on their behalf
    private void ResetData() {
        InventoryDropItemUI.ResetStaticData();
        InventorySingleUI.ResetStaticData();
        Equipment.ResetStaticData();
        PlacardHolder.ResetStaticData();
        FingerprintDuster.ResetStaticData();
        NextLocationArrow.ResetStaticData();
        Phone.ResetStaticData();
        SketchPlan.ResetStaticData();
        LabSceneMessageUI.ResetStaticData();
        LabEquipment.ResetStaticData();
        Microscope.ResetStaticData();
        BloodTestStation.ResetStaticData();
        AdultFly.ResetStaticData();
    }
}
