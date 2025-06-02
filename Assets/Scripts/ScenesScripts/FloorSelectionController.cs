using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FloorSelectionController : MonoBehaviour
{
    public Button[] floorsButtons;
    public FloorData[] floors;

    private void Start()
    {
        var totalFloors = floorsButtons.Length;

        int highestUnlockedFloor = PlayerPrefs.GetInt("HighestUnlockedFloor", 0);

        for (int i = 0; i < floorsButtons.Length; i++)
        {
            Button currentButton = floorsButtons[i];

            if (i <= highestUnlockedFloor)
            {
                currentButton.interactable = true;
                currentButton.onClick.RemoveAllListeners();
                int currentFloorIndex = i;
                currentButton.onClick.AddListener(() => LoadBoardScene(currentFloorIndex));
            }
            else
                currentButton.interactable = false;
        }
    }

    private void LoadBoardScene(int floorIndex)
    {
        Debug.Log("Loading board for floor: " + floorIndex);
        PlayerPrefs.SetInt("CurrentFloorIndex", floorIndex);
        PlayerPrefs.Save();

        CustomSceneManager.LoadScene("Floor_1");
    }
}