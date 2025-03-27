using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public ParticleSystem muzzlePS1;
    public ParticleSystem muzzlePS2;
    public Light muzzleLight;

    private void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab,
            player.gameDirector.levelManager.currentLevel.transform);
        newBullet.transform.position = shootPos.position;

        muzzleLight.DOKill();
        muzzleLight.intensity = 0;

        if (weaponType == WeaponType.Machinegun)
        {
            newBullet.transform.LookAt(shootPos.transform.position + shootPos.transform.forward * 10
            + transform.right * UnityEngine.Random.Range(-machinegunJitter, machinegunJitter)
            + Vector3.up * UnityEngine.Random.Range(-machinegunJitter, machinegunJitter));
            muzzleLight.DOIntensity(50, .05f).SetLoops(2, LoopType.Yoyo);
        }
        else if (weaponType == WeaponType.Shotgun)
        {
            newBullet.transform.LookAt(shootPos.transform.position + shootPos.transform.forward * 10
            + transform.right * UnityEngine.Random.Range(-shotgunJitter, shotgunJitter)
            + Vector3.up * UnityEngine.Random.Range(-shotgunJitter, shotgunJitter));
            muzzleLight.DOIntensity(100, .1f).SetLoops(2, LoopType.Yoyo);
        }

        newBullet.StartBullet(this);
        _lastShootTime = Time.time;
        var main = muzzlePS1.main;
        main.startColor = new Color(1, UnityEngine.Random.Range(.3f,.7f), 0, 1);
        muzzlePS1.Play();
        muzzlePS2.Play();
    }

    private void Update()
    {
        if (player.gameDirector.gameState != GameState.GamePlay)
        {
            return;
        }
        if (weaponType == WeaponType.Machinegun)
        {
            if (Input.GetMouseButton(0) 
                && Time.time - _lastShootTime > machinegunAttackRate 
                && !IsPointerOverUIObject())
            {
                Shoot();
                player.gameDirector.audioManager.PlayMachineGunShotSFX();
                player.cameraHolder.ShakeCamera(.2f, .1f);
            }
        }
        else if (weaponType == WeaponType.Shotgun)
        {
            if (Input.GetMouseButtonUp(0) 
                && Time.time - _lastShootTime > shotgunAttackRate
                && !IsPointerOverUIObject())
            {
                for (int i = 0; i < 10; i++)
                {
                    Shoot();
                }
                var targetPos = transform.position - transform.forward * recoilAmount;
                targetPos.y = player.transform.position.y;
                player.transform.DOMove(targetPos, .2f);
                player.gameDirector.audioManager.PlayShotGunSFX();
                player.cameraHolder.ShakeCamera(1f, .5f);                
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

    private bool IsPointerOverUIObject()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void LoadShotGun()
    {
        weaponType = WeaponType.Shotgun;
        machingunMesh.SetActive(false);
        shotgunMesh.SetActive(true);
        player.SetAnimationTrigger("Switch");
    }

    public void LoadMachineGun()
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