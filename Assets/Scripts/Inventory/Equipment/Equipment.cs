using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : InventoryObject {

    public static bool isInAction = false;

    public static void ResetStaticData() {
        isInAction = false;
    }
    public override void Interact() {
        //Use Equipment
        Debug.Log("Equipment used!");
    }

}
