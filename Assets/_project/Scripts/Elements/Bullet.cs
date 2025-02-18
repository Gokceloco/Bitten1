using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float maxDistance;

    private Weapon _weapon;
    private bool _isMoving;
    private Rigidbody _rb;

    public void StartBullet(Weapon weapon)
    {
        _isMoving = true;
        _weapon = weapon;
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        _rb.position += transform.forward * Time.deltaTime * bulletSpeed;

        var distanceVector = transform.position - _weapon.transform.position;
        var distanceToWeapon = distanceVector.magnitude;
        if (distanceToWeapon > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<Enemy>().GetHit(1, transform.forward);
            Destroy(gameObject);
        }
    }
}
