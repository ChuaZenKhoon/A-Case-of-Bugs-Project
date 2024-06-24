using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotographyCameraDeletePhotoUI : MonoBehaviour {

    [SerializeField] private PhotographyCameraUI photographyCameraUI;

    [SerializeField] private Button confirmRemovePictureButton;
    [SerializeField] private Button denyRemovePictureButton;

    private int indexToDelete;

    private void Awake() {
        confirmRemovePictureButton.onClick.AddListener(() => {
            EquipmentStorageManager.Instance.RemovePhotoFromPhotoGallery(indexToDelete);
            photographyCameraUI.ChangePhotoAfterRemovePhoto();
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

    public void Show(int index) {
        indexToDelete = index;
        gameObject.SetActive(true);
    }
}
