using System;
using UnityEngine;

/**
 * The class representing evidence in the form of a fingerprint that is invisible at first.
 */
public class Fingerprint : Evidence {

    public static event EventHandler OnImproperFingerprintCollection;

    public static void ResetStaticData() {
        OnImproperFingerprintCollection = null;
    }


    [SerializeField] private GameObject visual;
    [SerializeField] private SelectedVisual selectedVisual;

    private const string HIDDEN_LAYER_NAME = "hiddenFingerprintsLayer";
    private const string VISIBLE_LAYER_NAME = "interactableObjectLayer";


    private void Awake() {
        visual.SetActive(false);
        selectedVisual.gameObject.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer(HIDDEN_LAYER_NAME);
    }

    /**
     * Upon being dusted by the fingerprint duster, is revealed for collection.
     */
    public void RevealSelf() {
        visual.SetActive(true);
        selectedVisual.gameObject.SetActive(true);
        gameObject.layer = LayerMask.NameToLayer(VISIBLE_LAYER_NAME);
    }

    //For scoring
    public override void Interact() {
        OnImproperFingerprintCollection?.Invoke(this, EventArgs.Empty);
        Destroy(this.gameObject);
    }


}
