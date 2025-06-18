using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "My Game Data/Task")]
public class TaskData : ScriptableObject
{
    [Header("Основная информация")]
    public int taskId;
    public string taskName;

    [Header("Требования для разблокировки")]
    public List<int> prerequisiteTaskIds = new();

    [Header("Настройки автопроверки")]
    public List<TestCase> TestCases = new();

    [Header("Контент")]
    [TextArea(3, 10)]
    public string dialogueText;

    [TextArea(10, 20)]
    public string theoryText;

    [TextArea(10, 20)]
    public string assignmentText;
}
