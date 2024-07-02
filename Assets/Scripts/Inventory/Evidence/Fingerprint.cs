using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void RevealSelf() {
        visual.SetActive(true);
        selectedVisual.gameObject.SetActive(true);
        gameObject.layer = LayerMask.NameToLayer(VISIBLE_LAYER_NAME);
    }

    public override void Interact() {
        OnImproperFingerprintCollection?.Invoke(this, EventArgs.Empty);
        Destroy(this.gameObject);
    }


}
