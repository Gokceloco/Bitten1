using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();
    public void StartLevel(Player player)
    {
        _enemies = GetComponentsInChildren<Enemy>().ToList();

        foreach (var enemy in _enemies)
        {
            enemy.StartEnemy(player);
        }
    }
}
