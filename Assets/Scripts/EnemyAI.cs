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
    private Animator anim;

    private void Awake()
    {
        awareness = GetComponent<EnemyAwareness>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerTransform = FindFirstObjectByType<PlayerMove>()?.transform;
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
            anim.SetBool("run", true);
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