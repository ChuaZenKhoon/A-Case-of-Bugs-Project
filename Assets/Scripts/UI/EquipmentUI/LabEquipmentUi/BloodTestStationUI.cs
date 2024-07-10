using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing the display of the blood test station when the player uses it.
 */
public class BloodTestStationUI : MonoBehaviour {
    [SerializeField] private BloodTestStation bloodTestStation;

    [SerializeField] private Button addEthanolButton;
    [SerializeField] private Button addPhenolphthaleinButton;
    [SerializeField] private Button addHydrogenPeroxideButton;

    [SerializeField] private Button exitScreenButton;

    private void Awake() {
        exitScreenButton.onClick.AddListener(() => {
            Hide();
            bloodTestStation.ExitStation();
        });

        addEthanolButton.onClick.AddListener(() => {
            bloodTestStation.AddEthanol();
        });

        addPhenolphthaleinButton.onClick.AddListener(() => {
            bloodTestStation.AddPhenophtalein();
        });

        addHydrogenPeroxideButton.onClick.AddListener(() => {
            bloodTestStation.AddHydrogenPeroxide();
        });
    }

    private void Start() {
        Hide();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

}
