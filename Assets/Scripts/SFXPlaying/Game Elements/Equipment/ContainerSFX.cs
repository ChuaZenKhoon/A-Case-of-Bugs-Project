using UnityEngine;

public class ContainerSFX : SFX {

    [SerializeField] private Container container;

    private void Awake() {
        volMultiplier = 1f;
    }

    private void Start() {
        container.OnCollectFly += Container_OnCollectFly;
    }

    private void Container_OnCollectFly(object sender, System.EventArgs e) {
        GameElementSFXPlayer.Instance.PlayTweezerUseSound(Player.Instance.GetHoldPosition().position, volMultiplier);
    }
}
