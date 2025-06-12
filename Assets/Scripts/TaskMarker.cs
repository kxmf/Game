using UnityEngine;

public class TaskMarker : MonoBehaviour
{
    [SerializeField]
    private TaskData taskData;

    private void OnMouseDown()
    {
        if (taskData == null)
        {
            Debug.LogError("No task data", this.gameObject);
            return;
        }

        Debug.Log($"Task click: '{taskData.taskName}'. ID: '{taskData.taskId}'");

        // GameManager.instance.StartTask(taskData);
    }

    private void OnMouseEnter()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    private void OnMouseExit()
    {
        transform.localScale = Vector3.one;
    }
}
