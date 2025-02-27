using System;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public ParticleSystem wallHitPS;
    public ParticleSystem zombieHitPS;
    public void PlayWallImpactFX(Vector3 pos)
    {
        var newPS = Instantiate(wallHitPS);
        newPS.transform.position = pos;
        newPS.Play();
    }
    public void PlayZombieImpactFX(Vector3 pos, Vector3 dir)
    {
        var newPS = Instantiate(zombieHitPS);
        var velocity = newPS.velocityOverLifetime;
        velocity.x = dir.x * 40;
        velocity.y = dir.y * 40;
        velocity.z = dir.z * 40;
        newPS.transform.position = pos;
        newPS.Play();
    }
}
