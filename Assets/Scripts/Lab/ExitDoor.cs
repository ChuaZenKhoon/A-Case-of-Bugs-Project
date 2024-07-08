using System;
using UnityEngine;

/**
 * The class representing the exit game mechanism when
 * the player is done with the lab examination.
 */
public class ExitDoor : MonoBehaviour {

    public event EventHandler OnExitLab;

    //Event to signal to level manager that game is to exit.
    private void OnCollisionEnter(Collision collision) {
        OnExitLab?.Invoke(this, EventArgs.Empty);
    }
}
