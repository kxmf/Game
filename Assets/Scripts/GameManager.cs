using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private SaveManager saveManager;

    public int UnlockedFloorsCount { get; private set; }

    public int CurrentFloorIndex { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        saveManager = GetComponent<SaveManager>();

        Debug.Log("GameManager Initialized!");
    }

    void Start()
    {
        UnlockedFloorsCount = saveManager.LoadProgress();

        if (SceneManager.GetActiveScene().name == "InitScene")
            LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadGameScene(int floorIndex)
    {
        Debug.Log($"Loading Game Scene for floor {floorIndex}...");
        CurrentFloorIndex = floorIndex;
        SceneManager.LoadScene("GameScene");
    }

    private void HandleEscapeKey()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "GameScene":
                Debug.Log("Escape pressed on GameScene. Returning to Main Menu.");
                LoadMainMenu();
                break;
            case "MainMenu":
                UIManager.instance.OnBackButtonPressed();
                Debug.Log("Escape pressed on MainMenu.");
                break;

            // case "SettingsScene":
            //     LoadMainMenu();
            //     break;

            default:
                break;
        }
    }
}
