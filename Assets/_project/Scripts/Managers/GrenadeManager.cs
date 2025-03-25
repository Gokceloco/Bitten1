using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    public GrenadeUI grenadeUI;
    public float grenadeCoolDown;
    public bool isGrenadeAvailabe;

    private float _lastGrenadeThrowTime;

    private void Update()
    {
        if (!isGrenadeAvailabe && Time.time - _lastGrenadeThrowTime > grenadeCoolDown)
        {
            EnableGrenade();
        }
    }

    public void EnableGrenade()
    {
        isGrenadeAvailabe = true;
        grenadeUI.ShowEnabledImage();
    }

    public void StartGrenadeCoolDown()
    {
        isGrenadeAvailabe = false;
        _lastGrenadeThrowTime = Time.time;
        grenadeUI.ShowDisabledImage();
    }


}
