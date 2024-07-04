using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A manager that handles the storage logic for equipment
 */
public class EquipmentStorageManager : MonoBehaviour {

    public static EquipmentStorageManager Instance { get; private set; }


    //Equipment Storage
    private List<RenderTexture> photoGallery;

    private string[] savedSketchPlanDetailsTextList;

    private List<string> savedSketchPlanLegendInputTextList;

    private Texture2D savedSketchPlan;

    private Sprite savedSketchImage;

    [SerializeField] private Placard[] placards;

    [Serializable] 
    public struct WeatherRecord {
        public int temperatureRecord;
        public int humidityRecord;
        public string weatherConditionRecord;
    }

    [SerializeField] private List<WeatherRecord> weatherRecords;

    

    private void Awake() {
        Instance = this;

        photoGallery = new List<RenderTexture>();

        savedSketchPlanDetailsTextList = new string[6];
        savedSketchPlanLegendInputTextList = new List<string>();

    }

    //Photograph Camera
    public List<RenderTexture> GetPhotoGallery() {
        return photoGallery;
    }

    public void AddPhotoToPhotoGallery(RenderTexture picture) {
        photoGallery.Add(picture);
    }

    public void RemovePhotoFromPhotoGallery(int index) {
        photoGallery.RemoveAt(index);
    }

    //Sketch Plan

    //Sketch

    public Texture2D GetSavedSketchPlan() {
        return savedSketchPlan;
    }

    public void UpdateSketchPlan(Texture2D sketch) {
        this.savedSketchPlan = sketch;
    }
   
    public void UpdateSavedSketchImages(Sprite sprite) {
        savedSketchImage = sprite;
    }

    public Sprite GetSavedSketchImages() {
        return savedSketchImage;
    }

    //Sketch Details
    public string[] GetSketchPlanSavedDetailsTextList() {
        return this.savedSketchPlanDetailsTextList;
    }

    public void AddSketchPlanDetailsUI(string text, int index) {
        savedSketchPlanDetailsTextList[index] = text;
    }

    //Sketch legend

    public List<string> GetSketchPlanSavedLegendInputTextList() {
        return this.savedSketchPlanLegendInputTextList;
    }

    public void AddSketchPlanLegendInputUI(string text) {
        savedSketchPlanLegendInputTextList.Add(text);
    }

    public void ClearSketchPlanLegendInputUITextList() {
        savedSketchPlanLegendInputTextList.Clear();
    }



    //Placards
    public Placard[] GetPlacardList() {
        return placards;
    }

    public void SetPlacard(int index, Placard placard) {
        placards[index] = placard;
    }

    //Phone
    public WeatherRecord GetWeatherRecord(int index) {
        return weatherRecords[index];
    } 

    
}
