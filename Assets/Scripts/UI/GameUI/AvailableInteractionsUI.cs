using UnityEngine;
/**
 * A UI element representing the available keys displayed to the player that can be pressed for an interaction.
 */
public class AvailableInteractionsUI : MonoBehaviour {

    [SerializeField] private Transform container;
    [SerializeField] private Transform template;

    private void Start () {
        template.gameObject.SetActive(false);
    }

    //Using a template format, each interaction is created from the template and updated.

    /**
     * Interactions from before are to be cleared first before update.
     */
    public void ClearInteractions() {
        foreach (Transform child in container) {
            if (child == template) {
                continue;
            }
            Destroy(child.gameObject);
        }
    }

    /**
     * Every addition of an interaction creates a single UI using the template.
     */
    public void AddInteraction(string buttonText, string descriptionText) {
        Transform singleInteraction = Instantiate(template, container);
        singleInteraction.gameObject.SetActive(true);
        singleInteraction.GetComponent<AvailableInteractionsSingleUI>().SetUpInteraction(buttonText, descriptionText);
    }

}
