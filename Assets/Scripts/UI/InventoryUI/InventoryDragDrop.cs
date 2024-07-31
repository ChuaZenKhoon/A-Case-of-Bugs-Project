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

    public static bool IS_DRAGGING_CURRENTLY = false;

    public static void ResetStaticData() {
        IS_DRAGGING_CURRENTLY = false;
    }

    private static Color INVISIBLE_SPRITE = new Color(1, 1, 1, 0);
    private static Color VISIBLE_SPRITE = new Color(1, 1, 1, 1);

    [SerializeField] private Image iconImage;
    [SerializeField] InventorySingleUI parent;

    //For drag drop function to identify
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private Transform originalParentPosition;
   

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Gameplay Canvas").GetComponent<Canvas>();
    }

    //Drags the inventory slot sprite with left click drag only
    public void OnBeginDrag(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            IS_DRAGGING_CURRENTLY = true;
        } else {
            return;
        }

        //Adjust UI part to show drag action happening
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        originalParentPosition = rectTransform.parent;

    }

    //Handles the drag tracking of the sprite with the cursor
    public void OnDrag(PointerEventData eventData) {
        if (IS_DRAGGING_CURRENTLY) {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        } else {
            return;
        }
    }

    //Resets the visual of the sprite back to original position after drag is complete
    public void OnEndDrag(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            IS_DRAGGING_CURRENTLY = false;
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
            this.iconImage.color = VISIBLE_SPRITE;
            iconImage.raycastTarget = true;
            this.iconImage.sprite = sprite;
        } else {
            this.iconImage.color = INVISIBLE_SPRITE;
            iconImage.raycastTarget = false;
            this.iconImage.sprite = sprite;
        }
    }

    public InventorySingleUI GetParent() {
        return this.parent;
    }
}
