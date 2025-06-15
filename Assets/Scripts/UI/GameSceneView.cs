using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneView : MonoBehaviour
{
    [Header("Кнопки управления сценой")]
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button settingsButton;

    [SerializeField]
    private DialogueWindow dialogueWindow;

    [SerializeField]
    private TaskWindow taskWindow;

    void Start()
    {
        var uiManager = UIManager.instance;
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found");
            return;
        }

        uiManager.RegisterGameSceneView(this);

        backButton.onClick.AddListener(uiManager.OnGameSceneBackButtonPressed);

        settingsButton.onClick.AddListener(uiManager.OnSettingsButtonPressed);

        dialogueWindow.Hide();
        taskWindow.gameObject.SetActive(false);
    }

    public void ShowDialogue(NPCData npcData, List<TaskData> tasks)
    {
        dialogueWindow.Show(npcData, tasks);
    }

    public void ShowTask(TaskData task, string savedCode)
    {
        taskWindow.Show(task, savedCode);
    }

    public void HideTask()
    {
        taskWindow.gameObject.SetActive(false);
    }
}
