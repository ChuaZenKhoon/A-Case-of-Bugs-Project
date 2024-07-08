/**
 * The class representing evidence in the form of a bloodstain that can be sampled by a swab.
 */
public class Bloodstain : Evidence {
    public override void Interact() {
        MessageLogManager.Instance.LogMessage("Unable to pick up red liquid with bare hands.");
    }

}
