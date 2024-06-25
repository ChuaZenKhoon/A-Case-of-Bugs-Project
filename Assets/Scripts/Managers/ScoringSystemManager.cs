using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystemManager : MonoBehaviour {

    public static ScoringSystemManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    //Collected fingerprint with bare hand
    //Collected bloodstain with bare hand

    //walked too much on main walking path

    //Did not collect all evidence possible
    

    //empty sketch plan

    //less than 10 photographs

    //placards not used?

    //Never checked weather?
    //Never fill in sketch plan details
    

    //Stepping on deceased body
    //

}