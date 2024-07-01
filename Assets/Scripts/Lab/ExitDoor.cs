using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {

    public event EventHandler OnExitLab;
    private void OnCollisionEnter(Collision collision) {
        OnExitLab?.Invoke(this, EventArgs.Empty);
    }
}
