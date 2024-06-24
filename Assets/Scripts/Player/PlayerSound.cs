using UnityEngine;

/**
 * Controls the SFX of the Player. 
 */
public class PlayerSound : MonoBehaviour {

    private float footstepTimer;
    private float footstepResetTiming = 0.3f;

    private void Update() {
        HandlePlayerWalkingSFX();
    }


    /**
     * Plays the footstep sound at player's feet if player is walking.
     * Footstep timer introduced to space sound out appropriately.
     */
    private void HandlePlayerWalkingSFX() {

        if (Player.Instance.IsWalking()) {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0) {
                //Reset timer
                footstepTimer = footstepResetTiming;
                float volumeMultiplier = 0.6f;
                SoundManager.Instance.PlayFootstepSound(Player.Instance.transform.position, volumeMultiplier);
            }
        }
    }
}
