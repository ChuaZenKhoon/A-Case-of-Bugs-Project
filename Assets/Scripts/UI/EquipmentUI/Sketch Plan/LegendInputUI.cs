using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LegendInputUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI legendInputIndexText;
    [SerializeField] private TMP_InputField legendInputWordsText;

    public void UpdateText(string text, int index) {
        if (text != null) { 
        legendInputWordsText.text = text;
        }
        legendInputIndexText.text = index.ToString() + ":";
    }

    public string GetText() {
        return legendInputWordsText.text;
    }
}
