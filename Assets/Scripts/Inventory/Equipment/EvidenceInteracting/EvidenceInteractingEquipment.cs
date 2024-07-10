/**
 * The sub class representing equipment that the player can use to 
 * interact with evidence. There is an emphasis on equipment identification
 * to track the relevant evidence collected with it.
 */
public class EvidenceInteractingEquipment : Equipment {

    private int equipmentID;

    public void SetEquipmentID(int equipmentID) {
        this.equipmentID = equipmentID;
    }

    public int GetEquipmentID() {
        return equipmentID;
    }
}
