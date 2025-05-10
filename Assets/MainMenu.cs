using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("House");
    }

    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
