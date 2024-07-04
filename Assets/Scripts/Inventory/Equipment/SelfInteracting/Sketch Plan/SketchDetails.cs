using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The logic component of the details section of the sketch plan.
 */
public class SketchDetails : MonoBehaviour {

    [SerializeField] private SketchPlan sketchPlan;

    [SerializeField] private SketchDetailsUI sketchDetailsUI;

    private string[] textArray;

    private void Start() {
        textArray = EquipmentStorageManager.Instance.GetSketchPlanSavedDetailsTextList();
        sketchDetailsUI.UpdateDetails(textArray);       
    }

    public void Show() {
        sketchPlan.SetSketchMode(true);
        sketchDetailsUI.Show();
    }

    public void Hide() {
        sketchPlan.SetSketchMode(false);
        sketchDetailsUI.Hide();
    }

    /**
     * Transfer processed data to storage
     */
    public void SaveDetails(string[] textArray) {
        for (int i = 0; i < textArray.Length; i++) {
            if (textArray[i] != null) {
                EquipmentStorageManager.Instance.AddSketchPlanDetailsUI(textArray[i], i);
            }
        }

        Hide();
    }
}
