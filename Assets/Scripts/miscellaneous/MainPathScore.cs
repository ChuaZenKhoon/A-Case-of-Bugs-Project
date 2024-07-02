using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPathScore : MonoBehaviour {

    public event EventHandler OnMainPathWalkTooMuch;

    private float distanceAllowed = 100f;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Collider playerCollider;

    private float totalDistanceTraveled = 0f;
    private Vector3 lastPosition;

    private void Start () {
        CrimeSceneLevelManager.Instance.OnStateChange += CrimeSceneLevelManager_OnStateChange;
    }

    private void CrimeSceneLevelManager_OnStateChange(object sender, EventArgs e) {
        if (CrimeSceneLevelManager.Instance.IsGamePlaying()) {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        } else {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other) {
       
        if (other == playerCollider) {

            // Calculate the distance the player has moved since the last collision stay
            float distanceTraveled = Vector3.Distance(playerTransform.position, lastPosition);
            totalDistanceTraveled += distanceTraveled;

            // Update the last position to the current position
            lastPosition = playerTransform.position;

            // Check if the player has walked the allowed distance
            if (totalDistanceTraveled >= distanceAllowed) {
                // Reset the distance or perform other actions
                OnMainPathWalkTooMuch?.Invoke(this, EventArgs.Empty);
            }
        }
    }


}
