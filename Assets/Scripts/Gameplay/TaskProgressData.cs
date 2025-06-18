[System.Serializable]
public class TaskProgressData
{
    public int taskId;
    public TaskStatus status;
    public string savedCode;

    public TaskProgressData(int id)
    {
        taskId = id;
        status = TaskStatus.NotAvailable;
        savedCode = "";
    }
}

public enum TaskStatus
{
    NotAvailable,
    Available,
    InProgress,
    Completed,
}
