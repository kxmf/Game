using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() => CustomSceneManager.LoadScene("House");

    public void OpenDevelopersList() => Debug.Log("Dev list");

    public void ExitGame() => Application.Quit();
}
