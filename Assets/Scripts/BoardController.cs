//using UnityEngine;

//public class BoardController : MonoBehaviour
//{
//    public FloorData thisFloor;
//    public Transform contentParent;
//    public LevelButton levelButtonPrefab;

//    void Start()
//    {
//        foreach (var lvl in thisFloor.levels)
//        {
//            var btn = Instantiate(levelButtonPrefab, contentParent);
//            btn.Label.text = lvl.levelName;
//            bool completed = ProgressManager.Instance.IsLevelCompleted(lvl.sceneName);
//            btn.SetCompletedVisual(completed);
//            btn.Button.onClick.AddListener(() =>
//                SceneManager.LoadScene(lvl.sceneName));
//        }
//    }
//}