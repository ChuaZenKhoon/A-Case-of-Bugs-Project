using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class InventoryObjectSO : ScriptableObject {

    public GameObject prefab;
    public Sprite sprite;
    public string objectName;
    public string objectDescription;
}
