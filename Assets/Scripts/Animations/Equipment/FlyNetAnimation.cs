using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyNetAnimation : CustomAnimation {

    [SerializeField] private FlyNet flyNet;

    private const string SWEEP_NET_ANIMATION_BOOL = "isSweepingNet";

    private Coroutine sweepingNetCoroutine;

    private void Awake() {
        sweepingNetCoroutine = null;
    }
    private void Start() {
        flyNet.OnStartNetSweeping += FlyNet_OnSweepNet;
    }

    private void FlyNet_OnSweepNet(object sender, System.EventArgs e) {
        if (sweepingNetCoroutine != null) {
            return;
        }
        animator.SetBool(SWEEP_NET_ANIMATION_BOOL, true);
        sweepingNetCoroutine = StartCoroutine(SweepNetCoroutine());
    }

    private IEnumerator SweepNetCoroutine() {
        yield return new WaitForSeconds(3f);
        animator.SetBool(SWEEP_NET_ANIMATION_BOOL, false);
        sweepingNetCoroutine = null;
    }
}
