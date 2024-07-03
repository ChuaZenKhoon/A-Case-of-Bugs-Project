using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialItemsManager : MonoBehaviour {

    public static TutorialItemsManager Instance { get; private set; }

    public event EventHandler<OnEquipmentMarkerHitEventArgs> OnEquipmentMarkerHit;
    public event EventHandler OnEquipmentGuideOpenClose;

    [SerializeField] private List<InventoryObjectSO> dummyItems;
    [SerializeField] private List<InventoryObjectSO> equipmentList;

    [SerializeField] private List<string> equipmentUseClipsFilePath;
    [SerializeField] private List<string> equipmentUseText;

    [SerializeField] private EquipmentGuideUI equipmentGuideUI;

    private bool isEquipmentGuideOpen;

    public class OnEquipmentMarkerHitEventArgs {
        public string videoClipFilePath;
        public string equipmentUsageText;
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        TutorialLevelManager.Instance.OnStateChange += TutorialLevelManager_OnStateChange;
        equipmentGuideUI.OnClose += EquipmentGuideUI_OnClose;
    }

    private void TutorialLevelManager_OnStateChange(object sender, System.EventArgs e) {
        if (TutorialLevelManager.Instance.IsStartingInventory()) {
            foreach (InventoryObjectSO item in dummyItems) {
                AddDummyItemToInventory(item);
            }
        } else {
            AddEquipmentToInventory(TutorialLevelManager.Instance.GetState());
        }
    }

    private void AddDummyItemToInventory(InventoryObjectSO item) {
        InventoryManager.Instance.AddToInventory(item);
    }

    private void AddEquipmentToInventory(TutorialLevelManager.State state) {
        List<InventoryObjectSO> itemsToAdd = new List<InventoryObjectSO>();
        
        string equipmentVideoToPlay = null;
        string equipmentUsageTextToAdd = string.Empty;

        switch (state) {
            case TutorialLevelManager.State.Equipment_Camera:
                itemsToAdd.Add(equipmentList[0]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[0];
                equipmentUsageTextToAdd = equipmentUseText[0];
                break;
            case TutorialLevelManager.State.Equipment_Clipboard:
                itemsToAdd.Add(equipmentList[1]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[1];
                equipmentUsageTextToAdd = equipmentUseText[1];
                break;
            case TutorialLevelManager.State.Equipment_PlacardHolder:
                itemsToAdd.Add(equipmentList[2]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[2];
                equipmentUsageTextToAdd = equipmentUseText[2];
                break;
            case TutorialLevelManager.State.Equipment_MeasuringTool:
                itemsToAdd.Add(equipmentList[3]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[3];
                equipmentUsageTextToAdd = equipmentUseText[3];
                break;
            case TutorialLevelManager.State.Equipment_Phone:
                itemsToAdd.Add(equipmentList[4]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[4];
                equipmentUsageTextToAdd = equipmentUseText[4];
                break;
            case TutorialLevelManager.State.Equipment_FingerprintDuster_FingerprintLifter:
                itemsToAdd.Add(equipmentList[5]);
                itemsToAdd.Add(equipmentList[6]);
                itemsToAdd.Add(equipmentList[7]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[5];
                equipmentUsageTextToAdd = equipmentUseText[5];
                break;
            case TutorialLevelManager.State.Equipment_Swab:
                itemsToAdd.Add(equipmentList[8]);
                itemsToAdd.Add(equipmentList[9]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[6];
                equipmentUsageTextToAdd = equipmentUseText[6];
                break;
            case TutorialLevelManager.State.Equipment_Container:
                itemsToAdd.Add(equipmentList[10]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[7];
                equipmentUsageTextToAdd = equipmentUseText[7];
                break;
            case TutorialLevelManager.State.Equipment_FlyNet_KillJar:
                itemsToAdd.Add(equipmentList[11]);
                itemsToAdd.Add(equipmentList[12]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[8];
                equipmentUsageTextToAdd = equipmentUseText[8];
                break;
            case TutorialLevelManager.State.Equipment_HotWaterCup_EthanolContainer:
                itemsToAdd.Add(equipmentList[13]);
                itemsToAdd.Add(equipmentList[14]);
                equipmentVideoToPlay = equipmentUseClipsFilePath[9];
                equipmentUsageTextToAdd = equipmentUseText[9];
                break;
            default:
                break;

        }

        if (itemsToAdd.Count > 0) {
            if (equipmentVideoToPlay != null) {

                OnEquipmentMarkerHit?.Invoke(this, new OnEquipmentMarkerHitEventArgs { 
                    videoClipFilePath = equipmentVideoToPlay,
                    equipmentUsageText = equipmentUsageTextToAdd
                });

                equipmentGuideUI.Show();
                isEquipmentGuideOpen = true;

                OnEquipmentGuideOpenClose?.Invoke(this, EventArgs.Empty);
            }
        }

        foreach (InventoryObjectSO inventoryObjectSO in itemsToAdd) {
            InventoryManager.Instance.AddToInventory(inventoryObjectSO);
        }
    }

    public bool IsEquipmentGuideOpen() {
        return isEquipmentGuideOpen;
    }

    private void EquipmentGuideUI_OnClose(object sender, EventArgs e) {
        isEquipmentGuideOpen = false;
        OnEquipmentGuideOpenClose?.Invoke(this, EventArgs.Empty);
    }

}
