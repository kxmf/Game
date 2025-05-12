using System.Linq;
using UnityEngine;

public class FloorSelectController : MonoBehaviour
{
    public FloorData[] allFloors;
    public Transform contentParent;
    public FloorButton floorButtonPrefab;

    void Start()
    {
        for (int i = 0; i < allFloors.Length; i++)
        {
            var data = allFloors[i];
            var btn = Instantiate(floorButtonPrefab, contentParent);
            btn.Label.text = data.floorName;
            bool isUnlocked = i == 0
                || ProgressManager.Instance.IsLevelCompleted(allFloors[i - 1].levels.Last().sceneName);
            btn.Button.interactable = isUnlocked;
            int index = i;
            btn.Button.onClick.AddListener(() =>
                SceneManager.LoadScene(data.boardSceneName, LoadSceneMode.Single));
        }
    }
}