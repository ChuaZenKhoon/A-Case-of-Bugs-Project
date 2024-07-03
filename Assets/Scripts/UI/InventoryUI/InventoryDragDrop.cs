using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * A logic component that handles the drag drop functionality of the inventory slots.
 */
public class InventoryDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    [SerializeField] private Image iconImage;
    [SerializeField] InventorySingleUI parent;

    //For drag drop function to identify
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private Transform originalParentPosition;
    
    private bool isDragging;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Gameplay Canvas").GetComponent<Canvas>();
    }

    //Drags the inventory slot sprite with left click drag only
    public void OnBeginDrag(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            isDragging = true;
        } else {
            return;
        }

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        originalParentPosition = rectTransform.parent;

    }

    //Handles the drag tracking of the sprite with the cursor
    public void OnDrag(PointerEventData eventData) {
        if (isDragging) {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        } else {
            return;
        }
    }

    //Resets the visual of the sprite back to original position after drag is complete
    public void OnEndDrag(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            isDragging = false;
        } else {
            return;
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        rectTransform.SetParent(originalParentPosition, false);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public Sprite GetIconSprite() {
        return this.iconImage.sprite;
    }

    //Player to drag item sprite in inventory slot
    public void SetIconSprite(Sprite sprite) {
        if (sprite != null) {
            this.iconImage.color = new Color(1, 1, 1, 1);
            iconImage.raycastTarget = true;
            this.iconImage.sprite = sprite;
        } else {
            this.iconImage.color = new Color(1, 1, 1, 0);
            iconImage.raycastTarget = false;
            this.iconImage.sprite = sprite;
        }
    }

    public InventorySingleUI GetParent() {
        return this.parent;
    }
}
