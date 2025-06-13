[System.Serializable]
public class MarkerProgressData
{
    public string taskId;
    public MarkerStatus status;
    public string savedCode;

    public MarkerProgressData(string id)
    {
        taskId = id;
        status = MarkerStatus.Available;
        savedCode = "";
    }
}

public enum MarkerStatus
{
    Available,
    InProgress,
    Completed,
}
