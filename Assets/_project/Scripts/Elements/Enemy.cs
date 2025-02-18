using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float playerDistanceThreshold;
    public float speed;
    public float hitDuration;

    public int startHealth;
    private int _currentHealth;

    private GameDirector _gameDirector;
    private Player _player;

    private HealthBar _healthBar;

    private float _lastHitTime;

    public NavMeshAgent agent;

    private bool _isBeingPushed;

    public void StartEnemy(Player player)
    {
        _healthBar = GetComponentInChildren<HealthBar>();
        _currentHealth = startHealth;
        _player = player;
        _gameDirector = _player.gameDirector;
        _healthBar.SetHealthBar(1);
    }
    private void Update()
    {
        var distanceToPlayer = (transform.position - _player.transform.position).magnitude;
        if (distanceToPlayer < playerDistanceThreshold
            && _gameDirector.gameState == GameState.GamePlay
            && Time.time - _lastHitTime > hitDuration
            && !_isBeingPushed)
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
            _lastHitTime = Time.time;
        }
    }

    private void MoveToPlayer()
    {        
        agent.SetDestination(_player.transform.position);
    }

    public void StopEnemy()
    {
        agent.speed = 0;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void GetHit(int damage, Vector3 hitDirection)
    {
        _currentHealth -= damage;
        _healthBar.SetHealthBar((float)_currentHealth / startHealth);

        _isBeingPushed = true;
        transform.DOKill();
        hitDirection.y = 0;
        transform.DOMove(transform.position + hitDirection, .2f).OnComplete(SetIsBeingPushedFalse);
        
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    void SetIsBeingPushedFalse()
    {
        _isBeingPushed = false;
    }
    private void Die()
    {
        gameObject.SetActive(false);
    }
}
