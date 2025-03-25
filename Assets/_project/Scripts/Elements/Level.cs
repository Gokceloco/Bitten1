using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Player player;
    private List<Enemy> _enemies = new List<Enemy>();

    public float yBorder;

    public float levelTimeInMin;
    public void StartLevel(Player p)
    {
        player = p;
        _enemies = GetComponentsInChildren<Enemy>().ToList();

        foreach (var enemy in _enemies)
        {
            enemy.StartEnemy(p);
            enemy.StartMoving();
        }
    }
    public void StopAllEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.StopEnemy();
        }
    }
    public void EnemySpawned(Enemy enemy)
    {
        _enemies.Add(enemy);
    }
    public void EnemyDied(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }
}
