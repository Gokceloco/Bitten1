using DG.Tweening;
using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [Header("Elements")]
    public GameDirector gameDirector;
    public Transform cameraHolder;
    public HealthBar healthBar;
    public Transform clickIndicator;

    [Header("Properties")]
    public LayerMask groundLayerMask;
    public float startSpeed;  
    public float sensitivity;    
    public int startHealth;
    public float jumpPower;
    public float fallSpeed;
    public AnimationState animationState;

    private bool _isDead;

    private int _currentHealth;

    private Rigidbody _rb;

    private NavMeshAgent _agent;

    private Animator _animator;

    public void RestartPlayer()
    {
        gameObject.SetActive(true);
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _rb.position = Vector3.zero;
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Saw"))
        {
            GetHit(1);
        }
    }

    private void Update()
    {
        var direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        var speed = startSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = startSpeed * 2;
        }

        _rb.position += direction.normalized * speed * Time.deltaTime;

        var pos = transform.position;
        cameraHolder.position = pos;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
        {
            var lookPos = hit.point;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }

        if (Input.GetKeyDown(KeyCode.Space) 
            && Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, 1f, groundLayerMask))
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, jumpPower, _rb.linearVelocity.z);
        }

        if (_rb.linearVelocity.y < 0)
        {
            _rb.linearVelocity -= Vector3.up * Time.deltaTime * fallSpeed;
        }

        if (direction.magnitude > 0)
        {
            if (animationState != AnimationState.Walk)
            {
                animationState = AnimationState.Walk;
                _animator.SetTrigger("Walk");
            }
        }
        if (direction == Vector3.zero)
        {
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
            if (animationState != AnimationState.Idle)
            {
                animationState = AnimationState.Idle;
                _animator.SetTrigger("Idle");
            }
        }

        healthBar.transform.position = transform.position + Vector3.up * 2.4f;
    }

    public void SetAnimationTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
        if (_currentHealth <= 0)
        {
            gameDirector.gameState = GameState.FailScreen;
            _isDead = true;
            gameDirector.levelManager.currentLevel.StopAllEnemies();
            gameObject.SetActive(false);
        }
    }

    

    public bool GetIfDead()
    {
        return _isDead;
    }
}


public enum AnimationState
{
    Idle,
    Walk,
    Jump,
    Interact,
}