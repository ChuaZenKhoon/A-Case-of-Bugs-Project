using UnityEngine;
using UnityEngine.EventSystems;

public class InformationHoverSFX : MonoBehaviour, IPointerEnterHandler {

    private float volMultiplier = 0.8f;
    public void OnPointerEnter(PointerEventData eventData) {
        SFXPlayer.Instance.PlayInformationHoverSound(volMultiplier);
    }
}




