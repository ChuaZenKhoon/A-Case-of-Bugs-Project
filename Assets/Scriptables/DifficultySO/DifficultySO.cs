using UnityEngine;

[CreateAssetMenu()]
public class DifficultySO : ScriptableObject {

    public DifficultySettingManager.DifficultyLevel difficultyLevel;
    public string difficultyDescription;
    public EquipmentSO[] equipmentListToAdd;
}
