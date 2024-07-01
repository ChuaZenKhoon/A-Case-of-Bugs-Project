using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flies : Evidence {

    [SerializeField] List<Sprite> examinedSprites;
    [SerializeField] string speciesName;

    public List<Sprite> GetSpritesForExamination() {
        return examinedSprites;
    }

    public string GetSpeciesName() {
        return speciesName;
    }

}
