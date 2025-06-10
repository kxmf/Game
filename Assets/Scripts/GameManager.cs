using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        Debug.Log("GameManager Initialized!");
    }

    void Start()
    {
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
        // SceneManager.LoadScene("GameScene");
    }
}
