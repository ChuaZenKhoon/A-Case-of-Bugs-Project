using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodTestStationSFX : SFX {

    [SerializeField] private BloodTestStation bloodTestStation;

    [SerializeField] private Button addEthanolButton;
    [SerializeField] private Button addPhenolphthaleinButton;
    [SerializeField] private Button addHydrogenPeroxideButton;

    private void Awake() {
        volMultiplier = 1f;
        addEthanolButton.onClick.AddListener(() => {
            PlayDripWater();
        });
        addPhenolphthaleinButton.onClick.AddListener(() => {
            PlayDripWater();
        });
        addHydrogenPeroxideButton.onClick.AddListener(() => {
            PlayDripWater();
        });
    }

    private void Start() {
        bloodTestStation.OnCorrectTest += BloodTestStation_OnCorrectTest;
        bloodTestStation.OnWrongTest += BloodTestStation_OnWrongTest;
    }

    private void BloodTestStation_OnWrongTest(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayWrongTestSound(volMultiplier);
    }

    private void BloodTestStation_OnCorrectTest(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayCorrectTestSound(volMultiplier * 2f);
    }

    private void PlayDripWater() {
        GameElementSFXPlayer.Instance.PlayDripLiquidSound(volMultiplier * 5f);
    }

}
