using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element repesenting the score obtained by the player at the end of the main game.
 */
public class ScoringSystemUI : MonoBehaviour {

    public static ScoringSystemUI Instance { get; private set; }

    private const string SCORE_TOTAL = " / 7";
    private const int SCORE_EARNED = 1;

    [SerializeField] private Button closeButton;

    [SerializeField] private List<Image> tickList;
    [SerializeField] private List<Image> crossList;

    [SerializeField] private TextMeshProUGUI totalScoreText;

    private void Awake() {
        Instance = this;

        closeButton.onClick.AddListener(() => {
            Hide();
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

    public void UpdateScores(int stepOnFlyScore, int stepOnDeceasedScore, int walkMainPathScore,
        int sketchPlanDetailsPoint, int fingerprintCollectionPoint, int weatherAppPoint, 
        int evidenceCollectionPoint, int sumScore) {

        ToggleView(stepOnFlyScore, tickList[0], crossList[0]);
        ToggleView(stepOnDeceasedScore, tickList[1], crossList[1]);
        ToggleView(walkMainPathScore, tickList[2], crossList[2]);
        ToggleView(sketchPlanDetailsPoint, tickList[3], crossList[3]);
        ToggleView(fingerprintCollectionPoint, tickList[4], crossList[4]);
        ToggleView(weatherAppPoint, tickList[5], crossList[5]);
        ToggleView(evidenceCollectionPoint, tickList[6], crossList[6]);

        totalScoreText.text = sumScore.ToString() + SCORE_TOTAL;
    }

    private void ToggleView(int score, Image tickIcon, Image CrossIcon) {
        if (score == SCORE_EARNED) {
            tickIcon.gameObject.SetActive(true);
            CrossIcon.gameObject.SetActive(false);
        } else {
            tickIcon.gameObject.SetActive(false);
            CrossIcon.gameObject.SetActive(true);
        }
    }
}
