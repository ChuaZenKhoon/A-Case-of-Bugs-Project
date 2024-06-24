using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdultFly : Evidence {


    public override void Interact() {
        MessageLogManager.Instance.LogMessage("I should use something to collect this...");
    }
}
