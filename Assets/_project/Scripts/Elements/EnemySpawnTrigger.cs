using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public Enemy enemyPrefab;

    public List<Transform> _enemySpawnPoints = new List<Transform>();
    private Level _level;

    private void Start()
    {
        _level = GetComponentInParent<Level>();
        foreach (Transform t in transform)
        {
            _enemySpawnPoints.Add(t);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnEnemies();
            gameObject.SetActive(false);
        }
    }

    private void SpawnEnemies()
    {
        foreach (var sp in _enemySpawnPoints)
        {
            var newEnemy = Instantiate(enemyPrefab, _level.transform);
            newEnemy.transform.position = sp.position;
            newEnemy.transform.localScale = new Vector3(1,0,1);
            newEnemy.StartEnemy(_level.player);
            newEnemy.transform.DOScaleY(1, 1f).OnComplete(newEnemy.StartMoving);
            _level.EnemySpawned(newEnemy);
        }
    }
}
