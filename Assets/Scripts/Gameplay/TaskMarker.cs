using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class TaskMarker : MonoBehaviour
{
    [Header("Данные NPC")]
    [SerializeField]
    private NPCData npcData;

    [Header("Спрайты состояний")]
    [SerializeField]
    private Sprite notAvailableSprite;

    [SerializeField]
    private Sprite availableSprite;

    [SerializeField]
    private Sprite inProgressSprite;

    [SerializeField]
    private Sprite completedSprite;

    private SpriteRenderer spriteRenderer;
    private Collider2D myCollider;
    private bool accessable = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();

        if (npcData == null)
        {
            Debug.LogError(
                $"На маркере '{gameObject.name}' не назначены данные NPC (NPCData)!",
                gameObject
            );
            gameObject.SetActive(false);
            return;
        }

        UpdateIcon();
    }

    private void OnEnable()
    {
        GameManager.OnTaskStatusChanged += HandleTaskStatusChanged;
    }

    private void OnDisable()
    {
        GameManager.OnTaskStatusChanged -= HandleTaskStatusChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnTaskStatusChanged -= HandleTaskStatusChanged;
    }

    public void UpdateIcon()
    {
        if (npcData == null || npcData.availableTasks.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        int notAvailableCount = 0;
        int availableCount = 0;
        int inProgressCount = 0;
        int completedCount = 0;
        int totalTasks = npcData.availableTasks.Count;

        foreach (var task in npcData.availableTasks)
        {
            var progress = GameManager.instance.GetTaskProgressData(task.taskId);

            switch (progress.status)
            {
                case TaskStatus.NotAvailable:
                    notAvailableCount++;
                    break;
                case TaskStatus.Available:
                    availableCount++;
                    break;
                case TaskStatus.InProgress:
                    inProgressCount++;
                    break;
                case TaskStatus.Completed:
                    completedCount++;
                    break;
            }
        }

        accessable = true;

        if (completedCount == totalTasks)
            spriteRenderer.sprite = completedSprite;
        else if (inProgressCount > 0 || completedCount > 0)
            spriteRenderer.sprite = inProgressSprite;
        else if (availableCount > 0)
            spriteRenderer.sprite = availableSprite;
        else
        {
            accessable = false;
            spriteRenderer.sprite = notAvailableSprite;
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !accessable)
            return;
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
