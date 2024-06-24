using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGroupOfFlies : Evidence {

    [SerializeField] private List<AdultFly> adultFlies;

    
    public int GetNumFliesAvailable() {
        return adultFlies.Count;
    }

    public List<AdultFly> CaptureFlies() {
        int numOfFliesCaptured = Random.Range(1, adultFlies.Count);
        List<AdultFly> fliesCaptured = new List<AdultFly>();

        for (int index = 0; index < numOfFliesCaptured; index++) {
            int flyCapturedIndex = Random.Range(0, adultFlies.Count);
            AdultFly fly = adultFlies[flyCapturedIndex];
            fliesCaptured.Add(fly);
            adultFlies.RemoveAt(flyCapturedIndex);
        }

        if (adultFlies.Count == 0) {
            Destroy(gameObject);
        }

        return fliesCaptured;
    }
}
