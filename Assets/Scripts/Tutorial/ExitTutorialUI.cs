using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitTutorialUI : MonoBehaviour {

    [SerializeField] private Button exitButton;


    private void Awake() {
        exitButton.onClick.AddListener(() => {
            SceneManager.LoadScene(Loader.Scene.LoadingScreen.ToString());
            Loader.targetScene = Loader.Scene.MainMenu;
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
}
