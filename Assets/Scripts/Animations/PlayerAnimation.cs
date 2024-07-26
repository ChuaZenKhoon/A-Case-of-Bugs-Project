using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : CustomAnimation {

    private const string EQUIP_ITEM_ANIMATION_BOOL = "isEquippingItem";
    private const string EQUIP_ITEM_ANIMATION_NAME = "EquipItem";

    private Coroutine equipItemCoroutine;

    private void Awake() {
        equipItemCoroutine = null;
    }

    private void Start() {
        InventoryManager.Instance.OnEquipItem += Instance_OnEquipItem;
    }

    private void Instance_OnEquipItem(object sender, System.EventArgs e) {
        if (equipItemCoroutine != null) {
            StopCoroutine(equipItemCoroutine);
        }

        animator.Play(EQUIP_ITEM_ANIMATION_NAME, 0, 0f);
        animator.SetBool(EQUIP_ITEM_ANIMATION_BOOL, true);
        equipItemCoroutine = StartCoroutine(EquipItemCoroutine());
    }

    private IEnumerator EquipItemCoroutine() {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool(EQUIP_ITEM_ANIMATION_BOOL, false);
        equipItemCoroutine = null;
    }
}
