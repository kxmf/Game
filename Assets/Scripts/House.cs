using UnityEngine;

public class House : MonoBehaviour
{
    public void Settings()
    {
        CustomSceneManager.LoadScene("Settings");
    }

    public void Back()
    {
        CustomSceneManager.LoadPreviousScene();
    }
}
