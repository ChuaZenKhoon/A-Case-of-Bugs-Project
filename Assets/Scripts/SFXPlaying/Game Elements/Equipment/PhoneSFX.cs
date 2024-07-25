/**
 * A component of the Phone that handles its open close sound effect.
 */
public class PhoneSFX : SFX {
    private void Awake() {
        volMultiplier = 1f;
    }

    private void Start() {
        Phone.OnPhoneOpen += Phone_OnPhoneOpen;
    }

    private void Phone_OnPhoneOpen(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayPhoneOpenCloseSound(Player.Instance.GetHoldPosition().position, volMultiplier);
    }
}
