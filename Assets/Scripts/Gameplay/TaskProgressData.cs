[System.Serializable]
public class TaskProgressData
{
    public string taskId;
    public TaskStatus status;
    public string savedCode;

    public TaskProgressData(string id)
    {
        taskId = id;
        status = TaskStatus.Available;
        savedCode = "";
    }
}

public enum TaskStatus
{
    Available,
    InProgress,
    Completed,
}
