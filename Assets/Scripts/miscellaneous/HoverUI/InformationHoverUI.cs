using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * A UI component that represents a pop up containing information when another UI element is hovered over.
 */
public class InformationHoverUI : MonoBehaviour, IHoverUI {

    [SerializeField] private Image informationText;

    private void Start() {
        Hide();
    }

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

