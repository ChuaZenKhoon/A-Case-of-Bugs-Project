using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotWaterCupSFX : SFX {

    [SerializeField] private HotWaterCup hotWaterCup;
    private void Awake() {
        volMultiplier = 1f;
    }

    private void Start() {
        hotWaterCup.OnPourWater += HotWaterCup_OnPourWater;
        hotWaterCup.OnCollectSpecimen += HotWaterCup_OnCollectSpecimen;
    }

    private void HotWaterCup_OnCollectSpecimen(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayTweezerUseSound(Player.Instance.GetHoldPosition().position, volMultiplier);
    }

    private void HotWaterCup_OnPourWater(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayPourWaterSound(Player.Instance.GetHoldPosition().position, volMultiplier);
    }
}
