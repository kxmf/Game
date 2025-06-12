using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private MainMenuView mainMenuView;
    private GameSceneView gameSceneView;

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

    public void RegisterMainMenuView(MainMenuView view)
    {
        mainMenuView = view;
        mainMenuView.ShowMainMenu();
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
        GameManager.instance.LoadMainMenu();
    }

    public void OnSettingsButtonPressed()
    {
        SceneManager.LoadScene("SettingsScene");
    }
}
