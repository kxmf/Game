using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private SaveManager saveManager;
    private PythonExecutor pythonExecutor;

    public int UnlockedFloorsCount { get; private set; }

    public int CurrentFloorIndex { get; private set; }

    public Dictionary<int, TaskProgressData> tasksProgress = new();
    private string previousSceneName;

    public static event Action<int> OnTaskStatusChanged;

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
        pythonExecutor = GetComponent<PythonExecutor>();

        Debug.Log("GameManager Initialized!");
    }

    void Start()
    {
        UnlockedFloorsCount = saveManager.LoadProgress();
        pythonExecutor.Initialize();

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

    public void GoToSettings()
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Переходим в настройки. Запомненная сцена: {previousSceneName}");
        SceneManager.LoadScene("SettingsScene");
    }

    public void ReturnFromSettings()
    {
        if (string.IsNullOrEmpty(previousSceneName))
        {
            Debug.LogWarning("Предыдущая сцена не была запомнена. Возврат в главное меню.");
            LoadMainMenu();
            return;
        }

        Debug.Log($"Возвращаемся на сцену: {previousSceneName}");
        SceneManager.LoadScene(previousSceneName);
    }

    public void InitializeTaskProgress(TaskData task)
    {
        if (!tasksProgress.ContainsKey(task.taskId))
            tasksProgress.Add(task.taskId, new TaskProgressData(task.taskId));
    }

    public TaskProgressData GetTaskProgress(int taskId)
    {
        if (tasksProgress.TryGetValue(taskId, out TaskProgressData progress))
        {
            return progress;
        }
        else
        {
            Debug.Log(
                $"GAME MANAGER: Прогресс для задачи '{taskId}' не найден. Создаю новую запись со статусом 'Available'."
            );

            var newProgress = new TaskProgressData(taskId);
            tasksProgress.Add(taskId, newProgress);
            return newProgress;
        }
    }

    public void StartDialogueWithNPC(NPCData npcData)
    {
        // var tasksToShow = new List<TaskData>();
        // foreach (var task in npcData.availableTasks)
        // {
        //     TaskProgressData progress = GetTaskProgress(task.taskId);
        //     if (
        //         progress == null
        //         || progress.status == TaskStatus.Available
        //         || progress.status == TaskStatus.InProgress
        //     )
        //     {
        //         tasksToShow.Add(task);
        //     }
        // }
        // For future

        if (npcData.availableTasks.Count > 0)
            UIManager.instance.ShowDialogue(npcData, npcData.availableTasks);
        else
            Debug.Log($"С NPC {npcData.npcName} сейчас не о чем говорить.");
    }

    public void StartTask(TaskData task)
    {
        var progress = GetTaskProgress(task.taskId);
        string savedCode = (progress != null) ? progress.savedCode : "";

        progress.status = TaskStatus.InProgress;
        OnTaskStatusChanged?.Invoke(task.taskId);

        UIManager.instance.ShowTaskWindow(task, savedCode);
    }

    public void SaveTaskCode(int taskId, string code)
    {
        TaskProgressData progress = GetTaskProgress(taskId);
        if (progress != null)
        {
            progress.savedCode = code;
            Debug.Log($"Код для задачи {taskId} сохранен.");
        }
    }

    public void CheckTaskSolution(
        TaskData task,
        string userCode,
        Action<string, bool> onCheckCompleted
    )
    {
        if (task.TestCases == null || task.TestCases.Count == 0)
        {
            onCheckCompleted?.Invoke(
                "> Для этого задания нет тестов. Выполнено автоматически.",
                true
            );
            MarkTaskAsCompleted(task.taskId);
            return;
        }

        for (int i = 0; i < task.TestCases.Count; i++)
        {
            TestCase currentTest = task.TestCases[i];

            var result = pythonExecutor.Execute(userCode, currentTest.InputData);

            if (!result.Success)
            {
                string errorMessage = $"> Тест #{i + 1} провален с ошибкой:\n{result.Error}";
                onCheckCompleted?.Invoke(errorMessage, false);
                return;
            }

            string actualOutput = result.Output.Trim().Replace("\r\n", "\n");
            string expectedOutput = currentTest.ExpectedOutput.Trim().Replace("\r\n", "\n");

            if (actualOutput != expectedOutput)
            {
                string failureMessage =
                    $"> Тест #{i + 1} провален.\n"
                    + $"Входные данные:\n---\n{currentTest.InputData}\n---\n"
                    + $"Ожидаемый вывод:\n---\n{expectedOutput}\n---\n"
                    + $"Ваш вывод:\n---\n{actualOutput}\n---";
                onCheckCompleted?.Invoke(failureMessage, false);
                return;
            }
        }

        string successMessage = $"> Все {task.TestCases.Count} тестов пройдены успешно!";
        onCheckCompleted?.Invoke(successMessage, true);
        MarkTaskAsCompleted(task.taskId);
    }

    private void MarkTaskAsCompleted(int taskId)
    {
        TaskProgressData progress = GetTaskProgress(taskId);
        if (progress != null && progress.status != TaskStatus.Completed)
        {
            progress.status = TaskStatus.Completed;
            OnTaskStatusChanged?.Invoke(taskId);
        }
    }
}
