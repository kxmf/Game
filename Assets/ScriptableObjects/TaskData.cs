using UnityEngine;

public enum CheckType
{
    SimpleOutput,
    UnitTests,
}

[CreateAssetMenu(fileName = "New Task", menuName = "My Game Data/Task")]
public class TaskData : ScriptableObject
{
    [Header("Основная информация")]
    public int taskId;
    public string taskName;

    [Header("Настройки проверки")]
    public CheckType checkType;

    [Tooltip("Ожидаемый результат в консоли. Пробелы в конце будут удалены.")]
    [TextArea(3, 5)]
    public string expectedOutput;

    [Tooltip(
        "Код на Python, который будет выполнять тесты. Должен содержать функцию 'run_tests(user_module)'."
    )]
    [TextArea(10, 20)]
    public string unitTestCode;

    [Header("Контент")]
    [TextArea(3, 10)]
    public string dialogueText;

    [TextArea(10, 20)]
    public string theoryText;

    [TextArea(10, 20)]
    public string assignmentText;
}
