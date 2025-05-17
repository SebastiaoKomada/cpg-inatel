using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    [Header("Configuração")]
    public float maxHealth = 2f;

    [HideInInspector]
    public EnemyManager enemyManager;

    private float currentHealth;
    private bool isDead = false;
    
    private NavMeshAgent agent;
    private Rigidbody rb;
    private EnemyAI enemyAI;  // assume que existe esse script

    private void Awake()
    {
        currentHealth = maxHealth;
        enemyManager = enemyManager ? enemyManager : FindObjectOfType<EnemyManager>();

        agent   = GetComponent<NavMeshAgent>();
        rb      = GetComponent<Rigidbody>();
        enemyAI = GetComponent<EnemyAI>();

        // prepara para a “queda”, mas sem colisão desativada
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        isDead = true;

        // 1) Remove da lista do manager
        enemyManager.RemoveEnemy(this);

        // 2) Desliga o NavMeshAgent e o EnemyAI para não continuar chamando SetDestination
        if (agent   != null) agent.enabled   = false;
        if (enemyAI != null) enemyAI.enabled = false;

        // 3) Ativa a física de ragdoll simples
        rb.isKinematic = false;
        rb.useGravity  = true;

        // 4) Rotaciona para “deitar” (opcional)
        var e = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(90f, e.y, e.z);

        // 5) Desativa este script para não rodar mais lógica de inimigo vivo
        this.enabled = false;
    }
}
