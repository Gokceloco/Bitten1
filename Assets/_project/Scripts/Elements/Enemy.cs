using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float playerDistanceThreshold;
    public float speed;

    public int startHealth;
    private int _currentHealth;

    private Player _player;

    private HealthBar _healthBar;

    public void StartEnemy(Player player)
    {
        _healthBar = GetComponentInChildren<HealthBar>();
        _currentHealth = startHealth;
        _player = player;
        _healthBar.SetHealthBar(1);
    }
    private void Update()
    {
        var distanceToPlayer = (transform.position - _player.transform.position).magnitude;
        if (distanceToPlayer < playerDistanceThreshold)
        {
            MoveToPlayer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            player.GetHit(1);
        }
    }

    private void MoveToPlayer()
    {
        var direction = _player.transform.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;
        transform.LookAt(_player.transform.position);
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        _healthBar.SetHealthBar((float)_currentHealth / startHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        gameObject.SetActive(false);
    }
}
