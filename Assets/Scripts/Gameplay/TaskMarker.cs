using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class TaskMarker : MonoBehaviour
{
    [Header("Данные NPC")]
    [SerializeField]
    private NPCData npcData;

    [Header("Спрайты состояний")]
    [SerializeField]
    private Sprite availableSprite;

    [SerializeField]
    private Sprite inProgressSprite;

    [SerializeField]
    private Sprite completedSprite;

    private SpriteRenderer spriteRenderer;
    private Collider2D myCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();

        if (npcData == null)
        {
            Debug.LogError(
                $"На маркере '{this.gameObject.name}' не назначены данные NPC (NPCData)!",
                this.gameObject
            );
            gameObject.SetActive(false);
            return;
        }

        UpdateIcon();
        GameManager.OnTaskStatusChanged += HandleTaskStatusChanged;
    }

    public void UpdateIcon()
    {
        if (npcData == null || npcData.availableTasks.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        bool hasAvailableTasks = false;
        bool hasInProgressTasks = false;
        int completedTasksCount = 0;

        foreach (var task in npcData.availableTasks)
        {
            TaskProgressData progress = GameManager.instance.GetTaskProgress(task.taskId);

            if (progress == null || progress.status == TaskStatus.Available)
                hasAvailableTasks = true;
            else if (progress.status == TaskStatus.InProgress)
                hasInProgressTasks = true;
            else if (progress.status == TaskStatus.Completed)
                completedTasksCount++;
        }

        if (hasAvailableTasks)
            spriteRenderer.sprite = availableSprite;
        else if (hasInProgressTasks)
            spriteRenderer.sprite = inProgressSprite;
        else if (completedTasksCount == npcData.availableTasks.Count)
            spriteRenderer.sprite = completedSprite;
        else
            gameObject.SetActive(false);

        SetInteractable(true);
    }

    private void SetInteractable(bool interactable)
    {
        myCollider.enabled = interactable;
        gameObject.SetActive(interactable);
    }

    private void OnMouseDown()
    {
        GameManager.instance.StartDialogueWithNPC(npcData);
    }

    private void OnMouseEnter()
    {
        if (myCollider.enabled)
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    private void OnMouseExit()
    {
        transform.localScale = Vector3.one;
    }

    private void HandleTaskStatusChanged(int changedTaskId)
    {
        if (npcData == null)
            return;

        foreach (var task in npcData.availableTasks)
        {
            if (task.taskId == changedTaskId)
            {
                Debug.Log(
                    $"Маркер для NPC '{npcData.npcName}' услышал событие для задачи '{changedTaskId}' и обновляется."
                );
                UpdateIcon();
                break;
            }
        }
    }
}
