using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "My Game Data/Task")]
public class TaskData : ScriptableObject
{
    [Header("General")]
    public string taskId;
    public string taskName;

    [Header("Content")]
    [TextArea(3, 10)]
    public string dialogueText;

    [TextArea(10, 20)]
    public string theoryText;

    [TextArea(10, 20)]
    public string assignmentText;
}
