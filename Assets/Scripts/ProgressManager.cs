using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    private HashSet<string> completedLevels = new();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadProgress();
    }

    public bool IsLevelCompleted(string sceneName) => completedLevels.Contains(sceneName);

    public void MarkLevelCompleted(string sceneName)
    {
        if (completedLevels.Add(sceneName))
            SaveProgress();
    }

    void LoadProgress()
    {
        var json = PlayerPrefs.GetString("ProgressData", "{}");
        completedLevels = JsonUtility.FromJson<SerializableSet>(json).ToHash();
    }

    void SaveProgress()
    {
        var json = JsonUtility.ToJson(new SerializableSet(completedLevels));
        PlayerPrefs.SetString("ProgressData", json);
    }
}

[System.Serializable]
class SerializableSet
{
    public List<string> items;
    public SerializableSet(HashSet<string> set) => items = set.ToList();
    public HashSet<string> ToHash() => new(items);
}