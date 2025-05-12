using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        CustomSceneManager.LoadScene("House");
    }

    public void OpenSettings()
    {
        CustomSceneManager.LoadScene("Settings");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
