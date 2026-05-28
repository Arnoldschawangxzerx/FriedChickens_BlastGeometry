using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    [Tooltip("How long the guard waits at each patrol point before moving on.")]
    public float waitTimeAtPoints = 2f;
    
    private int currentPointIndex = 0;
    private Animator guardAnim;
    private NavMeshAgent agent; 
    private bool isWaiting = false;
    private bool isFrozen = false; // Flag to prevent movement once player is spotted

    void Start()
    {
        guardAnim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        SendAgentToNextPoint();
    }

    void Update()
    {
        // Prevent path updates if missing, waiting, or frozen
        if (agent == null || isWaiting || isFrozen) return;

        // Check if the enemy reached their destination
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAndMoveRoutine());
        }

        // Animate based on physical movement speed
        if (guardAnim != null)
        {
            float speedRatio = agent.velocity.magnitude > 0.1f ? 1f : 0f;
            guardAnim.SetFloat("ForwardSpeed", speedRatio);
        }
    }

    IEnumerator WaitAndMoveRoutine()
    {
        isWaiting = true;
        
        if (guardAnim != null) guardAnim.SetFloat("ForwardSpeed", 0f);
        
        yield return new WaitForSeconds(waitTimeAtPoints);
        
        if (isFrozen) yield break; // Safety exit if caught during a pause
        
        IncrementPointIndex();
        SendAgentToNextPoint();
        
        isWaiting = false;
    }

    void SendAgentToNextPoint()
    {
        if (patrolPoints.Length == 0 || agent == null || isFrozen) return;
        agent.destination = patrolPoints[currentPointIndex].position;
    }

    void IncrementPointIndex()
    {
        if (patrolPoints.Length == 0) return;
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    // CRITICAL: This is the exact method Detection.cs is looking for!
    public void FreezeGuard()
    {
        isFrozen = true;
        
        if (agent != null)
        {
            agent.isStopped = true;       // Halts pathfinding navigation mechanics
            agent.velocity = Vector3.zero; // Dampens all remaining physical push momentum
        }

        if (guardAnim != null)
        {
            guardAnim.SetFloat("ForwardSpeed", 0f); // Forces the visual state to Idle
        }
    }
}