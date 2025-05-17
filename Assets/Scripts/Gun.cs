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
    public GameObject weaponIconUI;

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

        // desativa até o jogador pegar a arma
        enabled = false;
        if (weaponIconUI != null)
            weaponIconUI.SetActive(false);
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
        enabled = true;
        if (weaponIconUI != null)
            weaponIconUI.SetActive(true);
    }

    private void Fire()
    {
        // Remove possíveis nulos antes de tudo
        enemyManager.enemiesInTrigger.RemoveAll(e => e == null);

        // Faz um snapshot (array) para não iterar na lista original:
        Enemy[] snapshot = enemyManager.enemiesInTrigger.ToArray();

        foreach (var enemy in snapshot)
        {
            if (enemy == null)
                continue;

            Vector3 dir = (enemy.transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, range * 1.5f, raycastLayerMask))
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
        if (other.TryGetComponent<Enemy>(out var enemy))
            enemyManager.AddEnemy(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy))
            enemyManager.RemoveEnemy(enemy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gunShotRadius);
    }
}
