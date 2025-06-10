using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string UnlockedFloorsKey = "UnlockedFloors";

    void Awake()
    {
        Debug.Log("SaveManager Initialized!");
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
}
