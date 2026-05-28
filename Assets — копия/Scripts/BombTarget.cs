using UnityEngine;

public class BombObjective : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Drag your raw Bomb Prefab from the Project window here.")]
    public GameObject bombPrefab;

    [Tooltip("Drag the Empty GameObject you created as a placement marker here.")]
    public Transform spawnMarker;

    private bool playerInsideZone = false;
    private bool isPlanted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideZone = true;
            Debug.Log("Press [E] to Plant Bomb on Target");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideZone = false;
        }
    }

    private void Update()
    {
        // Check if player presses E while inside the zone
        if (playerInsideZone && !isPlanted && Input.GetKeyDown(KeyCode.E))
        {
            PlantBomb();
        }
    }

    private void PlantBomb()
    {
        isPlanted = true;
        Debug.Log($"Bomb successfully attached to: {gameObject.name}");

        // RESOLUTION: Spawn the bomb at the custom empty gameobject marker position and rotation
        if (bombPrefab != null && spawnMarker != null)
        {
            Instantiate(bombPrefab, spawnMarker.position, spawnMarker.rotation);
        }
        else if (bombPrefab != null)
        {
            // Emergency fallback: if you forgot to create a marker, spawn it at the zone center
            Instantiate(bombPrefab, transform.position, transform.rotation);
            Debug.LogWarning($"[BOMB WARNING] Spawn Marker missing on {gameObject.name}. Spawning at default center.");
        }

        // Notify the GameManager to advance the score/timer
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterTargetCleared();
        }
    }
}