using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void RetryGame()
    {
        // Unfreeze time globally before requesting the scene swap
        Time.timeScale = 1f; 

        // Reload the current active scene layout fresh
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(mainMenuSceneName);
    }
}