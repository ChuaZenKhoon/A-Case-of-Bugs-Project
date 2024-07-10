using System.Collections.Generic;
using UnityEngine;

/**
 * The superclass representing any form of evidence related to flies.
 * This class and its subclasses can be examined under the microscope.
 */
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
