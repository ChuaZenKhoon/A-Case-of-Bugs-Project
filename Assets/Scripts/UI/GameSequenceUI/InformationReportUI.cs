using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI element that represents the information report to be read 
 * before the game starts.
 */
public class InformationReportUI : MonoBehaviour {

    public static InformationReportUI Instance { get; private set; }
    
    //Event for when information report is finished
    public event EventHandler OnClickReadFinishReport;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    
    //Pages of content to be shown to the player
    [SerializeField] private TextMeshProUGUI firstPage;
    [SerializeField] private TextMeshProUGUI secondPage;
    [SerializeField] private TextMeshProUGUI thirdPage;

    [SerializeField] private Image backgroundImage;

    private Page currentPage;

    private Vector2 nextButtonOriginalPagePosition = new Vector2(0, -450);
    private Vector2 nextButtonInBetweenPagesPosition = new Vector2(200, -450);

    //Sequence of pages in the information report
    private enum Page {
        First,
        Second,
        Third
    }

    private void Awake() {
        Instance = this;
        currentPage = Page.First;
        ShowCurrentPage();
        nextButton.onClick.AddListener(() => {
            NextPage();
        });
        backButton.onClick.AddListener(() => {
            PreviousPage();
        });
    }

    private void NextPage() {

        if (currentPage < Page.Third) {
            currentPage++;
            ShowCurrentPage();
        } else {
            FinishReading();
        }
    }

    private void PreviousPage() {
        if (currentPage > Page.First) {
            currentPage--;
            ShowCurrentPage();
        }
    }
    
    /**
     * Shows the current page of the information report for the player to read.
     */
    private void ShowCurrentPage() {
        firstPage.gameObject.SetActive(currentPage == Page.First);
        secondPage.gameObject.SetActive(currentPage == Page.Second);
        thirdPage.gameObject.SetActive(currentPage == Page.Third);   

        switch (currentPage) {
            case Page.First:
                backButton.gameObject.SetActive(false);
                nextButton.GetComponent<RectTransform>().anchoredPosition = nextButtonOriginalPagePosition;
                nextButton.gameObject.SetActive(true);
                break;
            case Page.Second:
                nextButton.GetComponent<RectTransform>().anchoredPosition = nextButtonInBetweenPagesPosition;
                backButton.gameObject.SetActive(true);
                buttonText.text = "Next";
                break;
            case Page.Third:
                buttonText.text = "Finish";
                break;

        }

    }

    private void FinishReading() {
        OnClickReadFinishReport?.Invoke(this, EventArgs.Empty);
        Hide();
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}
