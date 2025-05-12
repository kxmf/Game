using UnityEngine;

public class House : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Settings()
    {
        CustomSceneManager.LoadScene("Settings");
    }

    public void Back()
    {
        CustomSceneManager.LoadPreviousScene();
    }
}
