using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A Manager superclass that interacts with the options menu UI.
 */
public class SettingsManager : MonoBehaviour {

    private OptionsMenuUI currentOptionsMenuUISubscribedTo;

    protected void SetUpOptionsMenuUI() {
        if (currentOptionsMenuUISubscribedTo != null) {
            UnsubscribeFromEvents(currentOptionsMenuUISubscribedTo);

        }

        OptionsMenuUI optionsMenu = GameObject.FindObjectOfType<OptionsMenuUI>();

        if (optionsMenu != null) {
            currentOptionsMenuUISubscribedTo = optionsMenu;
            SubscribeToEvents(optionsMenu);
        }

    }

    protected virtual void SubscribeToEvents(OptionsMenuUI optionsMenuUI) { }

    protected virtual void UnsubscribeFromEvents(OptionsMenuUI optionsMenuUI) { }
}
