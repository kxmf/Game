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

    public void ResetProgress()
    {
        if (GameManager.instance != null)
            GameManager.instance.ResetGameProgress();
        else
            Debug.LogError("GameManager не найден! Не могу сбросить прогресс.");
    }
}
