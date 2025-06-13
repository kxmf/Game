using UnityEngine;

public class TaskMarker : MonoBehaviour
{
    [SerializeField]
    private TaskData taskData; // Статические данные

    [Header("Иконки состояний")]
    [SerializeField]
    private Sprite availableSprite; // !

    [SerializeField]
    private Sprite inProgressSprite; // ?

    [SerializeField]
    private Sprite completedSprite; // Галочка/серая иконка

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        if (taskData == null)
            return;

        // Получаем прогресс из GameManager
        MarkerProgressData progress = ProgressManager.instance.GetTaskProgress(taskData.taskId);

        if (progress == null)
        {
            spriteRenderer.sprite = availableSprite;
            return;
        }

        switch (progress.status)
        {
            case MarkerStatus.Available:
                spriteRenderer.sprite = availableSprite;
                break;
            case MarkerStatus.InProgress:
                spriteRenderer.sprite = inProgressSprite;
                break;
            case MarkerStatus.Completed:
                spriteRenderer.sprite = completedSprite;
                break;
        }
    }

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
