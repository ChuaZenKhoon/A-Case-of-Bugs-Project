/**
 * The superclass representing any form of lab equipment that the player can interact with
 * when holding a item in the hand.
 */
public class LabEquipment : InteractableObject {

    public static bool isInAction = false;

    public static void ResetStaticData() {
        isInAction = false;
    }
    public override void Interact() {
        if (Player.Instance.GetHeldItem() == null) {
            MessageLogManager.Instance.LogMessage("There is nothing held to examine.");
        }
    }
}