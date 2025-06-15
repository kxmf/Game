using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskWindow : MonoBehaviour
{
    [Header("Ссылки на UI элементы")]
    [SerializeField]
    private TextMeshProUGUI contentText;

    [SerializeField]
    private TMP_InputField codeInputField;

    [SerializeField]
    private TextMeshProUGUI consoleOutputText;

    [Header("Вкладки")]
    [SerializeField]
    private Button theoryTabButton;

    [SerializeField]
    private Button taskTabButton;

    [Header("Кнопки управления")]
    [SerializeField]
    private Button checkButton;

    [SerializeField]
    private Button closeButton;

    [Header("Цвета вкладок")]
    [SerializeField]
    private Color activeTabColor = Color.yellow;

    [SerializeField]
    private Color inactiveTabColor = Color.white;

    private TaskData currentTask;
    private bool isShowingTheory = true;

    void Awake()
    {
        theoryTabButton.onClick.AddListener(ShowTheory);
        taskTabButton.onClick.AddListener(ShowTask);
        checkButton.onClick.AddListener(CheckCode);
        closeButton.onClick.AddListener(CloseWindow);
    }

    public void Show(TaskData task, string savedCode)
    {
        currentTask = task;
        consoleOutputText.text = "> Ready...";

        codeInputField.text = savedCode;

        ShowTheory();

        gameObject.SetActive(true);
    }

    private void ShowTheory()
    {
        isShowingTheory = true;
        UpdateContent();
    }

    private void ShowTask()
    {
        isShowingTheory = false;
        UpdateContent();
    }

    private void UpdateContent()
    {
        if (currentTask == null)
            return;

        contentText.text = isShowingTheory ? currentTask.theoryText : currentTask.assignmentText;

        theoryTabButton.GetComponent<Image>().color = isShowingTheory
            ? activeTabColor
            : inactiveTabColor;
        taskTabButton.GetComponent<Image>().color = isShowingTheory
            ? inactiveTabColor
            : activeTabColor;
    }

    private void CheckCode()
    {
        if (currentTask == null)
            return;

        string userCode = codeInputField.text;
        consoleOutputText.text = "> Запускаю проверку...";

        GameManager.instance.CheckTaskSolution(
            currentTask,
            userCode,
            (resultMessage, isCorrect) =>
            {
                consoleOutputText.text = resultMessage;
                if (isCorrect)
                    Debug.Log("Task completed!");
            }
        );
    }

    private void CloseWindow()
    {
        GameManager.instance.SaveTaskCode(currentTask.taskId, codeInputField.text);
        UIManager.instance.HideTaskWindow();
    }
}
