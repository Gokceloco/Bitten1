using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Player player;
    public Bullet bulletPrefab;
    public Transform shootPos;

    public float attackRate;

    private float _lastShootTime;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time - _lastShootTime > attackRate)
        {
            Shoot();            
        }
    }

    private void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab,
            player.gameDirector.levelManager.currentLevel.transform);
        newBullet.transform.position = shootPos.position;
        newBullet.transform.LookAt(transform.position + transform.forward * 10);
        newBullet.StartBullet(this);
        _lastShootTime = Time.time;
    }
}
