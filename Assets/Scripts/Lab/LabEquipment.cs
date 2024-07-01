using System.Collections;
using System.Collections.Generic;
using Unity.Play.Publisher.Editor;
using UnityEngine;

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