using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public partial class Player : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController2d controller;
    public float runSpeed = 40f;

    [Header("Health")] 
    public float maxHP = 100f;
    public float minusHP = 1f;

    [Header("Display")] public Text textHP;
    
    // Health
    private float _currentHP = 100f;
    private bool death = false;
    private bool _inLight = false;
    
    // Movement
    private bool _jump = false;
    private float _horizontalMove = 0f;
    
    // Components
    private Animator _animator;
    private Rigidbody2D _rb2d;
    
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckDeath();
        if (death) return;
        
        Movement();
        CheckInLight();
        textHP.text = _currentHP.ToString("00");
    }

    private void FixedUpdate()
    {
        controller.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
        _jump = false;
    }

    private void Movement()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (!_jump)
        {
            if (_horizontalMove == 0f)
                _animator.SetBool("isRun", false);
            else _animator.SetBool("isRun", true);
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
            _animator.SetTrigger("Jump");
        } else _animator.ResetTrigger("Jump");
        
        _animator.SetFloat("yVelocity", _rb2d.velocity.y);
    }
    
    #region Health

    private void CheckDeath()
    {
        if (_currentHP > 0f) return;
        death = true;
        StartCoroutine(PlayDeath());
        //_animator.SetTrigger("isDead");
    }

    private IEnumerator PlayDeath()
    {
        _animator.SetTrigger("isDead");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void CheckInLight()
    {
        if (!_inLight)
        {
            _currentHP -= minusHP * Time.deltaTime;
        }
    }

    public void TakeDamage(float damage, string from = null)
    {
        if (from != null)
        {
            _animator.SetTrigger("isHit");
        }
        _currentHP -= damage;
    }

    public void RecoverHealth(bool max = false)
    {
        if (max) _currentHP = maxHP;
    }
    public void RecoverHealth(float value = 0)
    {
        _currentHP += value;
        if (_currentHP > maxHP)
            _currentHP = maxHP;
    }

    public bool isFullHP()
    {
        return _currentHP >= maxHP;
    }
    #endregion
}
