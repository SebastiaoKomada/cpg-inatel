using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EnemyAwareness : MonoBehaviour
{
    [Header("Detection Settings")]
    public float awarenessRadius = 10f;
    public float fieldOfViewAngle = 110f;
    public LayerMask obstacleMask;
    public Material aggroMaterial;

    private bool triggeredAggro = false;
    private bool isAggro = false;
    private Transform playerTransform;
    private MeshRenderer meshRenderer;
    private Material originalMaterial;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;
        playerTransform = FindObjectOfType<PlayerMove>()?.transform;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, playerTransform.position);

        bool inRange = dist <= awarenessRadius;
        bool inFOV = Vector3.Angle(transform.forward, dirToPlayer) <= fieldOfViewAngle * 0.5f;
        bool hasLOS = inRange && inFOV && !Physics.Raycast(transform.position, dirToPlayer, dist, obstacleMask);

        bool prev = isAggro;
        isAggro = triggeredAggro || hasLOS;
        if (isAggro && hasLOS)
            triggeredAggro = false;

        if (prev != isAggro)
            Debug.Log($"{gameObject.name} aggro: {isAggro}");

        meshRenderer.material = isAggro && aggroMaterial != null ? aggroMaterial : originalMaterial;
    }

    public void TriggerAggro()
    {
        triggeredAggro = true;
        isAggro = true;
    }

    public bool IsAggro() => isAggro;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, awarenessRadius);
        Vector3 fwd = transform.forward * awarenessRadius;
        Quaternion left = Quaternion.Euler(0, -fieldOfViewAngle * 0.5f, 0);
        Quaternion right = Quaternion.Euler(0, fieldOfViewAngle * 0.5f, 0);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, left * fwd);
        Gizmos.DrawRay(transform.position, right * fwd);
    }
}