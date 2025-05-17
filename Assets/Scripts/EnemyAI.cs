using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAwareness), typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private EnemyAwareness awareness;
    private NavMeshAgent agent;
    private Transform playerTransform;

    private void Awake()
    {
        awareness = GetComponent<EnemyAwareness>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<PlayerMove>()?.transform;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        if (awareness.isAggro)
            agent.SetDestination(playerTransform.position);
        else
            agent.ResetPath();
    }
}