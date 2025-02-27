using DG.Tweening;
using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Player player;
    public Bullet bulletPrefab;
    public Transform shootPos;

    public float recoilAmount;

    private float _lastShootTime;

    public float machinegunAttackRate;
    public float machinegunJitter;

    public float shotgunAttackRate;
    public float shotgunJitter;

    public WeaponType weaponType;

    public GameObject machingunMesh;
    public GameObject shotgunMesh;

    private void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab,
            player.gameDirector.levelManager.currentLevel.transform);
        newBullet.transform.position = shootPos.position;

        if (weaponType == WeaponType.Machinegun)
        {
            newBullet.transform.LookAt(shootPos.transform.position + shootPos.transform.forward * 10
            + transform.right * UnityEngine.Random.Range(-machinegunJitter, machinegunJitter)
            + Vector3.up * UnityEngine.Random.Range(-machinegunJitter, machinegunJitter));
        }
        else if (weaponType == WeaponType.Shotgun)
        {
            newBullet.transform.LookAt(shootPos.transform.position + shootPos.transform.forward * 10
            + transform.right * UnityEngine.Random.Range(-shotgunJitter, shotgunJitter)
            + Vector3.up * UnityEngine.Random.Range(-shotgunJitter, shotgunJitter));
        }

        newBullet.StartBullet(this);
        _lastShootTime = Time.time;
    }

    private void Update()
    {

        if (weaponType == WeaponType.Machinegun)
        {
            if (Input.GetMouseButton(0) && Time.time - _lastShootTime > machinegunAttackRate)
            {
                Shoot();
            }
        }
        else if (weaponType == WeaponType.Shotgun)
        {
            if (Input.GetMouseButtonUp(0) && Time.time - _lastShootTime > shotgunAttackRate)
            {
                for (int i = 0; i < 10; i++)
                {
                    Shoot();
                }
                var targetPos = transform.position - transform.forward * recoilAmount;
                targetPos.y = player.transform.position.y;
                player.transform.DOMove(targetPos, .2f);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadMachineGun();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadShotGun();
        }
    }

    private void LoadShotGun()
    {
        weaponType = WeaponType.Shotgun;
        machingunMesh.SetActive(false);
        shotgunMesh.SetActive(true);
        player.SetAnimationTrigger("Switch");
    }

    private void LoadMachineGun()
    {
        weaponType = WeaponType.Machinegun;
        shotgunMesh.SetActive(false);
        machingunMesh.SetActive(true);
        player.SetAnimationTrigger("Switch");
    }
}
public enum WeaponType
{
    Machinegun,
    Shotgun,
}