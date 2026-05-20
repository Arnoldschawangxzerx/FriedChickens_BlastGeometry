using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float viewDistance = 10f;
    [Range(0, 360)] public float viewAngle = 90f;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public Light visionLight;


    public bool canSeePlayer;

    void Update()
    {
        FieldOfViewCheck();

        if (canSeePlayer)
        {
            visionLight.color = Color.red;
        }
        else
        {
            visionLight.color = Color.yellow;
        }
    }

    private void FieldOfViewCheck()
    {
        Vector3 directionToTarget = (player.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, player.position);

                // Raycast to check for walls/obstacles
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    TriggerAlarm();
                }
                else { canSeePlayer = false; }
            }
            else { canSeePlayer = false; }
        }
        else { canSeePlayer = false; }
    }

    void TriggerAlarm()
    {
        Debug.Log("Intruder Alert! Mission Threat!");
        // Connect this to your Game Manager loss condition
    }
    }
