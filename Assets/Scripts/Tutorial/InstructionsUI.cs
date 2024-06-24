using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionsUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI instructionText;

    private const string movementMessage = "Use WASD to move, and mouse movement to look around!";
    private const string inventoryMessage = "Use Q to open your inventory!\n" + 
        "You can left click drag around items to swap them.\n" + 
        "The active slots are the ones in blue, which you can equip into your hand with 1/2/3/4/5.\n" +
        "Right Click to drop items! Equipment cannot be dropped.";
    private const string interactionMessage = "Use E to interact with certain objects when you're near them!\n" +
        "Interactable objects will be highlighted to you when you look at them.";
    private const string selfInteractingMessage = "A few equipment have been added to your inventory. Play around with them!";
    private const string evidenceInteractingMessage = "The next set of items allow you to collect certain evidence!\n" + 
        "Test to see which you can interact with!";


    public void DisplayMessage(TutorialLevelManager.State state) {
        switch (state) {
            case TutorialLevelManager.State.Movement:
                instructionText.text = movementMessage;
                break;
            case TutorialLevelManager.State.Inventory:
                instructionText.text = inventoryMessage;
                break;
            case TutorialLevelManager.State.Interaction:
                instructionText.text = interactionMessage;
                break;
            case TutorialLevelManager.State.SelfInteracting:
                instructionText.text = selfInteractingMessage;
                break;
            case TutorialLevelManager.State.EvidenceInteracting:
                instructionText.text = evidenceInteractingMessage;
                break;
            default:
                instructionText.text = "";
                break;

        }

    }

}
