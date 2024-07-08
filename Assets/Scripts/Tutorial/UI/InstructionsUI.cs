using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionsUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI instructionText;

    private const string movementMessage = "Use the WASD keys to move, and move your mouse to look around!\n" + 
        "Go to the red arrows!";
    
    private const string inventoryMessage = "Use Q to open/close your inventory!\n" + 
        "You can left click drag around items between slots to swap them.\n" + 
        "The active slots are the top 5, which you can equip into your hand with 1/2/3/4/5.\n" +
        "Right click on items in the inventory to drop them! Equipment cannot be dropped.";
    
    private const string interactionMessage = "Use E to interact with certain objects when you're near them!\n" +
        "Interactable objects will be highlighted to you when you look at them.";

    public void DisplayMessage(TutorialLevelManager.State state) {
        switch (state) {
            case TutorialLevelManager.State.IntroMessage:
                break;
            case TutorialLevelManager.State.Movement:
                instructionText.text = movementMessage;
                break;
            case TutorialLevelManager.State.Inventory:
                instructionText.text = inventoryMessage;
                break;
            case TutorialLevelManager.State.Interaction:
                instructionText.text = interactionMessage;
                break;
            case TutorialLevelManager.State.Exit:
                instructionText.text = "";
                break;
            default:
                instructionText.text = "Try the equipment out!";
                break;

        }

    }

}
