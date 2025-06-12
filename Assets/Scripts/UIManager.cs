using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private MainMenuView mainMenuView;

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

    public void OnStartButtonPressed()
    {
        if (mainMenuView == null)
            return;
        mainMenuView.ShowFloorSelection();
        mainMenuView.UpdateFloorButtons(GameManager.instance.UnlockedFloorsCount);
    }

    public void OnBackButtonPressed()
    {
        if (mainMenuView == null)
            return;
        mainMenuView.ShowMainMenu();
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

    public void HandleEscapeOnMainMenu()
    {
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (currentSceneName == "MenuScene")
            OnQuitButtonPressed();
        else if (currentSceneName == "GameScene")
            OnBackButtonPressed();
    }
}
