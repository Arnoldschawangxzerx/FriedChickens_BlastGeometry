using UnityEngine;

public class Detection : MonoBehaviour
{
    [Header("Detection Settings")]
    public float maxDetectionDistance = 8f; // Distance threshold
    public float viewAngle = 90f;          // Frontal arc width

    [Header("FORCED REFERENCE - DRAG PLAYER HERE")]
    public Transform playerTransform; // Direct link slot

    [Header("Visual Feedback")]
    public Light detectionSpotlight; 
    public Color normalColor = Color.green;
    public Color alertColor = Color.red;

    private void Start()
    {
        if (detectionSpotlight != null)
        {
            detectionSpotlight.color = normalColor;
        }
    }

    private void Update()
    {
        // Absolute safety check: If you forgot to drag the player in, complain out loud!
        if (playerTransform == null) 
        {
            Debug.LogError($"[DETECTION ERROR] Player Transform is MISSING on {gameObject.name}! Drag your Player object into the script slot!");
            return;
        }

        // 1. DISTANCE CALCULATION (Pure math, no physics engines involved)
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= maxDetectionDistance)
        {
            // 2. ANGLE CALCULATION (Pure vector math)
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer < viewAngle / 2f)
            {
                TriggerAlert();
            }
        }
    }

    private void TriggerAlert()
    {
        if (detectionSpotlight != null)
        {
            detectionSpotlight.color = alertColor;
        }

        EnemyPatrol patrolScript = GetComponent<EnemyPatrol>();
        if (patrolScript != null)
        {
            patrolScript.FreezeGuard();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerLossCondition();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;
        
        Gizmos.DrawRay(transform.position + Vector3.up, leftBoundary * maxDetectionDistance);
        Gizmos.DrawRay(transform.position + Vector3.up, rightBoundary * maxDetectionDistance);
    }
}