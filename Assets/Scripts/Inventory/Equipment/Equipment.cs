/**
 * A sub class of the inventory object, referring to the equipment to be used in the game.  
 */
public class Equipment : InventoryObject {

    public static bool isInAction = false;

    public static void ResetStaticData() {
        isInAction = false;
    }

}
