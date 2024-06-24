using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextLocationArrow : MonoBehaviour {

    public static event EventHandler<int> OnTouchArrow;

    [SerializeField] private int index;

    public static void ResetStaticData() {
        OnTouchArrow = null;
    }
    private void OnCollisionEnter(Collision collision) {
        OnTouchArrow?.Invoke(this, this.index);
        gameObject.SetActive(false);
    }
}
