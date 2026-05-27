using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    [Tooltip("Drag your GameOverPanel Gameobject here from the Hierarchy")]
    public GameObject gameOverPanel;

    [Header("Prototype Settings")]
    [SerializeField] private float restartDelay = 3f;
    [SerializeField] private float winDelay = 3f;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // FIX: Restoring the missing method for BombTarget.cs
    public void OnBombPlanted()
    {
        if (gameEnded) return;
        
        Debug.Log("Bomb successfully planted! Get ready for detonation...");
        Invoke(nameof(TriggerWinCondition), winDelay);
    }

    public void TriggerLossCondition()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("GAME OVER: Player spotted!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Invoke(nameof(RestartLevel), restartDelay);
    }

    private void TriggerWinCondition()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("VICTORY: Target eliminated cleanly!");
        Invoke(nameof(RestartLevel), restartDelay);
    }

    private void RestartLevel()
    {
        gameEnded = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}