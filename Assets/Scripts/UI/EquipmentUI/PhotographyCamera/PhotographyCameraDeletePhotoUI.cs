using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component of the photo gallery for when the player wishes to delete a photo.
 */
public class PhotographyCameraDeletePhotoUI : MonoBehaviour {

    [SerializeField] private PhotoGallery photoGallery;

    [SerializeField] private Button confirmRemovePictureButton;
    [SerializeField] private Button denyRemovePictureButton;

    private int indexToDelete;

    private void Awake() {
        confirmRemovePictureButton.onClick.AddListener(() => {
            photoGallery.ChangePhotoAfterRemovePhoto();
            Hide();
        });
        denyRemovePictureButton.onClick.AddListener(() => {
            Hide();
        });
    }

    private void Start() {
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }
}
