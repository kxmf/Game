using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private MainMenuView mainMenuView;
    private GameSceneView gameSceneView;

    public enum MainMenuState
    {
        Main,
        FloorSelection,
    }

    private bool isFloorSelectionVisible = false;

    public static event Action<bool> OnUIStateChanged;

    private MainMenuState targetStateOnLoad = MainMenuState.Main;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Debug.Log("UIManager Initialized!");
    }

    public void SetTargetMainMenuState(MainMenuState state)
    {
        targetStateOnLoad = state;
    }

    public void RegisterMainMenuView(MainMenuView view)
    {
        mainMenuView = view;
        switch (targetStateOnLoad)
        {
            case MainMenuState.Main:
                mainMenuView.ShowMainMenu();
                break;
            case MainMenuState.FloorSelection:
                mainMenuView.ShowFloorSelection();
                mainMenuView.UpdateFloorButtons(GameManager.instance.UnlockedFloorsCount);
                break;
        }
        targetStateOnLoad = MainMenuState.Main;
    }

    public void RegisterGameSceneView(GameSceneView view)
    {
        gameSceneView = view;
        Debug.Log("GameSceneView зарегистрирован в UIManager.");
    }

    public void OnStartButtonPressed()
    {
        if (mainMenuView == null)
            return;
        mainMenuView.ShowFloorSelection();
        mainMenuView.UpdateFloorButtons(GameManager.instance.UnlockedFloorsCount);
        isFloorSelectionVisible = true;
    }

    public void OnFloorButtonPressed(int floorIndex)
    {
        Debug.Log($"Player selected Floor {floorIndex}. Loading scene...");
        GameManager.instance.LoadGameScene(floorIndex);
    }

    public void OnQuitButtonPressed()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnBackButtonPressed()
    {
        if (mainMenuView == null)
            return;
        mainMenuView.ShowMainMenu();
        isFloorSelectionVisible = false;
    }

    public void OnGameSceneBackButtonPressed()
    {
        SetTargetMainMenuState(MainMenuState.FloorSelection);
        GameManager.instance.LoadMainMenu();
    }

    public void OnSettingsButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            if (isFloorSelectionVisible)
                SetTargetMainMenuState(MainMenuState.FloorSelection);
            else
                SetTargetMainMenuState(MainMenuState.Main);
        }

        GameManager.instance.GoToSettings();
    }

    public void ShowDialogue(NPCData npcData, List<TaskData> tasks)
    {
        OnUIStateChanged?.Invoke(true);
        gameSceneView.ShowDialogue(npcData, tasks);
    }

    public void HideDialogueWindow()
    {
        gameSceneView.HideDialogue();
        OnUIStateChanged?.Invoke(false);
    }

    public void ShowTaskWindow(TaskData task, string savedCode)
    {
        OnUIStateChanged?.Invoke(true);
        gameSceneView.ShowTask(task, savedCode);
    }

    public void HideTaskWindow()
    {
        gameSceneView.HideTask();
        OnUIStateChanged?.Invoke(false);
    }
}
