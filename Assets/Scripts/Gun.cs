using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class Gun : MonoBehaviour
{
    [Header("Settings")]
    public float range = 20f;
    public float verticalRange = 20f;
    public float fireRate = 1f;
    public float bigDamage = 2f;
    public float smallDamage = 1f;
    public float gunShotRadius = 20f;
    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;

    [Header("References")]
    public EnemyManager enemyManager;

    private BoxCollider gunTrigger;
    private float nextFireTime;
    private bool hasWeapon = false;

    private void Awake()
    {
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.isTrigger = true;
        gunTrigger.size = new Vector3(1f, verticalRange, range);
        gunTrigger.center = new Vector3(0f, 0f, range * 0.5f);

        if (enemyManager == null)
            enemyManager = FindObjectOfType<EnemyManager>();

        // Começa desativado até o jogador pegar a arma
        enabled = false;
    }

    private void Update()
    {
        if (!hasWeapon) return;

        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }

    public void Equip()
    {
        hasWeapon = true;
        enabled = true; // ativa o script
    }

    private void Fire()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask);
        foreach (var col in hits)
        {
            var awareness = col.GetComponent<EnemyAwareness>();
            if (awareness)
                awareness.TriggerAggro();
        }

        foreach (var enemy in enemyManager.enemiesInTrigger)
        {
            if (enemy == null) continue;
            Vector3 dir = (enemy.transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, dir, out var hit, range * 1.5f, raycastLayerMask))
            {
                if (hit.transform == enemy.transform)
                {
                    float dist = Vector3.Distance(transform.position, enemy.transform.position);
                    float dmg = (dist <= range * 0.5f) ? bigDamage : smallDamage;
                    enemy.TakeDamage(dmg);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy) enemyManager.AddEnemy(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy) enemyManager.RemoveEnemy(enemy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gunShotRadius);
    }
}
