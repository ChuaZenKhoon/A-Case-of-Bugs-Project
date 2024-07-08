using System;
using UnityEngine;

/**
 * A logic component of the tutorial stage that triggers the following
 * parts of the tutorial when the players moves over it.
 */
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
