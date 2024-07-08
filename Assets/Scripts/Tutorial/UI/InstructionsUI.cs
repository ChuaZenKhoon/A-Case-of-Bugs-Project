using TMPro;
using UnityEngine;

/**
 * A UI component representing the display to the player for instructions.
 */
public class InstructionsUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI instructionText;

    private const string movementMessage = "Use the WASD keys to move, and move your mouse to look around!\n" + 
        "Go to the red arrows!";
    
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
                instructionText.text = "Try using the inventory!";
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
