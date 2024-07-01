using TMPro;
using UnityEngine;


/**
 * A UI element that represents the game clock.
 */
public class GamePlayClockUI : MonoBehaviour {

    public static GamePlayClockUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI minutesText;
    [SerializeField] private TextMeshProUGUI secondsText;
    [SerializeField] private TextMeshProUGUI tickText;

    private float timer;

    private void Awake() {
        Instance = this;
        timer = 0f;
    }

    private void Start() {
        CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;
    }

    private void CrimeSceneLevelManager_OnStateChange(object sender, System.EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsLabStarted()) {
            Hide();
        }
    }

    private void Update() {
        float minutesLeft = CrimeSceneLevelManager.Instance.GetGamePlayTimeLeft(out float secondsTime);
        minutesText.text = minutesLeft.ToString();
        if (secondsTime < 10f) {
            secondsText.text = "0" + secondsTime.ToString();
        } else {
            secondsText.text = secondsTime.ToString();
        }
        timer += Time.deltaTime;
        
        //For the ':' tick to show dynamic time movement
        if(timer > 1f) {
            tickText.gameObject.SetActive(!tickText.gameObject.activeSelf);
            timer = 0f;
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
