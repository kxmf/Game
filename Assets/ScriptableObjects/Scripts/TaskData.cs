using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "My Game Data/Task")]
public class TaskData : ScriptableObject
{
    [Header("Основная информация")]
    public int taskId;
    public string taskName;

    [Header("Настройки автопроверки")]
    [Tooltip("Список тестов для проверки кода. Код будет запущен для каждого теста.")]
    public List<TestCase> TestCases = new List<TestCase>();

    [Header("Контент")]
    [TextArea(3, 10)]
    public string dialogueText;

    [TextArea(10, 20)]
    public string theoryText;

    [TextArea(10, 20)]
    public string assignmentText;
}
