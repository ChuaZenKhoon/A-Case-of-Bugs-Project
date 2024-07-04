using TMPro;
using UnityEngine;

/**
 * A UI component representing a singular tab in the legend screen in the sketch plan.
 */
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
