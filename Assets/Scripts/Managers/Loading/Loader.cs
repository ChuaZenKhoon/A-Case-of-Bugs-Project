using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * A static class used in managing the scenes and which scene to load.
 */
public static class Loader {

    //A list of all scenes to load, using enum instead of string which is bad practice and error prone
    public enum Scene {
        MainMenu,
        LoadingScreen,
        TutorialScene,
        CrimeScene,
    }

    public static Scene targetScene; //For scenes to set and refer to
}
