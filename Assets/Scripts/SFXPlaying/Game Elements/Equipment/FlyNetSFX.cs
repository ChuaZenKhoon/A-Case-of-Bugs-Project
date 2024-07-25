using System.Collections;
using UnityEngine;

public class FlyNetSFX : SFX {

    [SerializeField] private FlyNet flyNet;

    private Coroutine sweepNetCoroutine;
    private void Awake() {
        volMultiplier = 1f;
        sweepNetCoroutine = null;
    }

    private void Start() {
        flyNet.OnSweepNet += FlyNet_OnSweepNet;
    }

    private void FlyNet_OnSweepNet(object sender, System.EventArgs e) {
        if (sweepNetCoroutine != null) {
            return;
        }

        sweepNetCoroutine = StartCoroutine(SweepNetCoroutine());
    }

    private IEnumerator SweepNetCoroutine() {
        GameElementSFXPlayer.Instance.PlayFlyNetUseSound(Player.Instance.GetHoldPosition().position, volMultiplier);
        yield return new WaitForSeconds(0.5f);
        sweepNetCoroutine = null;
    }
}
