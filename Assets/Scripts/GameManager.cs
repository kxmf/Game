using System;
using System.Collections.Generic;
using Python.Runtime;
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
                $"<color=yellow>GAME MANAGER:</color> Прогресс для задачи '{taskId}' не найден. Создаю новую запись со статусом 'Available'."
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
        PythonExecutionResult initialRun = pythonExecutor.Execute(userCode);
        if (!initialRun.Success)
        {
            onCheckCompleted?.Invoke(initialRun.Error, false);
            return;
        }

        bool isCorrect = false;
        string finalMessage = "";

        if (task.checkType == CheckType.SimpleOutput)
        {
            string actualOutput = initialRun.Output.Trim();
            string expected = task.expectedOutput.Trim();
            isCorrect = actualOutput == expected;

            if (isCorrect)
                finalMessage = $"> Тест пройден!\nВывод:\n{actualOutput}";
            else
                finalMessage =
                    $"> Тест не пройден.\nОжидалось:\n{expected}\nПолучено:\n{actualOutput}";
        }
        else if (task.checkType == CheckType.UnitTests)
        {
            try
            {
                using (Py.GIL())
                {
                    using (PyModule userModule = Py.CreateScope())
                    {
                        userModule.Exec(userCode);

                        using (PyModule testModule = Py.CreateScope())
                        {
                            testModule.Set("user_module", userModule);
                            testModule.Exec(task.unitTestCode);

                            PyObject testResultTuple = testModule.InvokeMethod("run_tests");

                            PyObject boolResult = testResultTuple.GetItem(0);
                            PyObject stringResult = testResultTuple.GetItem(1);

                            isCorrect = boolResult.As<bool>();
                            finalMessage = stringResult.As<string>();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                finalMessage = "Ошибка при запуске тестов: " + e.Message;
                isCorrect = false;
            }
        }

        if (isCorrect)
        {
            TaskProgressData progress = GetTaskProgress(task.taskId);
            if (progress != null && progress.status != TaskStatus.Completed)
            {
                progress.status = TaskStatus.Completed;
                OnTaskStatusChanged?.Invoke(task.taskId);
            }
        }

        onCheckCompleted?.Invoke(finalMessage, isCorrect);
    }
}
