using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Player player;
    public Bullet bulletPrefab;
    public Transform shootPos;

    public float attackRate;

    private float _lastShootTime;

    public float jitter;

    public WeaponType weaponType;
    private void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab,
            player.gameDirector.levelManager.currentLevel.transform);
        newBullet.transform.position = shootPos.position;
        newBullet.transform.LookAt(shootPos.transform.position + shootPos.transform.forward * 10
            + transform.right * UnityEngine.Random.Range(-jitter, jitter)
            + Vector3.up * UnityEngine.Random.Range(-jitter, jitter));
        newBullet.StartBullet(this);
        _lastShootTime = Time.time;
    }

    private void Update()
    {

        if (weaponType == WeaponType.Machinegun)
        {
            if (Input.GetMouseButton(0) && Time.time - _lastShootTime > attackRate)
            {
                Shoot();
            }
        }
        else if (weaponType == WeaponType.Shotgun)
        {
            if (Input.GetMouseButtonUp(0) && Time.time - _lastShootTime > attackRate)
            {
                for (int i = 0; i < 10; i++)
                {
                    Shoot();
                }
            }
        }
    }
    
}
public enum WeaponType
{
    Machinegun,
    Shotgun,
}