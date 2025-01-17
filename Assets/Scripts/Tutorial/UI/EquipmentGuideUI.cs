using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/**
 * A UI component that represents the pop up display to the player 
 * that demonstrates the use of equipment for ease of learning.
 */
public class EquipmentGuideUI : MonoBehaviour {

    public event EventHandler OnClose;

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI equipmentUsageText;
    [SerializeField] private Button closeButton;
    [SerializeField] private ScrollRect scrollRect;

    private string videoPathStorage;

    private void Awake() {
        closeButton.onClick.AddListener(() => {
            Hide();
            scrollRect.verticalNormalizedPosition = 1;
            OnClose?.Invoke(this, EventArgs.Empty);
        });
    }

    private void Start() {
        TutorialItemsManager.Instance.OnEquipmentMarkerHit += TutorialItemsManager_OnEquipmentMarkerHit;
        Hide();
    }

    private void TutorialItemsManager_OnEquipmentMarkerHit(object sender, TutorialItemsManager.OnEquipmentMarkerHitEventArgs e) {
        equipmentUsageText.text = e.equipmentUsageText;
        videoPathStorage = e.videoClipFilePath;
        
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    //Play the equipment guide video
    public void Show() {
        gameObject.SetActive(true);
        string videoPath = System.IO.Path.Combine(GetStreamingAssetsPath(), videoPathStorage);
        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }

    private string GetStreamingAssetsPath() {
        return Application.streamingAssetsPath.Replace("\\", "/");
    }

}
