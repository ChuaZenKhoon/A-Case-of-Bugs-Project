using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeceasedBody : Evidence {

    public event EventHandler OnStepOnDeadBody;

    public override void Interact() {
        MessageLogManager.Instance.LogMessage("This is the body of the deceased.");
    }

    private void OnCollisionEnter(Collision collision) {
       OnStepOnDeadBody?.Invoke(this, EventArgs.Empty);
      
    }
}
