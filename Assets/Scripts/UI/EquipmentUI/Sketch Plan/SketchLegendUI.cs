using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing the legend screen of the sketch plan.
 */
public class SketchLegendUI : MonoBehaviour {

    [SerializeField] private SketchLegend sketchLegend;

    [SerializeField] private Button addLegendButton;
    [SerializeField] private Button removeLegendButton;
    [SerializeField] private Button backButton;
    [SerializeField] private LegendInputUI legendInputUIPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private RectTransform savedImageSpace;

    private const int MAX_SLOTS = 12;

    private void Awake() {
        addLegendButton.onClick.AddListener(() => {
            sketchLegend.AddLegendInput(null);
        });

        removeLegendButton.onClick.AddListener(() => {
            sketchLegend.RemoveLegendInput();
        });

        backButton.onClick.AddListener(() => {
            SaveDetails();
        });

        //Logic component does not tell UI for this specific point.
        UpdateButtons(0);
    }

    private void Start() {
        Hide();
    }
    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    /**
     * Adds a new tab for input.
     * 
     * @param legendInputText Any stored string from before.
     * @param index The index of the tab to be added.
     */
    public void AddLegendInput(string legendInputText, int index) {
        LegendInputUI newLegendInputUI = Instantiate(legendInputUIPrefab, container);
        
        if (legendInputText != null) {
            newLegendInputUI.UpdateText(legendInputText, index+1);
        } else {
            newLegendInputUI.UpdateText(null, index+1);
        }
        UpdateButtons(index+1);
    }

    /**
     * Removes the latest tab.
     *
     * @param index The index of the tab to be removed.
     */
    public void RemoveLegendInput(int index) {
        Transform child = container.GetChild(container.childCount - 1);
        Destroy(child.gameObject);
        UpdateButtons(index);
    }

    /**
     * A sanity check to prevent UI overspill.
     * 
     * @param index Number of tabs currently in the UI to check against.
     */
    public void UpdateButtons(int index) {
        addLegendButton.interactable = index < MAX_SLOTS;
        removeLegendButton.interactable = index > 0;
    }

    //Processes information before passing to logic component for storage.
    private void SaveDetails() {
        List<string> details = new List<string>();

        foreach (Transform child in container) {
            LegendInputUI legendInputUI = child.GetComponent<LegendInputUI>();
            if (legendInputUI != null) {
                details.Add(legendInputUI.GetText());
            }
        }

        sketchLegend.SaveDetails(details);
    }
}
