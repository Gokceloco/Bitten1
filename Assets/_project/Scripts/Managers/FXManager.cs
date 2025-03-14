using DG.Tweening;
using System;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public ParticleSystem wallHitPS;
    public ParticleSystem zombieHitPS;

    public ParticleSystem grenadeExplosionPS;

    public void PlayWallImpactFX(Vector3 pos, Vector3 dir)
    {
        var newPS = Instantiate(wallHitPS);
        newPS.transform.position = pos;
        newPS.Play();
        newPS.transform.LookAt(pos + dir);
    }
    public void PlayZombieImpactFX(Vector3 pos, Vector3 dir)
    {
        var newPS = Instantiate(zombieHitPS);
        newPS.transform.position = pos;
        newPS.transform.LookAt(newPS.transform.position + dir * 10);
        newPS.Play();
    }

    public void PlayPlayerGotHitFX()
    {
        gameDirector.playerGotHitUI.Show();
    }

    public void PlayGrenadeExplosionFX(Vector3 pos)
    {
        var newPS = Instantiate(grenadeExplosionPS);
        newPS.transform.position = pos;
        var light = newPS.GetComponentInChildren<Light>();
        light.DOIntensity(300, .2f).SetLoops(2, LoopType.Yoyo);
        light.transform.position += Vector3.up * .1f;
        newPS.Play();
    }
}
