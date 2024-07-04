using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/**
 * A manager that handles the scoring throughout the main game
 */
public class ScoringSystemManager : MonoBehaviour {

    public static ScoringSystemManager Instance { get; private set; }

    [SerializeField] private DeceasedBody deceasedBody;

    [SerializeField] private MainPath mainPath;

    private int didNotStepOnFliesPoint = 1; //One time trigger

    private int didNotStepOnDeceasedBodyPoint = 1; //One time trigger

    private int didNotWalkOnMainPathTooMuchPoint = 1; //Cumulative add up trigger

    private int completeSketchPlanDetailsPoint = 1; //One time check at the end

    private int properFingerprintCollectionPoint = 1; //One time trigger

    private int checkedWeatherAppPoint = 0; //One time trigger

    private int completeEvidenceCollectionPoint = 1; //One time check at the end

    private List<GameObject> evidenceListToCompare;

    private void Awake() {
        Instance = this;
    }
    
    //Subscribe to events for triggers
    private void Start() {
        CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;

        deceasedBody.OnStepOnDeadBody += DeceasedBody_OnStepOnDeadBody;

        AdultFly.OnStepOnFly += AdultFly_OnStepOnFly;

        Phone.OnPhoneOpen += Phone_OnPhoneOpen;

        Fingerprint.OnImproperFingerprintCollection += Fingerprint_OnImproperFingerprintCollection;

        mainPath.OnMainPathWalkTooMuch += MainPath_OnMainPathWalkTooMuch;

        evidenceListToCompare = CrimeSceneLevelManager.Instance.GetDifficultyEvidenceListItems();
    }

    private void MainPath_OnMainPathWalkTooMuch(object sender, System.EventArgs e) {
        didNotWalkOnMainPathTooMuchPoint = 0;
    }

    private void Fingerprint_OnImproperFingerprintCollection(object sender, System.EventArgs e) {
        properFingerprintCollectionPoint = 0;
    }

    private void Phone_OnPhoneOpen(object sender, System.EventArgs e) {
        checkedWeatherAppPoint = 1;
    }

    private void AdultFly_OnStepOnFly(object sender, System.EventArgs e) {
        didNotStepOnFliesPoint = 0;
    }

    private void DeceasedBody_OnStepOnDeadBody(object sender, System.EventArgs e) {
        didNotStepOnDeceasedBodyPoint = 0;
    }

    //Do one time checks at the end
    private void CrimeSceneLevelManager_OnStateChange(object sender, System.EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsGameOver()) {
            OneTimeCheckSketchDetails();
            OneTimeCheckEvidenceList();

            int points = SumPoints();

            ScoringSystemUI.Instance.UpdateScores(didNotStepOnFliesPoint, didNotStepOnDeceasedBodyPoint, 
                didNotWalkOnMainPathTooMuchPoint, completeSketchPlanDetailsPoint, 
                properFingerprintCollectionPoint, checkedWeatherAppPoint, 
                completeEvidenceCollectionPoint, points);
            
            ScoringSystemUI.Instance.Show();
        }   
    }

    //For extension in the future
    private int SumPoints() {
        return didNotStepOnFliesPoint + didNotStepOnDeceasedBodyPoint + 
            checkedWeatherAppPoint + completeSketchPlanDetailsPoint + 
            completeEvidenceCollectionPoint + properFingerprintCollectionPoint + 
            didNotWalkOnMainPathTooMuchPoint;
    }


    //One time checks

    private void OneTimeCheckSketchDetails() {
        string[] sketchPlanDetails = EquipmentStorageManager.Instance.GetSketchPlanSavedDetailsTextList();
        foreach (string s in sketchPlanDetails) {
            if (string.IsNullOrEmpty(s)) {
                completeSketchPlanDetailsPoint = 0;
            }
        }
    }

    private void OneTimeCheckEvidenceList() {
        for (int i = 0; i < evidenceListToCompare.Count; i++) {
            if (evidenceListToCompare[i] != null) {
                if (evidenceListToCompare[i].GetComponentInChildren<Bloodstain>() != null) {
                    //Ignore, bloodstains will not disappear
                    continue;
                } else {
                    //Incomplete collection
                    completeEvidenceCollectionPoint = 0;
                }
            }
        }

        //Check if both swabs are used
        if (EvidenceStorageManager.Instance.GetBloodStains().Count != 2) {
            completeEvidenceCollectionPoint = 0;
        }
    }
}