using UnityEngine;

[CreateAssetMenu(menuName = "LevelSystem/FloorData")]
public class FloorData : ScriptableObject
{
    public string floorName;
    public string floorSceneName;
    public LevelData[] levels;
}
