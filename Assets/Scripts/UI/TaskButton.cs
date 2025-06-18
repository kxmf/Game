using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI taskNameText;

    [SerializeField]
    private Image buttonBackground;

    [Header("Спрайты статусов")]
    [SerializeField]
    private Sprite availableSprite;

    [SerializeField]
    private Sprite inProgressSprite;

    [SerializeField]
    private Sprite completedSprite;

    private Button button;
    private TaskData associatedTask;
    private Action<TaskData> onTaskSelectedCallback;
    private bool isInitialized = false;

    void Awake()
    {
        InitializeButton();
    }

    private void InitializeButton()
    {
        if (isInitialized)
            return;

        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
            isInitialized = true;
        }
        else
            Debug.LogError("На префабе TaskButton не найден компонент Button!", gameObject);
    }

    public void Setup(TaskData taskData, TaskStatus status, Action<TaskData> callback)
    {
        InitializeButton();

        associatedTask = taskData;
        onTaskSelectedCallback = callback;
        taskNameText.text = taskData.taskName;
        button.interactable = true;

        switch (status)
        {
            case TaskStatus.Available:
                buttonBackground.sprite = availableSprite;
                break;
            case TaskStatus.InProgress:
                buttonBackground.sprite = inProgressSprite;
                break;
            case TaskStatus.Completed:
                buttonBackground.sprite = completedSprite;
                break;
        }
    }

    private void OnButtonClicked()
    {
        onTaskSelectedCallback?.Invoke(associatedTask);
    }
}
