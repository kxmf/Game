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

    [Header("Общие кнопки сцены")]
    [SerializeField]
    private GameObject backButtonObject;

    [SerializeField]
    private GameObject settingsButtonObject;

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

        UIManager.instance.HideDialogueWindow();
        taskWindow.gameObject.SetActive(false);
    }

    public void ShowDialogue(NPCData npcData, List<TaskData> tasks)
    {
        dialogueWindow.Show(npcData, tasks);
    }

    public void HideDialogue()
    {
        dialogueWindow.Hide();
    }

    public void ShowTask(TaskData task, string savedCode)
    {
        SetActive(false);
        taskWindow.Show(task, savedCode);
    }

    public void HideTask()
    {
        SetActive(true);
        taskWindow.gameObject.SetActive(false);
    }

    private void SetActive(bool isActive)
    {
        if (backButtonObject != null)
            backButtonObject.SetActive(isActive);
        if (settingsButtonObject != null)
            settingsButtonObject.SetActive(isActive);
    }
}
