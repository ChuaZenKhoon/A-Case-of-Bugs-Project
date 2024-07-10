using UnityEngine;

/**
 * A super class representing all objects in the game that can be interacted with by the player.
 */
public class InteractableObject : MonoBehaviour {

    //Individual unique behaviour for each object when interacted with by player.
    public virtual void Interact() { }
}
