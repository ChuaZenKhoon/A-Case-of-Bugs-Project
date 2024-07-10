using System;
using UnityEngine;

/**
 * The class representing evidence in the form of adult flies.
 */
public class AdultFly : Flies {

    public static event EventHandler OnStepOnFly;

    public static void ResetStaticData() {
        OnStepOnFly = null;
    }

    //For scoring
    private void OnCollisionEnter(Collision collision) {
        OnStepOnFly?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
    public override void Interact() {
        MessageLogManager.Instance.LogMessage("I should use something to collect this...");
    }
}
