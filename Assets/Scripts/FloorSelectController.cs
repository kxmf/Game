using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FloorSelectController : MonoBehaviour
{
    [SerializeField] private Button[] floorsButtons;
    [SerializeField] private FloorData[] floors;

    void Start()
    {
        var totalFloors = floorsButtons.Length;

        int highestUnlockedFloor = PlayerPrefs.GetInt("HighestUnlockedFloor", 2);

        for (int i = 0; i < floorsButtons.Length; i++)
        {
            int floorNumber = i;
            Button currentButton = floorsButtons[i];

            if (floorNumber <= highestUnlockedFloor)
            {
                currentButton.interactable = true;

                //Image lockImage = currentButton.transform.Find("LockIcon")?.GetComponent<Image>();
                //if (lockImage != null) lockImage.enabled = false;

                currentButton.onClick.RemoveAllListeners();
                int currentFloorIndex = floorNumber;
                currentButton.onClick.AddListener(() => LoadFloorScene(currentFloorIndex));
            }
            else
            {
                currentButton.interactable = false;

                // ������� 1: ������ ������ ����� ������
                //if (lockedSprite != null)
                //{
                //    currentButton.GetComponent<Image>().sprite = lockedSprite;
                //    // ��������, ����� �������� � disabledSprite ������ ��� ����������� ����
                //    var colors = currentButton.colors;
                //    colors.disabledColor = Color.white; // ����� ������ ����� �� ����������
                //    currentButton.colors = colors;
                //}

                //// ������� 2: �������� �������� ������ � ������� �����
                //Image lockImage = currentButton.transform.Find("LockIcon")?.GetComponent<Image>();
                //if (lockImage != null) lockImage.enabled = true;

                currentButton.onClick.RemoveAllListeners();
            }
        }
    }

    void LoadFloorScene(int floorIndex)
    {
        Debug.Log("Loading board for floor: " + floorIndex);
        PlayerPrefs.SetInt("CurrentFloorIndex", floorIndex);
        PlayerPrefs.Save();

        CustomSceneManager.LoadScene(floors[floorIndex].floorSceneName);
    }
}