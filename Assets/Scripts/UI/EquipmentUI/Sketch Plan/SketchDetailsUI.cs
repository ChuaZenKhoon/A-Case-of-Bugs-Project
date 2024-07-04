using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing the details screen of the sketch plan.
 */

public class SketchDetailsUI : MonoBehaviour {

    [SerializeField] private SketchDetails sketchDetails;

    [SerializeField] private Button backButton;
    [SerializeField] private TMP_InputField[] inputArray;
    [SerializeField] private RectTransform savedImageSpace;

    private void Awake() {
        backButton.onClick.AddListener(() => {
            SaveDetails();
        });
    }

    public void Hide() {

        gameObject.SetActive(false);
    }

    public void Show() {

        gameObject.SetActive(true);
    }

    public void UpdateDetails(string[] textArray) {
        for (int i = 0; i < textArray.Length; i++) {
            if (textArray[i] != null) {
                inputArray[i].text = textArray[i];
            }
        }

        Hide();
    }

    //Process UI information to pass to logic component for storage.
    private void SaveDetails() {
        string[] textArray = new string[6];

        for (int i = 0; i < inputArray.Length;i++) {
            textArray[i] = inputArray[i].text;
        }
 
        sketchDetails.SaveDetails(textArray);
    }
}

