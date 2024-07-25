using UnityEngine;

/**
 * A component of the placard holder that handles its sound effects.
 */
public class PlacardSFX : SFX {

    [SerializeField] private PlacardHolder placardHolder;

    private void Awake() {
        volMultiplier = 1f;
    }

    private void Start() {
        placardHolder.OnPickup += PlacardHolder_OnPickup;
        placardHolder.OnPutDown += PlacardHolder_OnPutDown;
    }

    private void PlacardHolder_OnPutDown(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayPlacardPutDownSound(Player.Instance.GetStareAtPosition(), volMultiplier);
    }

    private void PlacardHolder_OnPickup(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayPlacardPickUpSound(Player.Instance.GetStareAtPosition(), volMultiplier);
    }
}
