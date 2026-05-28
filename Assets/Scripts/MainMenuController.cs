using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Configuration")]
    [Tooltip("Type the exact name of your main gameplay level scene file here.")]
    [SerializeField] private string gameplaySceneName = "GameplayScene";

    private void Start()
    {
        // Explicitly ensure the cursor is completely visible and unlocked on the menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Ensure time is running normally
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        Debug.Log("Loading gameplay level...");
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Application closed safely.");
        Application.Quit(); // Closes the final built standalone application executable
    }
}