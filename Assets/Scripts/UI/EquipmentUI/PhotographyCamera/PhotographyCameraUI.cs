using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotographyCameraUI : MonoBehaviour {

    public static event EventHandler OnOpenPhotoGallery;
    private static bool isShown;

    [SerializeField] PhotographyCamera photographyCamera;

    [SerializeField] RawImage photographImage;

    [SerializeField] private Button backButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button removePictureButton;
    [SerializeField] private TextMeshProUGUI photographText;

    [SerializeField] private PhotographyCameraDeletePhotoUI photographyCameraDeletePhotoUI;

    private int nextPhotographNumber;
    private int previousPhotographNumber;
    private int currentPhotographNumber;
    private RenderTexture currentPhotograph;

    

    private void Awake() {
        nextButton.onClick.AddListener(() => {
            NextPhotograph(EquipmentStorageManager.Instance.GetPhotoGallery());
        });
        backButton.onClick.AddListener(() => {
            PreviousPhotograph(EquipmentStorageManager.Instance.GetPhotoGallery());
        });
        removePictureButton.onClick.AddListener(() => {
            photographyCameraDeletePhotoUI.Show(currentPhotographNumber);
        });

        isShown = false;

        GameInput.Instance.OnInteract2Action += GameInput_OnInteract2Action;
    }

    private void Start() {
        List<RenderTexture> photoGallery = EquipmentStorageManager.Instance.GetPhotoGallery();
        if (photoGallery.Count > 0) {
            currentPhotograph = photoGallery.First();
            UpdatePhotographSequence(photoGallery);
        }
        Hide();
    }

    private void OnDestroy() {
        GameInput.Instance.OnInteract2Action -= GameInput_OnInteract2Action;
    }

    private void GameInput_OnInteract2Action(object sender, System.EventArgs e) {
        if (photographyCamera.IsInCameraMode()) {
            MessageLogManager.Instance.LogMessage("Please exit camera mode first before viewing the photo gallery.");
            return;
        }

        if (InventoryManager.Instance.IsInventoryOpen()) {
            MessageLogManager.Instance.LogMessage("Close Inventory first before viewing photo gallery.");
            return;
        }

        if (!isShown) {
            if (EquipmentStorageManager.Instance.GetPhotoGallery().Count > 0) {
                ViewPhotoGallery();
                
            } else {
                MessageLogManager.Instance.LogMessage("There are no photographs available for viewing.");
            }
        } else {
            ClosePhotoGallery();
        }
    }
    private void ViewPhotoGallery() {
        Equipment.isInAction = true;
        Show();
        OnOpenPhotoGallery?.Invoke(this, EventArgs.Empty);
    }
    private void ClosePhotoGallery() {
        Equipment.isInAction = false;
        Hide();
        OnOpenPhotoGallery?.Invoke(this, EventArgs.Empty);
    }
    private void Show() {
        List<RenderTexture> photoGallery = EquipmentStorageManager.Instance.GetPhotoGallery();
        //First show
        if (currentPhotograph == null) {
            currentPhotograph = photoGallery.First();
        }

        UpdatePhotographSequence(photoGallery);

        photographImage.texture = currentPhotograph;
        isShown = true;
        gameObject.SetActive(true);
    }

    private void Hide() {
        isShown = false;
        gameObject.SetActive(false);
    }

    private void UpdatePhotographSequence(List<RenderTexture> photoGallery) {
        currentPhotographNumber = photoGallery.IndexOf(currentPhotograph);
        photographText.text = "Photograph No: " + (currentPhotographNumber + 1).ToString();
        nextPhotographNumber = (currentPhotographNumber + 1) % photoGallery.Count;
        previousPhotographNumber = (currentPhotographNumber - 1 + photoGallery.Count) % photoGallery.Count;
    }
    private void NextPhotograph(List<RenderTexture> photoGallery) {
        currentPhotograph = photoGallery[nextPhotographNumber];
        photographImage.texture = currentPhotograph;
        UpdatePhotographSequence(photoGallery);
    }

    private void PreviousPhotograph(List<RenderTexture> photoGallery) {
        currentPhotograph = photoGallery[previousPhotographNumber];
        photographImage.texture = currentPhotograph;
        UpdatePhotographSequence(photoGallery);
    }

    public void ChangePhotoAfterRemovePhoto() {
        List<RenderTexture> photoGallery = EquipmentStorageManager.Instance.GetPhotoGallery();

        if (photoGallery.Count > 0) {
            previousPhotographNumber = (currentPhotographNumber - 1 + photoGallery.Count) % photoGallery.Count;
            PreviousPhotograph(photoGallery);
        } else {
            currentPhotograph = null;
            ClosePhotoGallery();

        }
    }

    public static bool IsInPhotoGalleryMode() {
        return isShown;
    }
}
