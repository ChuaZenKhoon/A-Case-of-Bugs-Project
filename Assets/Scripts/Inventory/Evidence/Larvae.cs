using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Larvae : Flies {


    public override void Interact() {
        MessageLogManager.Instance.LogMessage("I need to kill the maggots somehow...");
    }


}
