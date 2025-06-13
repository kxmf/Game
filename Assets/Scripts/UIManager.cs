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
    }

    public void OnGameSceneBackButtonPressed()
    {
        SetTargetMainMenuState(MainMenuState.FloorSelection);
        GameManager.instance.LoadMainMenu();
    }

    public void OnSettingsButtonPressed()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void ShowDialogue(NPCData npcData, List<TaskData> tasks)
    {
        gameSceneView.ShowDialogue(npcData, tasks);
    }
}
