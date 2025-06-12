using UnityEngine;
using UnityEngine.UI;

public class GameSceneView : MonoBehaviour
{
    [Header("Кнопки управления сценой")]
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button settingsButton;

    void Start()
    {
        var uiManager = UIManager.instance;
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found");
            return;
        }

        uiManager.RegisterGameSceneView(this);

        backButton.onClick.AddListener(uiManager.OnGameSceneBackButtonPressed);

        settingsButton.onClick.AddListener(uiManager.OnSettingsButtonPressed);
    }
}
