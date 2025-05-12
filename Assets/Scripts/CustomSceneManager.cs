using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    private static readonly Stack<string> _scenes = new();

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Menu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LoadPreviousScene();
    }

    public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        _scenes.Push(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneName, mode);
    }

    public static void LoadPreviousScene()
    {
        if (!string.IsNullOrEmpty(_scenes.Peek()))
            SceneManager.LoadScene(_scenes.Pop());
    }
}
