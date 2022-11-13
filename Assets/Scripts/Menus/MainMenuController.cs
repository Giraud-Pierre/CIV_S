using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string levelToLoad;
    [SerializeField] private GameObject settingsWindow;

    [SerializeField] private TutorialsList allTutorials = default;

    public void StartGame()
    {
        allTutorials.tutorialsThatHaveBeenSeen = new bool[3]{false, false, false};
        SceneManager.LoadScene(levelToLoad);
    }

    public void GetSettings()
    {
        settingsWindow.SetActive(true);
    }

    // Exit the setting window.
    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
