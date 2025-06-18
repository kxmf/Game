using UnityEngine;

public class SettingsSceneController : MonoBehaviour
{
    public void GoBack()
    {
        if (GameManager.instance != null)
            GameManager.instance.ReturnFromSettings();
        else
            Debug.LogError("GameManager не найден! Не могу вернуться назад.");
    }
}
