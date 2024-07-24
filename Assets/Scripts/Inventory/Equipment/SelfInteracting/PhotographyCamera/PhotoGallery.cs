using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * A logic component of the photography camera that handles the function of the photo gallery.
 */
public class PhotoGallery : MonoBehaviour {

    public event EventHandler OnShufflePicButtonClick;

    [SerializeField] private PhotographyCamera photographyCamera;

    [SerializeField] private PhotoGalleryUI photoGalleryUI;
    [SerializeField] private PhotographyCameraDeletePhotoUI photographyCameraDeletePhotoUI;



    private int nextPhotographNumber;
    private int previousPhotographNumber;
    private int currentPhotographNumber;
    private RenderTexture currentPhotograph;

    private List<RenderTexture> photoGallery;

    private void Start() {
        photoGallery = EquipmentStorageManager.Instance.GetPhotoGallery();
        if (photoGallery.Count > 0) {
            currentPhotograph = photoGallery.First();
            UpdatePhotographSequence();
        }
    }

    public void ViewPhotoGallery() {
        //First show
        if (currentPhotograph == null) {
            currentPhotograph = photoGallery.First();
        }

        UpdatePhotographSequence();

        photoGalleryUI.UpdatePhoto(currentPhotograph);



        photoGalleryUI.Show();
    }
    public void ClosePhotoGallery() {
        photoGalleryUI.Hide();
    }

    private void UpdatePhotographSequence() {
        currentPhotographNumber = photoGallery.IndexOf(currentPhotograph);
        nextPhotographNumber = (currentPhotographNumber + 1) % photoGallery.Count;
        previousPhotographNumber = (currentPhotographNumber - 1 + photoGallery.Count) % photoGallery.Count;

        string newPhotoNumber = (currentPhotographNumber + 1).ToString();
        photoGalleryUI.UpdatePhotographNumber(newPhotoNumber);
    }
    public void NextPhotograph() {
        currentPhotograph = photoGallery[nextPhotographNumber];

        photoGalleryUI.UpdatePhoto(currentPhotograph);

        UpdatePhotographSequence();

        OnShufflePicButtonClick?.Invoke(this, EventArgs.Empty);
    }

    public void PreviousPhotograph() {
        currentPhotograph = photoGallery[previousPhotographNumber];

        photoGalleryUI.UpdatePhoto(currentPhotograph);

        UpdatePhotographSequence();
        OnShufflePicButtonClick?.Invoke(this, EventArgs.Empty);
    }

    public void DeletePhoto() {
        photographyCameraDeletePhotoUI.Show();
        OnShufflePicButtonClick?.Invoke(this, EventArgs.Empty);
    }

    /**
     * Adjusts the photo gallery displayed to the user after a confirmed deletion.
     */
    public void ChangePhotoAfterRemovePhoto() {
        EquipmentStorageManager.Instance.RemovePhotoFromPhotoGallery(currentPhotographNumber);

        if (photoGallery.Count > 0) {
            previousPhotographNumber = (currentPhotographNumber - 1 + photoGallery.Count) % photoGallery.Count;
            PreviousPhotograph();
        } else {
            currentPhotograph = null;
            photographyCamera.ClosePhotoGallery();
        }
    }

}
