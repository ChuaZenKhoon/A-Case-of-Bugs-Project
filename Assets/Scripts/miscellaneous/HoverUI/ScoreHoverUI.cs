using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScoreHoverUI : MonoBehaviour, IHoverUI {

    [SerializeField] private Image informationText;

    public void OnPointerExit(PointerEventData eventData) {
        Hide();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Show();
    }

    private void Hide() {
        informationText.gameObject.SetActive(false);
    }

    private void Show() {
        informationText.gameObject.SetActive(true); 
    }
}

