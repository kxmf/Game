using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string UnlockedFloorsKey = "UnlockedFloors";
    private const string TaskProgressKey = "TaskProgress";

    void Awake()
    {
        Debug.Log("SaveManager Initialized!");
    }

    [System.Serializable]
    private class TaskProgressCollection
    {
        public List<TaskProgressEntry> entries = new();
    }

    [System.Serializable]
    private class TaskProgressEntry
    {
        public int taskId;
        public TaskProgressData progressData;
    }

    public void SaveProgress(int unlockedFloorsCount)
    {
        PlayerPrefs.SetInt(UnlockedFloorsKey, unlockedFloorsCount);
        PlayerPrefs.Save();
        Debug.Log($"Progress Saved! Unlocked floors: {unlockedFloorsCount}");
    }

    public int LoadProgress()
    {
        if (!PlayerPrefs.HasKey(UnlockedFloorsKey))
            return 0;

        int unlockedFloors = PlayerPrefs.GetInt(UnlockedFloorsKey);
        Debug.Log($"Progress Loaded! Unlocked floors: {unlockedFloors}");
        return unlockedFloors;
    }

    public void SaveTaskProgress(Dictionary<int, TaskProgressData> tasksProgress)
    {
        var collection = new TaskProgressCollection();

        foreach (var pair in tasksProgress)
        {
            collection.entries.Add(
                new TaskProgressEntry { taskId = pair.Key, progressData = pair.Value }
            );
        }

        string json = JsonUtility.ToJson(collection);

        PlayerPrefs.SetString(TaskProgressKey, json);
        Debug.Log($"SAVE MANAGER: Сохранено {collection.entries.Count} записей о прогрессе задач.");
    }

    public Dictionary<int, TaskProgressData> LoadTaskProgress()
    {
        var loadedProgress = new Dictionary<int, TaskProgressData>();

        if (PlayerPrefs.HasKey(TaskProgressKey))
        {
            string json = PlayerPrefs.GetString(TaskProgressKey);

            var collection = JsonUtility.FromJson<TaskProgressCollection>(json);

            foreach (var entry in collection.entries)
                loadedProgress[entry.taskId] = entry.progressData;

            Debug.Log($"SAVE MANAGER: Загружено {loadedProgress.Count} записей о прогрессе задач.");
        }
        else
        {
            Debug.Log(
                "SAVE MANAGER: Файл сохранения прогресса задач не найден. Будет создан новый."
            );
        }

        return loadedProgress;
    }

    public void DeleteAllSaveData()
    {
        PlayerPrefs.DeleteKey(UnlockedFloorsKey);
        PlayerPrefs.DeleteKey(TaskProgressKey);

        Debug.LogWarning("SAVE MANAGER: Все данные сохранения были удалены!");
    }
}
