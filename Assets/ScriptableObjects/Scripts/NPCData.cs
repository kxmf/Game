using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "My Game Data/NPC")]
public class NPCData : ScriptableObject
{
    public Sprite npcPortrait;
    public string npcName;
    public List<TaskData> availableTasks;
}
