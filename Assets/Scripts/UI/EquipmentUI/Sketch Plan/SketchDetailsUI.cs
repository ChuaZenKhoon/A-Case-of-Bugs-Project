using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SketchDetailsUI : MonoBehaviour {

    [SerializeField] private SketchPlanUI planUI;

    [SerializeField] private Button backButton;

    [SerializeField] private TMP_InputField[] inputArray;

    [SerializeField] private RectTransform savedImageSpace;

    private void Awake() {
        backButton.onClick.AddListener(() => {
            SaveDetails();
        });
    }
    private void Start() {
        string[] textArray = EquipmentStorageManager.Instance.GetSketchPlanSavedDetailsTextList();
        
        for (int i = 0; i < textArray.Length; i++) {
            if (textArray[i] != null) {
                inputArray[i].text = textArray[i];
            }
        }

        Hide();
    }
    private void Hide() {
        SketchPlanUI.isInSketchMode = false;
        gameObject.SetActive(false);
    }

    public void Show() {
        SketchPlanUI.isInSketchMode = true;
        gameObject.SetActive(true);
    }

    private void SaveDetails() {
        for (int i = 0; i < inputArray.Length; i++) {
            if (inputArray[i].text != null) {
                EquipmentStorageManager.Instance.AddSketchPlanDetailsUI(inputArray[i].text, i);
            }
        }

        //SaveDetailsImage();
        //planUI.UpdateSketchImages();
        Hide();
    }

/*    private void SaveDetailsImage() {
        int width = (int)savedImageSpace.rect.width;
        int height = (int)savedImageSpace.rect.height;

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(((Screen.width - width) / 2), (Screen.height - height) / 2, width, height), 0, 0);
        texture.Apply();

        // Convert the Texture2D to a Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Optionally, handle the sprite (e.g., save it or display it)
        EquipmentStorageManager.Instance.UpdateSavedSketchImages(sprite, 1);
    }*/
}
