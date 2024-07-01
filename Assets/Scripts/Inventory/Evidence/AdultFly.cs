using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdultFly : Flies {

    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
    public override void Interact() {
        MessageLogManager.Instance.LogMessage("I should use something to collect this...");
    }


}
