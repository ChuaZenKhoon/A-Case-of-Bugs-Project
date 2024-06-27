using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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
