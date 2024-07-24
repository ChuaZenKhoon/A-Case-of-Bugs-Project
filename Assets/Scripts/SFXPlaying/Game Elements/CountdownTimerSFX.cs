using System.Collections;
using UnityEngine;

public class CountdownTimerSFX : SFX {

    private void Awake() {
        volMultiplier = 1f;
    }

    private void Start() {
        CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;
    }

    private void CrimeSceneLevelManager_OnStateChange(object sender, System.EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsGameStartCountingDown()) {
            StartCoroutine(CountdownSFX());
        }
    }

    private IEnumerator CountdownSFX() {
        float countdownTime = 5f;

        while (countdownTime > 0f) {
            SFXPlayer.Instance.PlayCountdownTickSound(volMultiplier);
            yield return new WaitForSeconds(1f);
            countdownTime -= 1f;
        }

        SFXPlayer.Instance.PlayCountdownEndSound(volMultiplier);

    }
}
