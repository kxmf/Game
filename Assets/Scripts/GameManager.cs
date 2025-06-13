using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private SaveManager saveManager;

    public int UnlockedFloorsCount { get; private set; }

    public int CurrentFloorIndex { get; private set; }

    public Dictionary<string, TaskProgressData> tasksProgress = new();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        saveManager = GetComponent<SaveManager>();

        Debug.Log("GameManager Initialized!");
    }

    void Start()
    {
        UnlockedFloorsCount = saveManager.LoadProgress();

        if (SceneManager.GetActiveScene().name == "InitScene")
            LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadGameScene(int floorIndex)
    {
        Debug.Log($"Loading Game Scene for floor {floorIndex}...");
        CurrentFloorIndex = floorIndex;
        SceneManager.LoadScene("GameScene");
    }

    public void InitializeTaskProgress(TaskData task)
    {
        if (!tasksProgress.ContainsKey(task.taskId))
        {
            tasksProgress.Add(task.taskId, new TaskProgressData(task.taskId));
        }
    }

    public TaskProgressData GetTaskProgress(string taskId)
    {
        if (tasksProgress.TryGetValue(taskId, out TaskProgressData progress))
            return progress;

        return null;
    }

    public void StartDialogueWithNPC(NPCData npcData)
    {
        var tasksToShow = new List<TaskData>();
        foreach (var task in npcData.availableTasks)
        {
            TaskProgressData progress = GetTaskProgress(task.taskId);
            if (
                progress == null
                || progress.status == TaskStatus.Available
                || progress.status == TaskStatus.InProgress
            )
            {
                tasksToShow.Add(task);
            }
        }

        if (tasksToShow.Count > 0)
        {
            UIManager.instance.ShowDialogue(npcData, tasksToShow);
        }
        else
            Debug.Log($"С NPC {npcData.npcName} сейчас не о чем говорить.");
    }

    public void StartTask(TaskData task)
    {
        Debug.Log($"Начинаем выполнение задачи '{task.taskName}'...");
        // Например: UIManager.instance.ShowTaskWindow(task);
    }
}
