using System.Collections.Generic;
using UnityEngine;

/**
 * The class representing evidence in the form of a swarm of live adult flies.
 */
public class FlyingGroupOfFlies : Evidence {

    [SerializeField] private List<AdultFly> adultFlies;

    public int GetNumFliesAvailable() {
        return adultFlies.Count;
    }

    /**
     * Upon net sweeping, a random number of flies are collected from the ones available.
     * Fly swarm is removed when there are no more flies left to collect.
     * Random number of flies + Random fly index in fixed list 
     * Simulates real fly catching that takes several tries.
     */
    public List<AdultFly> CaptureFlies() {
        //Get random number of flies to collect, x
        int numOfFliesCaptured = Random.Range(1, adultFlies.Count);
        List<AdultFly> fliesCaptured = new List<AdultFly>();

        //Get random fly to collect based on random index
        //Repeat for x times
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
