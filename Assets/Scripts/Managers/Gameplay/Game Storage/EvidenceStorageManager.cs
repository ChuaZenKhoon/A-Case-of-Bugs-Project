using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A manager that handles the storage logic for evidence
 */
public class EvidenceStorageManager : MonoBehaviour {

    public static EvidenceStorageManager Instance { get; private set; }

    //Evidence Storage
    [Serializable]
    public struct FingerprintLifting {
        public Fingerprint liftedFingerprint;
        public int fingerprintLifterID;
    }

    private List<FingerprintLifting> fingerprintLiftings;

    [Serializable]
    public struct BloodStainCollection {
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

        fingerprintLiftings = new List<FingerprintLifting>();
        bloodStainCollection = new List<BloodStainCollection>();
        capturedAdultFlyCollection = new List<AdultFly>();
        killingAdultFlyCollection = new List<AdultFly>();
        deadAdultFlyCollection = new List<AdultFly>();
        maggotCollection = new List<Larvae>();
        killedMaggotsCollection = new List<Larvae>();
    }

    //Fingerprint
    public void SetFingerprint(int fingerprintLifterNum, Fingerprint fingerprint) {
        fingerprintLiftings.Add(new FingerprintLifting {
            fingerprintLifterID = fingerprintLifterNum,
            liftedFingerprint = fingerprint
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

    //Bloodstain
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


    //FlyNet
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

    public void AddToKillingJar(List<AdultFly> adultFlyToKill) {
        foreach (AdultFly adultFly in adultFlyToKill) {
            killingAdultFlyCollection.Add(adultFly);
        }

    }


    //Acetone Kill Jar
    public List<AdultFly> GetKillingFlies() {
        return killingAdultFlyCollection;
    }

    //Container
    public List<AdultFly> GetDeadFlies() {
        return deadAdultFlyCollection;
    }
    public void AddDeadFly(AdultFly adultFly) {
        deadAdultFlyCollection.Add(adultFly);
    }

    //HotWaterCup
    public List<Larvae> GetMaggots() {
        return maggotCollection;
    }

    public void CollectMaggots(Larvae maggot) {
        maggotCollection.Add(maggot);
    }

    public void ClearMaggotCollection() {
        maggotCollection.Clear();
    }
    public void AddKilledMaggots(List<Larvae> maggotsKilled) {
        foreach (Larvae maggot in maggotsKilled) {
            killedMaggotsCollection.Add(maggot);
        }
    }

    public void SetKillingMaggotProgressDuration(float duration) {
        killingMaggotsDuration = duration;
    }

    public float GetKillingMaggotProgressDuration() {
        return killingMaggotsDuration;
    }

    //Ethanol Container
    public List<Larvae> GetKilledMaggots() {
        return killedMaggotsCollection;
    }

}
