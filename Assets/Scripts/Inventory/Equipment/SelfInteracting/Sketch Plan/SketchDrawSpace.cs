using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SketchDrawSpace : MonoBehaviour {

    [SerializeField] private SketchPlan sketchPlan;
    [SerializeField] private SketchDrawSpaceUI drawSpaceUI;

    public void Show() {
        sketchPlan.SetSketchMode(true);
        drawSpaceUI.Show();
    }

    public void Hide() {
        sketchPlan.SetSketchMode(false);
        drawSpaceUI.Hide();
    }
    public void UpdateSketchImage() {
        sketchPlan.UpdateSketchImages();
    }
    public void SaveDetails(Texture2D canvasTexture) {
        EquipmentStorageManager.Instance.UpdateSketchPlan(canvasTexture);
        SaveSketchImage(canvasTexture);
        UpdateSketchImage();
        Hide();
    }
    private void SaveSketchImage(Texture2D canvasTexture) {
        Sprite sprite = Sprite.Create(canvasTexture, new Rect(0, 0, canvasTexture.width, canvasTexture.height), new Vector2(0.5f, 0.5f));
        EquipmentStorageManager.Instance.UpdateSavedSketchImages(sprite);
    }

}
