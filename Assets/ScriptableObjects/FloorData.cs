using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Floor", menuName = "My Game Data/Floor")]
public class FloorData : ScriptableObject
{
    [Header("Основная информация")]
    public int floorIndex;

    [Header("Игровой контент")]
    public string sceneToLoad;

    [Tooltip("Список всех задач, которые относятся к этому этажу.")]
    public List<TaskData> tasksOnThisFloor;

    [Header("Логика разблокировки")]
    public int unlocksFloorIndex = -1;
}
