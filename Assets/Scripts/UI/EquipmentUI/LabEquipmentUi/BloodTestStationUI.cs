using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodTestStationUI : MonoBehaviour {

    [SerializeField] private BloodTestStation bloodTestStation;

    [SerializeField] private Button addEthanolButton;
    [SerializeField] private Button addPhenolphthaleinButton;
    [SerializeField] private Button addHydrogenPeroxideButton;

    [SerializeField] private Button exitScreenButton;

    private void Awake() {
        exitScreenButton.onClick.AddListener(() => {
            Hide();
            bool isTestedHalfWay = bloodTestStation.CheckHasBeenTestedHalfway();
            
            if (isTestedHalfWay) {
                bloodTestStation.ExitFromEquipmentScreen();
                bloodTestStation.IncompleteTestProcedure();
            } else {
                bloodTestStation.ExitFromEquipmentScreen();
            }
            
        });

        addEthanolButton.onClick.AddListener(() => {
            bool isCorrect = bloodTestStation.CheckEthanolIsAddedFirst();
            if (!isCorrect) {
                Hide();
                bloodTestStation.ExitFromEquipmentScreen();
                bloodTestStation.WrongTestProcedure();
            } else {
                MessageLogManager.Instance.LogMessage("Correct step! What next?");
            }
        });

        addPhenolphthaleinButton.onClick.AddListener(() => {
            bool isCorrect = bloodTestStation.CheckPhenophthaleinIsAddedSecond();
            if (!isCorrect) {
                Hide();
                bloodTestStation.ExitFromEquipmentScreen();
                bloodTestStation.WrongTestProcedure();
            } else {
                MessageLogManager.Instance.LogMessage("Correct step! What next?");
            }
        });

        addHydrogenPeroxideButton.onClick.AddListener(() => {
            bool isCorrect = bloodTestStation.CheckHydrogenPeroxideIsAddedThird();
            if (!isCorrect) {
                Hide();
                bloodTestStation.ExitFromEquipmentScreen();
                bloodTestStation.WrongTestProcedure();
            } else {
                Hide();
                bloodTestStation.ExitFromEquipmentScreen();
                bloodTestStation.CorrectTestProcedure();
            }
        });
    }

    private void Start() {
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

}
