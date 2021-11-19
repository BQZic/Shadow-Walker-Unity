using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerAim : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 StartPosition;
        public Vector3 ShootPosition;
        public int LightBallLevel;
        public float MaxShootRange;
    }

    public float maxRange = 1.5f;
    
    private Transform _aimTransform;
    private Transform _fireStartPosition;
    private Animator _playerAnimator;
    private SpriteRenderer _aimSR;
    private Player _player;
    
    private bool _isClicking = false;
    private float _totalDownTime;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        _aimTransform = transform.Find("Aim");
        _playerAnimator = _aimTransform.parent.GetComponent<Animator>();
        _aimSR = _aimTransform.Find("Sprite").GetComponent<SpriteRenderer>();
        _aimSR.enabled = false;
        _fireStartPosition = _aimTransform.Find("StartPosition");
    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        if (_isClicking)
            _aimSR.enabled = true;
        else _aimSR.enabled = false;
        
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _aimTransform.eulerAngles = new Vector3(0, 0, angle);
        _aimTransform.localScale = transform.localScale;
    }

    private void HandleShooting()
    {
        if (!_isClicking && Input.GetMouseButtonDown(0))
        {
            _totalDownTime = 0f;
            _playerAnimator.SetBool("isShooting", true);
            _isClicking = true;
        }

        if (_isClicking && Input.GetMouseButton(0))
        {
            _totalDownTime += Time.deltaTime;
        }

        if (_isClicking && Input.GetMouseButtonUp(0))
        {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                StartPosition = _fireStartPosition.position,
                ShootPosition = mousePosition,
                LightBallLevel = Time2LightLevel(),
                MaxShootRange = maxRange
            });
            
            print("Pressed left mouse button for " + _totalDownTime + "s");
            
            _player.TakeDamage(Time2LightLevel()*10);
            _playerAnimator.SetBool("isShooting", false);
            _isClicking = false;
            _totalDownTime = 0f;
        }
    }

    private int Time2LightLevel()
    {
        if (_totalDownTime >= 0 && _totalDownTime < 0.5)
             return 0;
        if (_totalDownTime >= 0.5 && _totalDownTime < 1)
             return 1;
        if (_totalDownTime >= 1 && _totalDownTime < 2)
             return 2;
        return 3;
    }
}
