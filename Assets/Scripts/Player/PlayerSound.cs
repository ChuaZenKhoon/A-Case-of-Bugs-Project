using UnityEngine;

/**
 * A component of the Player that handles its SFX.
 */
public class PlayerSound : MonoBehaviour {

    [SerializeField] private PlayerMovement playerMovement;

    private float footstepTimer;
    private float footstepResetTiming = 0.3f;

    /**
     * Plays the footstep sound at player's feet if player is walking.
     * Footstep timer introduced to space sound out appropriately.
     */
    public void HandlePlayerWalkingSFX() {

        if (playerMovement.IsWalking()) {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0) {
                //Reset timer
                footstepTimer = footstepResetTiming;
                float volumeMultiplier = 0.6f;
                GameElementSFXPlayer.Instance.PlayFootstepSound(Player.Instance.transform.position, volumeMultiplier);
            }
        }
    }
}
