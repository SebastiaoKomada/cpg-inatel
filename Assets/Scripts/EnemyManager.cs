using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [HideInInspector]
    public List<Enemy> enemiesInTrigger = new List<Enemy>();

    public void AddEnemy(Enemy enemy)
    {
        if (!enemiesInTrigger.Contains(enemy))
            enemiesInTrigger.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (enemiesInTrigger.Contains(enemy))
            enemiesInTrigger.Remove(enemy);
    }
}