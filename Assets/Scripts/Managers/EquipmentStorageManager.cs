using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    //Evidence Storage
    [Serializable] public struct FingerprintLifting {
        public Fingerprint liftedFingerprint;
        public int fingerprintLifterID;
    }

    private List<FingerprintLifting> fingerprintLiftings;

    [Serializable] public struct BloodStainCollection {
        public Bloodstain bloodStainCollected;
        public int swabID;
        public bool testedPositive;
        public bool cannotBeUsedAnymore;
    }

    private List<BloodStainCollection> bloodStainCollection;

    private List<AdultFly> capturedAdultFlyCollection;

    private List<AdultFly> killingAdultFlyCollection;

    private List<AdultFly> deadAdultFlyCollection;

    private List<Larvae> maggotCollection;

    private List<Larvae> killedMaggotsCollection;

    private float killingMaggotsDuration;

    private void Awake() {
        Instance = this;

        photoGallery = new List<RenderTexture>();

        savedSketchPlanDetailsTextList = new string[6];
        savedSketchPlanLegendInputTextList = new List<string>();

        fingerprintLiftings = new List<FingerprintLifting>();
        bloodStainCollection = new List<BloodStainCollection>();
        capturedAdultFlyCollection = new List<AdultFly>();
        killingAdultFlyCollection = new List<AdultFly>();
        deadAdultFlyCollection = new List<AdultFly>();
        maggotCollection = new List<Larvae>();
        killedMaggotsCollection = new List<Larvae>();
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

    public Texture2D GetSavedSketchPlan() {
        return savedSketchPlan;
    }

    public void UpdateSketchPlan(Texture2D sketch) {
        this.savedSketchPlan = sketch;
    }

    public string[] GetSketchPlanSavedDetailsTextList() {
        return this.savedSketchPlanDetailsTextList;
    }

    public void AddSketchPlanDetailsUI(string text, int index) {
        savedSketchPlanDetailsTextList[index] = text;
    }

    public List<string> GetSketchPlanSavedLegendInputTextList() {
        return this.savedSketchPlanLegendInputTextList;
    }

    public void AddSketchPlanLegendInputUI(string text) {
        savedSketchPlanLegendInputTextList.Add(text);
    }

    public void ClearSketchPlanLegendInputUITextList() {
        savedSketchPlanLegendInputTextList.Clear();
    }

    public void UpdateSavedSketchImages(Sprite sprite) {
        savedSketchImage = sprite;
    }

    public Sprite GetSavedSketchImages() {
        return savedSketchImage;
    }

    public Placard[] GetPlacardList() {
        return placards;
    }

    public void SetPlacard(int index, Placard placard) {
        placards[index] = placard;
    }


    public WeatherRecord GetWeatherRecord(int index) {
        return weatherRecords[index];
    } 

    //Evidence
    public void SetFingerprint(int fingerprintLifterNum, Fingerprint fingerprint) {
        fingerprintLiftings.Add(new FingerprintLifting {
            fingerprintLifterID = fingerprintLifterNum, liftedFingerprint = fingerprint
        });
    }

    public Fingerprint GetFingerprint(int fingerprintLifterNum) {
        for (int i = 0; i < fingerprintLiftings.Count; i++) {
            if (fingerprintLiftings[i].fingerprintLifterID == fingerprintLifterNum) {
                return fingerprintLiftings[i].liftedFingerprint;
            }
        }

        return null;
    }

    public void SetBloodStain(int swabNum, Bloodstain bloodStain, bool positive, bool obsolete) {
        for (int i = 0; i < bloodStainCollection.Count; i++) {
            if (bloodStainCollection[i].swabID == swabNum) {
                bloodStainCollection.RemoveAt(i);
            }
        }

        bloodStainCollection.Add(new BloodStainCollection {
            swabID = swabNum,
            bloodStainCollected = bloodStain,
            testedPositive = positive,
            cannotBeUsedAnymore = obsolete
        });
    }

    public Bloodstain GetBloodStain(int swabNum, out bool positive, out bool cannotBeUsed) {
        for (int i = 0; i < bloodStainCollection.Count; i++) {
            if (bloodStainCollection[i].swabID == swabNum) {
                positive = bloodStainCollection[i].testedPositive;
                cannotBeUsed = bloodStainCollection[i].cannotBeUsedAnymore;
                return bloodStainCollection[i].bloodStainCollected;
            }
        }

        positive = false;
        cannotBeUsed = false;
        return null;
    }

    public List<BloodStainCollection> GetBloodStains() {
        return bloodStainCollection;
    }

    public List<AdultFly> GetCapturedFlies() {
        return capturedAdultFlyCollection;
    }

    public void CaptureFlies(List<AdultFly> capturedFlies) {
        foreach (AdultFly fly in capturedFlies) {
            capturedAdultFlyCollection.Add(fly);
        }
    }

    public void ClearCapturedFlies() {
        capturedAdultFlyCollection.Clear();
    }

    public List<AdultFly> GetKillingFlies() {
        return killingAdultFlyCollection;
    }

    public void AddToKillingJar(List<AdultFly> adultFlyToKill) {
        foreach (AdultFly adultFly in adultFlyToKill) {
            killingAdultFlyCollection.Add(adultFly);
        }
        
    }

    public List<AdultFly> GetDeadFlies() {
        return deadAdultFlyCollection;
    }
    public void AddDeadFly(AdultFly adultFly) {
        deadAdultFlyCollection.Add(adultFly);
    }

    public List<Larvae> GetMaggots() {
        return maggotCollection;
    }

    public void CollectMaggots(Larvae maggot) {
        maggotCollection.Add(maggot);
    }

    public void ClearMaggotCollection() {
        maggotCollection.Clear();
    }

    public List<Larvae> GetKilledMaggots() {
        return killedMaggotsCollection;
    }

    public void AddKilledMaggots(List<Larvae> maggotsKilled) {
        foreach (Larvae maggot in maggotsKilled) {
            killedMaggotsCollection.Add(maggot);
        }
    }

    public void ClearMaggotKilledCollection() {
        killedMaggotsCollection.Clear();
    }

    public void SetKillingMaggotProgressDuration(float duration) {
        killingMaggotsDuration = duration;
    }

    public float GetKillingMaggotProgressDuration() {
        return killingMaggotsDuration;
    }
}
