using UnityEngine;

public class BombTarget : MonoBehaviour
{
    [Header("Bomb Settings")]
    public GameObject bombVisualPrefab; // Drop a small proxy model/cube here to represent the planted C4
    public Transform plantLocation;      // Where the bomb model should physically snap to
    
    private bool isBombPlanted = false;
    private bool playerInRange = false;

    private void Update()
    {
        // If player is close by and presses the interaction key 'E'
        if (playerInRange && !isBombPlanted && Input.GetKeyDown(KeyCode.E))
        {
            PlantBomb();
        }
    }

    private void PlantBomb()
    {
        isBombPlanted = true;

        // Spawn visual feedback of the bomb if a prefab was provided
        if (bombVisualPrefab != null && plantLocation != null)
        {
            Instantiate(bombVisualPrefab, plantLocation.position, plantLocation.rotation);
        }

        // Notify the game manager that the objective is complete
        GameManager.Instance.OnBombPlanted();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press 'E' to plant explosive charges.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
