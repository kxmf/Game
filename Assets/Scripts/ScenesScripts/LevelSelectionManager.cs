using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    public Transform boardsContainer;

    private int currentFloor;

    void Start()
    {
        currentFloor = PlayerPrefs.GetInt("CurrentFloorIndex", 1);

        for (int i = 0; i < boardsContainer.childCount; i++)
        {
            var board = boardsContainer.GetChild(i);

            if (i == currentFloor)
                board.gameObject.SetActive(true);
            else
                board.gameObject.SetActive(false);
        }
    }

    public void LoadLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("CurrentLevelIndex", levelIndex);
        PlayerPrefs.Save();
        CustomSceneManager.LoadScene($"Level");
    }
}
