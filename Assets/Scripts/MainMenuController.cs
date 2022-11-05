using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string levelToLoad;
    [SerializeField] GameObject settingsWindow;

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void GetSettings()
    {
        settingsWindow.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
