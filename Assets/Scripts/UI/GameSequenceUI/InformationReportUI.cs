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

    private static Vector2 NEXT_BUTTON_ORIGINAL_POSITION = new Vector2(0, -450);
    private static Vector2 NEXT_BUTTON_SHIFTED_POSITION = new Vector2(200, -450);
    private const string NEXT_BUTTON_ORIGINAL_TEXT = "Next";
    private const string NEXT_BUTTON_FINAL_PAGE_TEXT = "Finish";

    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    
    //Pages of content to be shown to the player
    [SerializeField] private TextMeshProUGUI firstPage;
    [SerializeField] private TextMeshProUGUI secondPage;
    [SerializeField] private TextMeshProUGUI thirdPage;

    [SerializeField] private Image backgroundImage;

    private Page currentPage;

    //Sequence of pages in the information report
    private enum Page {
        First,
        Second,
        Third
    }

    private void Awake() {
        Instance = this;

        nextButton.onClick.AddListener(() => {
            NextPage();
        });
        backButton.onClick.AddListener(() => {
            PreviousPage();
        });

        currentPage = Page.First;
        ShowCurrentPage();

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
                nextButton.GetComponent<RectTransform>().anchoredPosition = NEXT_BUTTON_ORIGINAL_POSITION;
                nextButton.gameObject.SetActive(true);
                break;
            case Page.Second:
                nextButton.GetComponent<RectTransform>().anchoredPosition = NEXT_BUTTON_SHIFTED_POSITION;
                backButton.gameObject.SetActive(true);
                buttonText.text = NEXT_BUTTON_ORIGINAL_TEXT;
                break;
            case Page.Third:
                buttonText.text = NEXT_BUTTON_FINAL_PAGE_TEXT;
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
