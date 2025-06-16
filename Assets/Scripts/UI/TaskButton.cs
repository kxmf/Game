using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI taskNameText;
    private Button button;

    private TaskData associatedTask;
    private Action<TaskData> onTaskSelectedCallback;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    public void Setup(TaskData taskData, Action<TaskData> callback)
    {
        associatedTask = taskData;
        onTaskSelectedCallback = callback;
        taskNameText.text = taskData.taskName;
    }

    private void OnButtonClicked()
    {
        onTaskSelectedCallback?.Invoke(associatedTask);
    }
}
