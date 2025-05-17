using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EnemyAwareness : MonoBehaviour
{
    public float awarenessRadius = 10f;
    public bool isAggro = false;
    public Material aggroMaterial;

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

        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (!isAggro && dist < awarenessRadius)
            isAggro = true;

        meshRenderer.material = isAggro && aggroMaterial != null ? aggroMaterial : originalMaterial;
    }
}