using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Required for updating the UI text

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels & Text")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI objectiveText; // Drag your UI Text asset here

    [Header("Objective Settings")]
    public int totalTargetsNeeded = 3;
    private int targetsCompleted = 0;

    [Header("Timer Settings")]
    public float bombTimerDuration = 10f; // Seconds before detonation
    private float currentTimer = 0f;
    private bool isTimerRunning = false;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f; 
        gameEnded = false;
        isTimerRunning = false;
        targetsCompleted = 0;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reset UI Display text layout at start
        UpdateObjectiveUI();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        // Handle the countdown sequence once all 3 bombs are active
        if (isTimerRunning && !gameEnded)
        {
            currentTimer -= Time.deltaTime;
            
            if (objectiveText != null)
            {
                objectiveText.text = $"DETONATION IN: {currentTimer:F1}s\nRUN AWAY!";
                objectiveText.color = Color.red;
            }

            if (currentTimer <= 0f)
            {
                TriggerWinCondition();
            }
        }
    }

    // This replaces your old single OnBombPlanted call
    public void RegisterTargetCleared()
    {
        if (gameEnded) return;

        targetsCompleted++;
        UpdateObjectiveUI();

        // Check if all three points are planted
        if (targetsCompleted >= totalTargetsNeeded)
        {
            StartBombCountdown();
        }
    }

    private void UpdateObjectiveUI()
    {
        if (objectiveText != null && !isTimerRunning)
        {
            objectiveText.text = $"Targets Planted: {targetsCompleted} / {totalTargetsNeeded}";
            objectiveText.color = Color.white;
        }
    }

    private void StartBombCountdown()
    {
        currentTimer = bombTimerDuration;
        isTimerRunning = true;
        Debug.Log("ALL BOMBS PLANTED! Timer started...");
    }

    public void TriggerLossCondition()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void TriggerWinCondition()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("BOOM! Target eliminated cleanly! Victory Screen Loading...");
        
        // Load your victory screen or return to Main Menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}