using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAwareness), typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Wander Settings")]
    public float patrolRadius = 10f;
    public float wanderInterval = 5f;

    private EnemyAwareness awareness;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private Vector3 wanderCenter;
    private float nextWanderTime;

    private void Awake()
    {
        awareness = GetComponent<EnemyAwareness>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<PlayerMove>()?.transform;
        wanderCenter = transform.position;
        nextWanderTime = Time.time + wanderInterval;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        if (awareness.IsAggro())
        {
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);
        }
        else if (!agent.hasPath || agent.remainingDistance < 1f || Time.time >= nextWanderTime)
        {
            Vector3 rnd = Random.insideUnitSphere * patrolRadius + wanderCenter;
            if (NavMesh.SamplePosition(rnd, out var hit, patrolRadius, NavMesh.AllAreas))
            {
                agent.isStopped = false;
                agent.SetDestination(hit.position);
            }
            nextWanderTime = Time.time + wanderInterval;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}