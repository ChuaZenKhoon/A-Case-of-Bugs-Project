using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ScoringSystemManager : MonoBehaviour {

    public static ScoringSystemManager Instance { get; private set; }

    [SerializeField] private DeceasedBody deceasedBody;

    [SerializeField] private MainPathScore mainPath;

    private int didNotStepOnFliesPoint = 1; //event

    private int didNotStepOnDeceasedBodyPoint = 1; //event

    private int walkOnMainPathTooMuchPoint = 1; //cumulative event

    private int completeSketchPlanDetailsPoint = 1; //event

    private int properFingerprintCollectionPoint = 1; //event

    private int checkedWeatherAppPoint = 0; //event

    private int completeEvidenceCollectionPoint = 1; //list of events

    private List<GameObject> evidenceListToCompare;

    private void Awake() {
        Instance = this;
    }
    
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
        walkOnMainPathTooMuchPoint = 0;
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

    private void CrimeSceneLevelManager_OnStateChange(object sender, System.EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsGameOver()) {
            string[] sketchPlanDetails = EquipmentStorageManager.Instance.GetSketchPlanSavedDetailsTextList();
            foreach (string s in sketchPlanDetails) {
                if (string.IsNullOrEmpty(s)) {
                    completeSketchPlanDetailsPoint = 0;
                }
            }

            for (int i = 0; i < evidenceListToCompare.Count; i++) {
                Debug.Log("Checking Item");
                if (evidenceListToCompare[i] == null) {
                    continue;
                }

                if (evidenceListToCompare[i] != null) {
                    if (evidenceListToCompare[i].GetComponentInChildren<Bloodstain>() != null) {
                        continue;
                    } else {
                        Debug.Log("Item there");
                        completeEvidenceCollectionPoint = 0;
                    }
                }
 
            }

            if (EquipmentStorageManager.Instance.GetBloodStains().Count != 2) {
                completeEvidenceCollectionPoint = 0;
            }

            int points = SumPoints();

            ScoringSystemUI.Instance.UpdateScores(didNotStepOnFliesPoint, didNotStepOnDeceasedBodyPoint, walkOnMainPathTooMuchPoint,
                completeSketchPlanDetailsPoint, properFingerprintCollectionPoint, checkedWeatherAppPoint, completeEvidenceCollectionPoint,
                points);
            
            ScoringSystemUI.Instance.Show();
        }

        
    }

    private int SumPoints() {
        return didNotStepOnFliesPoint + didNotStepOnDeceasedBodyPoint + checkedWeatherAppPoint + completeSketchPlanDetailsPoint
            + completeEvidenceCollectionPoint + properFingerprintCollectionPoint + walkOnMainPathTooMuchPoint;
    }
    //Collected fingerprint with bare hand



    //Collected bloodstain with bare hand

    //walked too much on main walking path

    //Did not collect all evidence possible


    //empty sketch plan

    //less than 10 photographs

    //Never checked weather?

    //Never fill in sketch plan details

    //Stepping on deceased body

    //Stepping on dead adult flies

    //

}