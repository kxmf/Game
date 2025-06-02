using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() => CustomSceneManager.LoadScene("House");

    public void ExitGame() => Application.Quit();
}
