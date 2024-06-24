using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SketchPlanUI : MonoBehaviour {

    public static event EventHandler OnOpenSketchPlan;

    [SerializeField] private SketchDrawSpaceUI drawSpaceUI;
    [SerializeField] private SketchDetailsUI detailsUI;
    [SerializeField] private SketchLegendUI legendUI;

    [SerializeField] private Button drawSpaceButton;
    [SerializeField] private Button detailsButton;
    [SerializeField] private Button legendButton;

    [SerializeField] private Image sketchPlanImage;

    public static bool isInSketchMode;
    private static bool isShown;

    private void Awake() {

        isInSketchMode = false;
        isShown = false;

        drawSpaceButton.onClick.AddListener(() => {
            drawSpaceUI.Show();
        });
        detailsButton.onClick.AddListener(() => {
            detailsUI.Show();
        });
        legendButton.onClick.AddListener(() => {
            legendUI.Show();
        });

        UpdateSketchImages();

    }

    public static bool IsShown() {
        return isShown;
    }

    private void Start() {
        Hide();
    }

    public void Hide() {
        gameObject.SetActive(false);
        isShown = false;
        OnOpenSketchPlan?.Invoke(this, EventArgs.Empty);
    }

    public void Show() {
        gameObject.SetActive(true);
        isShown = true;
        UpdateSketchImages();
        OnOpenSketchPlan?.Invoke(this, EventArgs.Empty);
    }

    public void SaveSketch() {
        Hide();
    }

    public void UpdateSketchImages() {
        Sprite savedImage = EquipmentStorageManager.Instance.GetSavedSketchImages();

        if (savedImage != null) {
            sketchPlanImage.sprite = savedImage;
        }
    }

}
