using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float playerDistanceThreshold;
    public float hitDuration;

    public int startHealth;
    private int _currentHealth;

    private GameDirector _gameDirector;
    private Player _player;

    private HealthBar _healthBar;

    private float _lastHitTime;

    public NavMeshAgent agent;

    private bool _isBeingPushed;

    private Vector3 _playerAttackOffset;

    private bool _isMoving;

    public void StartEnemy(Player player)
    {
        agent.SetDestination(transform.position);
        _healthBar = GetComponentInChildren<HealthBar>();
        _currentHealth = startHealth;
        _player = player;
        _gameDirector = _player.gameDirector;
        _healthBar.SetHealthBar(1);
        _playerAttackOffset = new Vector3(UnityEngine.Random.Range(-1f,1f),
            0, UnityEngine.Random.Range(-1f, 1f)) * UnityEngine.Random.Range(1f,3f);
    }

    public void StartMoving()
    {
        _isMoving = true;
    }

    private void Update()
    {
        if (_isMoving)
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
        var distanceToPlayer = (transform.position - _player.transform.position).magnitude;
        if (distanceToPlayer < 4.5f)
        {
            agent.SetDestination(_player.transform.position);
        }
        else
        {
            agent.SetDestination(_player.transform.position + _playerAttackOffset);
        }
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
        if (_isMoving)
        {
            transform.DOKill();
        }
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
        GetComponentInParent<Level>().EnemyDied(this);
        gameObject.SetActive(false);
    }
}
