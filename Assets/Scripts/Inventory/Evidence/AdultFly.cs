using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdultFly : Flies {

    public static event EventHandler OnStepOnFly;

    public static void ResetStaticData() {
        OnStepOnFly = null;
    }
    private void OnCollisionEnter(Collision collision) {
        OnStepOnFly?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
    public override void Interact() {
        MessageLogManager.Instance.LogMessage("I should use something to collect this...");
    }


}
