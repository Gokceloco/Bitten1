using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Player _player;

    public EnemyDetector enemyDetector;


    public void StartGrenade(Player player)
    {
        _player = player;
        enemyDetector.transform.SetParent(null);
    }

    private void Update()
    {
        enemyDetector.transform.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Enemy"))
        {
            Explode();
        }
        if (other.CompareTag("YBorder"))
        {
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        _player.gameDirector.fXManager.PlayGrenadeExplosionFX(transform.position);
        _player.cameraHolder.ShakeCamera(1.2f, .75f);
        _player.gameDirector.audioManager.PlayExplosionSFX();
        foreach (var e in enemyDetector.enemiesInRange)
        {
            e.GetHit(1000, (e.transform.position - transform.position) + Vector3.up, 500);
        }
        
        Destroy(gameObject);
    }
}
