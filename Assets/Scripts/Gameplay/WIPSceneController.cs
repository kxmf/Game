using UnityEngine;

public class WIPSceneController : MonoBehaviour
{
    public void GoBack()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.SetTargetMainMenuState(UIManager.MainMenuState.FloorSelection);

            if (GameManager.instance != null)
                GameManager.instance.LoadMainMenu();
        }
    }
}
