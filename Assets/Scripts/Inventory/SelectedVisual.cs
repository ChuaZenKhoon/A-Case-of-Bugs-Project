using UnityEngine;

/**
 * A logic component that handles highlighting the object to the player for interaction.
 */
public class SelectedVisual : MonoBehaviour {

    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private GameObject[] selectedVisualArray;

    private const int EVIDENCE_INDEX = 0;
    private const int SEALED_EVIDENCE_INDEX = 1;

    private void Start() {
        Player.Instance.OnPlayerStareAtInteractableObjectChange += Player_OnPlayerStareAtInventoryObjectChange;
    }

    private void Player_OnPlayerStareAtInventoryObjectChange(object sender, PlayerInteractor.OnPlayerStareAtInteractableObjectChangeEventArgs e) {
        if (e.interactableObject == interactableObject) {
            Show();
        } else {
            Hide();
        }
    }

    //Object stops listening when it is destroyed
    private void OnDestroy() {
        Player.Instance.OnPlayerStareAtInteractableObjectChange -= Player_OnPlayerStareAtInventoryObjectChange;
    }

    private void Show() {
        foreach (GameObject selectedVisual in selectedVisualArray) {
            selectedVisual.SetActive(true);
        }

        if (interactableObject is Evidence) {
            HandleEvidenceSealing();
        }
    }

    private void Hide() {
        foreach (GameObject selectedCounterVisual in selectedVisualArray) {
            selectedCounterVisual.SetActive(false);
        }
    }

    //If evidence is picked up, it is sealed and should highlight seal bag and not evidence.
    private void HandleEvidenceSealing() {
        Evidence evidence = interactableObject as Evidence;
        if (evidence.IsSealed()) {
            selectedVisualArray[EVIDENCE_INDEX].SetActive(false);
        } else {
            if (selectedVisualArray.Length > 1) {
                selectedVisualArray[SEALED_EVIDENCE_INDEX].SetActive(false);
            }
        }
    }
}
