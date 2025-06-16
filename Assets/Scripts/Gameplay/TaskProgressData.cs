[System.Serializable]
public class TaskProgressData
{
    public int taskId;
    public TaskStatus status;
    public string savedCode;

    public TaskProgressData(int id)
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
