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
    public HealthBar healthBar;
    public Transform clickIndicator;
    public CameraHolder cameraHolder;
    public Weapon weapon;

    [Header("Properties")]
    public LayerMask groundLayerMask;
    public float startSpeed;  
    public float sensitivity;    
    public int startHealth;
    public float jumpPower;
    public float fallSpeed;
    public AnimationState animationState;
    public float lookOffset;
    public float cameraSmoothTime;

    private bool _isDead;

    private int _currentHealth;

    private Rigidbody _rb;

    private NavMeshAgent _agent;

    private Animator _animator;

    public Grenade grenadePrefab;
    public float throwSpeed;
    public Transform handTransform;
    public float grenadeAngularVelocity;

    private Vector3 _moveDirection;

    private Vector3 _mousePos;

    public Collider deadCollider;

    private bool _canJump;

    private Vector3 _velocity;

    public void RestartPlayer(Vector3 startPos)
    {
        _rb = GetComponent<Rigidbody>();
        gameObject.SetActive(true);
        _agent = GetComponent<NavMeshAgent>();
        _rb.position = startPos;
        transform.position = startPos;
        cameraHolder.transform.position = Vector3.zero;
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
        _animator = GetComponentInChildren<Animator>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _animator.SetTrigger("Idle");
        _animator.SetLayerWeight(1, 1);
        deadCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Saw"))
        {
            GetHit(1);
        }
        if (other.CompareTag("Serum"))
        {
            gameDirector.LevelCompleted();
            _animator.SetTrigger("Idle");
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Gate"))
        {
            var gate = other.GetComponent<Gate>();
            if (gate.levelIncrement != 0)
            {
                PlayerPrefs.SetInt("LastReachedLevel", PlayerPrefs.GetInt("LastReachedLevel") + gate.levelIncrement);
                gameDirector.levelManager.currentLevel.StopAllEnemies();
                gameDirector.RestartLevel(gate.playerPositionOnOtherSide);                
            }
            else
            {
                _rb.position = gate.playerPositionOnOtherSide;
                transform.position = gate.playerPositionOnOtherSide;
            }
            
            //gameDirector.LevelCompleted();
        }
        if (other.CompareTag("MessageTrigger"))
        {
            var msgTrigger = other.GetComponent<MessageTrigger>();
            gameDirector.messageUI.Show(msgTrigger.msg, msgTrigger.duration);
            msgTrigger.gameObject.SetActive(false);
        }
        if (other.CompareTag("YBorder"))
        {
            Die(true);
        }
    }

    private void Update()
    {
        if (gameDirector.gameState != GameState.GamePlay)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }
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

        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
        {
            var lookPos = hit.point;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
            _mousePos = lookPos;
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
                _animator.ResetTrigger("Idle");
                _animator.SetTrigger("Walk");
            }
        }
        if (direction == Vector3.zero)
        {
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
            if (animationState != AnimationState.Idle)
            {
                animationState = AnimationState.Idle;
                _animator.ResetTrigger("Walk");
                _animator.SetTrigger("Idle");
            }
        }
        
        

        healthBar.transform.position = transform.position + Vector3.up * 2.4f;

        SetWalkDirection(direction);

        if (Input.GetMouseButtonDown(1) && gameDirector.grenadeManager.isGrenadeAvailabe)
        {
            gameDirector.grenadeManager.StartGrenadeCoolDown();
            Invoke(nameof(ThrowGrenade), .25f);
            _animator.SetTrigger("Throw");
        }
        _moveDirection = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            transform.DOKill();
        }
    }

    private void FixedUpdate()
    {
        var pos = transform.position;
        var lookOffsetVector = transform.forward * lookOffset;

        cameraHolder.transform.position
            = Vector3.SmoothDamp(cameraHolder.transform.position,
            pos + lookOffsetVector, ref _velocity, cameraSmoothTime);
    }

    private void ThrowGrenade()
    {
        var newGrenade = Instantiate(grenadePrefab);
        newGrenade.transform.position = handTransform.position;
        var grenadeRB = newGrenade.GetComponent<Rigidbody>();
        grenadeRB.linearVelocity = (_mousePos - transform.position + Vector3.up * 2) * throwSpeed;
        grenadeRB.angularVelocity = transform.right * grenadeAngularVelocity;
        newGrenade.StartGrenade(this);        
    }

    private void SetWalkDirection(Vector3 direction)
    {
        _animator.SetFloat("WalkDirection", Vector3.SignedAngle(transform.forward, direction, Vector3.up));
    }

    public void SetAnimationTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
        cameraHolder.ShakeCamera(.9f, .4f);
        gameDirector.fXManager.PlayPlayerGotHitFX();
        if (_currentHealth <= 0)
        {
            Die(false);
        }
    }

    private void Die(bool isFalling)
    {        
        _isDead = true;        
        _animator.SetLayerWeight(1, 0);
        if (!isFalling)
        {
            gameDirector.LevelFailed(false, 2);
            _animator.SetTrigger("Fall");
            _rb.linearVelocity = Vector3.zero;
        }
        else
        {
            gameDirector.LevelFailed(false, 0);
        }
        deadCollider.enabled = true;
    }


    public bool GetIfDead()
    {
        return _isDead;
    }

    public void FreezeRigidBody()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void PlayTimeUpAnimation()
    {
        _animator.SetLayerWeight(1, 0);
        _animator.SetTrigger("FallForTime");
    }
}


public enum AnimationState
{
    Idle,
    Walk,
    Jump,
    Interact,
}