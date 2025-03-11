using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource machineGunShotAS;
    public AudioSource shotGunAS;
    public AudioSource zombieAS;
    public AudioSource zombieHitAS;
    public AudioSource ambiance;
    public AudioSource ambiance2;
    public void PlayMachineGunShotSFX()
    {
        machineGunShotAS.pitch = Random.Range(.8f, 1.2f);
        machineGunShotAS.Play();
    }
    public void PlayShotGunSFX()
    {
        shotGunAS.pitch = Random.Range(.8f, 1.2f);
        shotGunAS.Play();
    }
    public void PlayZombieSFX()
    {
        zombieAS.pitch = Random.Range(.8f, 1.2f);
        zombieAS.Play();
    }
    public void PlayZombieHitSFX()
    {
        zombieHitAS.pitch = Random.Range(.8f, 1.2f);
        zombieHitAS.Play();
    }
    public void PlayAmbiantSound()
    {
        ambiance.Play();
    }
    public void PlayAmbiantSound2()
    {
        ambiance2.Play();
    }
    public void StopAmbientSound()
    {
        ambiance.Stop();
        ambiance2.Stop();
    }
}
