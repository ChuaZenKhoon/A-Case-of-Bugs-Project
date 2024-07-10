using UnityEngine;


/**
 * A scriptable object that represents the information an inventory object should have.
 */
[CreateAssetMenu()]
public class InventoryObjectSO : ScriptableObject {

    public GameObject prefab;
    public Sprite sprite;
    public string objectName;
    public string objectDescription;
}
