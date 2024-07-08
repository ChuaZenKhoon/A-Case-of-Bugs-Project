using System;
using UnityEngine;

/**
 * The class representing evidence in the form of the deceased's body.
 */
public class DeceasedBody : Evidence {

    public event EventHandler OnStepOnDeadBody;

    public override void Interact() {
        MessageLogManager.Instance.LogMessage("This is the body of the deceased.");
    }

    //For scoring
    private void OnCollisionEnter(Collision collision) {
       OnStepOnDeadBody?.Invoke(this, EventArgs.Empty);
    }
}
