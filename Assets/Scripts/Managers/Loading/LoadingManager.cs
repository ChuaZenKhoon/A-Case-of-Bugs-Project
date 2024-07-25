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
    private Coroutine holdLoadingCoroutine;

    private bool isLoading;

    private void Awake() {
        isLoading = false;
        holdLoadingCoroutine = null;
    }
    
    private void Start() {
        StartLoading(Loader.targetScene);

        if (isLoading && asyncOperation != null) {
            holdLoadingCoroutine = StartCoroutine(UpdateLoadingProgress());
        }
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
     * Imposes a minor waiting time for dynamic aesthetic purposes.
     */
    private IEnumerator UpdateLoadingProgress() {

        bool isLoaded = false;

        //Updates loading bar
        while (!isLoaded) {
            yield return new WaitForSeconds(0.1f);
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressBar.value = (progress - 0.1f) > 0f ? (progress - 0.1f) : progress;
            progressText.text = (progressBar.value * 100f).ToString("F0") + "%";
            if (asyncOperation.progress >= 0.9f) {
                isLoaded = true;
            }
        }

        //Once loading is done, impose minor waiting time.
        yield return new WaitForSeconds(2f);
        float finishedProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
        progressBar.value = finishedProgress;
        progressText.text = (finishedProgress * 100f).ToString("F0") + "%";
        yield return new WaitForSeconds(0.5f);
        asyncOperation.allowSceneActivation = true;
        holdLoadingCoroutine = null;
    }
}
