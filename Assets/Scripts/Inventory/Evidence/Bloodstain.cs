using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodstain : Evidence {
    public override void Interact() {
        MessageLogManager.Instance.LogMessage("Unable to pick up red liquid with bare hands.");
    }

}
