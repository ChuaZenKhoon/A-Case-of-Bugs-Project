using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The logic component of the legend screen in the sketch plan.
 */
public class SketchLegend : MonoBehaviour {

    [SerializeField] private SketchPlan sketchPlan;

    [SerializeField] private SketchLegendUI sketchLegendUI;

    private List<string> savedLegendInputTextList;
    private int index;

    private void Awake() {
        index = 0;
    }

    private void Start() {
        savedLegendInputTextList = EquipmentStorageManager.Instance.GetSketchPlanSavedLegendInputTextList();

        UpdateLegend();
    }

    //For first load up
    private void UpdateLegend() {
        if (savedLegendInputTextList.Count != 0) {
            foreach (string legendInputText in savedLegendInputTextList) {
                sketchLegendUI.AddLegendInput(legendInputText, index);
                index++;
            }
        }
    }

    public void AddLegendInput(string legendInputText) {
        sketchLegendUI.AddLegendInput(legendInputText, index);
        index++;
    }

    public void RemoveLegendInput() {
        index--;
        sketchLegendUI.RemoveLegendInput(index);
    }

    public void Show() {
        sketchPlan.SetSketchMode(true);
        sketchLegendUI.Show();
    }

    public void Hide() {
        sketchPlan.SetSketchMode(false);
        sketchLegendUI.Hide();
    }

    /**
     * Transfer processed data to storage
     */
    public void SaveDetails(List<string> legendTextArray) {
        EquipmentStorageManager.Instance.ClearSketchPlanLegendInputUITextList();
        foreach (string legendText in legendTextArray) {
            EquipmentStorageManager.Instance.AddSketchPlanLegendInputUI(legendText);
        }

        Hide();
    }
}
