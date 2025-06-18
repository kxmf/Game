using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    private GameObject mainMenuPanel;

    [SerializeField]
    private GameObject floorSelectionPanel;

    [Header("Floor selection buttons")]
    [SerializeField]
    private Button floor0Button;

    [SerializeField]
    private Button floor1Button;

    [SerializeField]
    private Button floor2Button;

    [SerializeField]
    private Button floor3Button;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button settingsButton_MainPanel;

    [SerializeField]
    private Button settingsButton_FloorPanel;

    [SerializeField]
    private Button quitButton;

    [SerializeField]
    private Button backButton;

    void Start()
    {
        var uiManager = UIManager.instance;
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found");
            return;
        }

        uiManager.RegisterMainMenuView(this);

        startButton.onClick.AddListener(uiManager.OnStartButtonPressed);
        settingsButton_MainPanel.onClick.AddListener(uiManager.OnSettingsButtonPressed);
        quitButton.onClick.AddListener(uiManager.OnQuitButtonPressed);
        backButton.onClick.AddListener(uiManager.OnBackButtonPressed);
        settingsButton_FloorPanel.onClick.AddListener(uiManager.OnSettingsButtonPressed);

        floor0Button.onClick.AddListener(() => uiManager.OnFloorButtonPressed(0));
        floor1Button.onClick.AddListener(() => uiManager.OnFloorButtonPressed(1));
        floor2Button.onClick.AddListener(() => uiManager.OnFloorButtonPressed(2));
        floor3Button.onClick.AddListener(() => uiManager.OnFloorButtonPressed(3));
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        floorSelectionPanel.SetActive(false);
    }

    public void ShowFloorSelection()
    {
        mainMenuPanel.SetActive(false);
        floorSelectionPanel.SetActive(true);
    }

    public void UpdateFloorButtons(int unlockedFloors)
    {
        floor0Button.interactable = unlockedFloors >= 0;
        floor1Button.interactable = unlockedFloors >= 1;
        floor2Button.interactable = unlockedFloors >= 2;
        floor3Button.interactable = unlockedFloors >= 3;
    }
}
