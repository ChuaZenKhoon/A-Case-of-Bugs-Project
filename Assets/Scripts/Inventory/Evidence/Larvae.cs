/**
 * The class representing evidence in the form of larvae and pupae(group together due to same behaviour)
 */
public class Larvae : Flies {
    public override void Interact() {
        MessageLogManager.Instance.LogMessage("I need to kill the maggots somehow...");
    }

}
