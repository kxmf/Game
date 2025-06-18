using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : MonoBehaviour
{
    [Header("Ссылки на UI элементы")]
    [SerializeField]
    private Image npcPortraitImage;

    [SerializeField]
    private TextMeshProUGUI npcNameText;

    [SerializeField]
    private Button closeButton;

    [Header("Список Задач")]
    [SerializeField]
    private Transform tasksContentParent;

    [SerializeField]
    private GameObject taskButtonPrefab;

    void Awake()
    {
        closeButton.onClick.AddListener(() => UIManager.instance.HideDialogueWindow());
    }

    public void Show(NPCData npcData, List<TaskData> tasksToShow)
    {
        npcNameText.text = npcData.npcName;
        if (npcData.npcPortrait != null)
            npcPortraitImage.sprite = npcData.npcPortrait;

        foreach (Transform child in tasksContentParent)
            Destroy(child.gameObject);

        foreach (var task in tasksToShow)
        {
            GameObject buttonGO = Instantiate(taskButtonPrefab, tasksContentParent);
            TaskButton taskButton = buttonGO.GetComponent<TaskButton>();
            taskButton.Setup(task, OnTaskSelected);
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnTaskSelected(TaskData selectedTask)
    {
        Debug.Log($"Игрок выбрал задачу: {selectedTask.taskName}");
        GameManager.instance.StartTask(selectedTask);
        Hide();
    }
}
