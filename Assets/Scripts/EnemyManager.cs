// EnemyManager.cs
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [HideInInspector]
    public List<Enemy> enemiesInTrigger = new List<Enemy>();

    public void AddEnemy(Enemy e)
    {
        if (!enemiesInTrigger.Contains(e))
            enemiesInTrigger.Add(e);
    }

    public void RemoveEnemy(Enemy e)
    {
        enemiesInTrigger.Remove(e);
    }

    private void LateUpdate()
    {
        // limpa referências nulas (inimigos já destruídos)
        enemiesInTrigger.RemoveAll(e => e == null);
    }
}
