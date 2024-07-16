using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * A manager in charge of handling the transition between scenes using the Loader static class.
 */
public class LoadingManager : MonoBehaviour {

    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    private AsyncOperation asyncOperation;

    private bool isLoading;
    private bool isFirstUpdate;

    private void Awake() {
        isLoading = false;
        isFirstUpdate = true;
    }
    
    private void Update() {

        if(isFirstUpdate) {
            PrepareLoading();
        }

        if (!isFirstUpdate && isLoading && asyncOperation != null) {
            UpdateLoadingProgress();
        }
    }

    /**
     * Prepares the loading action after loading screen is ready.
     */
    private void PrepareLoading() {
        isFirstUpdate = false;
        StartLoading(Loader.targetScene);
    }

    /**
     * Loads the target scene asynchronously.
     * 
     * @param scene The scene to be loaded while in the loading screen.
     */
    public void StartLoading(Loader.Scene scene) {
        if (!isLoading) {
            isLoading = true;
            asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
            asyncOperation.allowSceneActivation = false;
        }
    }

    /**
     * Updates the progress bar and opens target scene once fully loaded.
     */
    private void UpdateLoadingProgress() {
        float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
        progressBar.value = progress;
        progressText.text = (progress * 100f).ToString("F0") + "%";

        //Progress for async loading of scenes cap at 0.9f, rest of 0.1f is for finalising scene activation
        if (asyncOperation.progress >= 0.9f) {
            asyncOperation.allowSceneActivation = true;
        }
    }
}
