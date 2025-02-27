using DG.Tweening;
using System;
using System.Collections;
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

    private bool _isAbleToMove;

    private Rigidbody _rb;

    private Level _level;

    private Animator _animator;

    private bool _isMoveAnimationActive;

    private bool _isAttacking;

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
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void StartMoving()
    {
        _isAbleToMove = true;
    }

    private void Update()
    {
        if (_isAbleToMove)
        {
            var distanceToPlayer = (transform.position - _player.transform.position).magnitude;
            if (distanceToPlayer < playerDistanceThreshold
                && _gameDirector.gameState == GameState.GamePlay
                && Time.time - _lastHitTime > hitDuration
                && !_isBeingPushed
                && !_isAttacking)
            {
                MoveToPlayer();
                if (!_isMoveAnimationActive)
                {
                    _isMoveAnimationActive = true;
                    _animator.SetTrigger("Walk");
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _animator.SetTrigger("Attack");
            Invoke(nameof(TryHitPlayer), .5f);
            _isAttacking = true;
        }
    }

    private void TryHitPlayer()
    {
        var distance = Vector3.Distance(_player.transform.position, transform.position);
        if (distance < 2)
        {
            _player.GetHit(1);
        }
        _isAttacking = false;
        _lastHitTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            agent.enabled = false;
            _isAbleToMove = false;
            _rb.constraints = RigidbodyConstraints.None;
        }
        if (other.CompareTag("YBorder"))
        {
            Destroy(gameObject);
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
        if (_isAbleToMove)
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
