using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SketchLegendUI : MonoBehaviour {

    [SerializeField] private SketchPlanUI planUI;

    [SerializeField] private Button addLegendButton;
    [SerializeField] private Button removeLegendButton;
    [SerializeField] private Button backButton;
    [SerializeField] private LegendInputUI legendInputUIPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private RectTransform savedImageSpace;

    private int index;
    private void Awake() {

        index = 0;

        addLegendButton.onClick.AddListener(() => {
            AddLegendInput(null);
        });

        removeLegendButton.onClick.AddListener(() => {
            RemoveLegendInput();
        });

        backButton.onClick.AddListener(() => {
            SaveDetails();
        });


    }

    private void Start() {
        List<string> savedLegendInputTextList = EquipmentStorageManager.Instance.GetSketchPlanSavedLegendInputTextList();
        if (savedLegendInputTextList.Count != 0) {
            foreach (string legendInputText in savedLegendInputTextList) {
                AddLegendInput(legendInputText);
            }
        }

        UpdateButtons();
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

    private void AddLegendInput(string legendInputText) {
        LegendInputUI newLegendInputUI = Instantiate(legendInputUIPrefab, container);
        
        if (legendInputText != null) {
            newLegendInputUI.UpdateText(legendInputText, index+1);
        } else {
            newLegendInputUI.UpdateText(null, index+1);
        }

        index++;
        UpdateButtons();
    }

    private void RemoveLegendInput() {
        Transform child = container.GetChild(container.childCount - 1);
        Destroy(child.gameObject);
        index--;
        UpdateButtons();
    }

    private void UpdateButtons() {
        addLegendButton.interactable = index < 12;
        removeLegendButton.interactable = index > 0;
    }

    private void SaveDetails() {

        EquipmentStorageManager.Instance.ClearSketchPlanLegendInputUITextList();
        foreach (Transform child in container) {
            LegendInputUI legendInputUI = child.GetComponent<LegendInputUI>();
            if (legendInputUI != null) {
                EquipmentStorageManager.Instance.AddSketchPlanLegendInputUI(legendInputUI.GetText());
            }
        }

        //SaveLegendImage();
        //planUI.UpdateSketchImages();
        Hide();
    }

/*    private void SaveLegendImage() {
        int width = (int)savedImageSpace.rect.width;
        int height = (int)savedImageSpace.rect.height;

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(((Screen.width - width) / 2) , (Screen.height - height)/ 2, width, height), 0, 0);
        texture.Apply();

        // Convert the Texture2D to a Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Optionally, handle the sprite (e.g., save it or display it)
        EquipmentStorageManager.Instance.UpdateSavedSketchImages(sprite, 2);

    }*/
}
