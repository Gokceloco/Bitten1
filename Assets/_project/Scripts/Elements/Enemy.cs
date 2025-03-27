using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float playerDistanceThreshold;
    public float hitDuration;
    public int damage;

    public int startHealth;
    private int _currentHealth;

    private GameDirector _gameDirector;
    private Player _player;

    private HealthBar _healthBar;

    private float _lastHitTime;

    public NavMeshAgent agent;

    private bool _isBeingPushed;

    private Vector3 _playerAttackOffset;
    
    private Rigidbody _rb;
    private Level _level;
    private Animator _animator;
    private bool _isMoveAnimationActive;
    private bool _isAttacking;
    private bool _isDead;
    private bool _isAbleToMove;

    public Collider aliveCollider;
    public List<Collider> deadColliders;
    public GameObject shadow;
    public ParticleSystem expirePSPrefab;
    public Transform chestBone;

    public EnemyType enemyType;

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
        if (_isDead)
        {
            return;
        }
        if (_isAbleToMove)
        {
            var distanceToPlayer = (transform.position - _player.transform.position).magnitude;
            if (distanceToPlayer < playerDistanceThreshold
                && _gameDirector.gameState == GameState.GamePlay
                && Time.time - _lastHitTime > hitDuration
                && !_isBeingPushed
                && !_isAttacking)
            {
                if (!_isMoveAnimationActive)
                {
                    _isMoveAnimationActive = true;
                    _animator.SetTrigger("Walk");
                    _gameDirector.audioManager.PlayZombieSFX();
                }
                MoveToPlayer();                
            }
            else
            {
                agent.SetDestination(transform.position);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isDead && !_player.GetIfDead() && !_isAttacking)
        {
            transform.DOKill();
            transform.DOLookAt(_player.transform.position, .2f);
            _animator.SetTrigger("Attack");
            Invoke(nameof(TryHitPlayer), .5f * hitDuration);
            _isAttacking = true;
        }
    }

    private void TryHitPlayer()
    {
        if (_player.gameDirector.gameState == GameState.GamePlay)
        {
            var distance = Vector3.Distance(_player.transform.position, transform.position);
            if (distance < 2)
            {
                _player.GetHit(damage);
                _gameDirector.audioManager.PlayZombieHitSFX();
            }
            _isAttacking = false;
            _lastHitTime = Time.time;
        }        
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
        _animator.SetTrigger("Idle");
    }

    public void GetHit(int damage, Vector3 hitDirection, float pushForce)
    {
        var angle = Vector3.Angle(transform.forward, hitDirection);
        var dmgMultiplier = 1;
        if (angle < 90f)
        {
            dmgMultiplier = 3;
            _gameDirector.fXManager.PlayCriticalHitFX(transform.position);
        }
        _currentHealth -= damage * dmgMultiplier;
        _healthBar.SetHealthBar((float)_currentHealth / startHealth);

        
        if (_isAbleToMove)
        {
            transform.DOKill();
        }
        hitDirection.y = 0;
        if (enemyType != EnemyType.Boss)
        {
            _isBeingPushed = true;
            transform.DOMove(transform.position + hitDirection, .2f).OnComplete(SetIsBeingPushedFalse);
        }

        if (_currentHealth <= 0 && !_isDead)
        {
            Die(hitDirection, pushForce);
        }
    }
    void SetIsBeingPushedFalse()
    {
        _isBeingPushed = false;
    }
    private void Die(Vector3 direction, float pushForce)
    {
        GetComponentInParent<Level>().EnemyDied(this);
        _isDead = true;
        agent.enabled = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.AddForce(direction.normalized * pushForce);

        aliveCollider.enabled = false;
        foreach (var c in deadColliders)
        {
            c.enabled = true;
        }
        shadow.SetActive(false);

        transform.LookAt(transform.position - direction);

        PlayFallBackAnimation();

        Destroy(gameObject, 3f);
    }

    private void OnDestroy()
    {
        var newPS = Instantiate(expirePSPrefab);
        newPS.transform.position = chestBone.position;
        newPS.Play();
    }

    private void PlayFallBackAnimation()
    {
        if (UnityEngine.Random.value < .5f)
        {
            _animator.SetTrigger("FallBack");
        }
        else
        {
            _animator.SetTrigger("FallBack2");
        }
    }
}

public enum EnemyType
{
    Normal,
    Fast,
    Boss
}