using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing the photo gallery of the photography camera.
 */
public class PhotoGalleryUI : MonoBehaviour {

    [SerializeField] private PhotoGallery photoGallery;

    [SerializeField] RawImage photographImage;

    [SerializeField] private Button backButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button removePictureButton;
    [SerializeField] private TextMeshProUGUI photographText;

    private void Awake() {
        nextButton.onClick.AddListener(() => {
            photoGallery.NextPhotograph();
        });
        backButton.onClick.AddListener(() => {
            photoGallery.PreviousPhotograph();
        });
        removePictureButton.onClick.AddListener(() => {
            photoGallery.DeletePhoto(); 
        });
    }

    private void Start() {
        Hide();
    }

    public void UpdatePhoto(RenderTexture photo) {
        photographImage.texture = photo;
    }

    public void UpdatePhotographNumber(string text) {
        photographText.text = "Photograph No: " + text;
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
