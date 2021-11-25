using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Priority_Queue;
using LightObject;

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

    public float maxShootRange = 1.5f;
    public float maxAbsorbRange = 1f;
    
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
        HandleAbsorbing();
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
                MaxShootRange = maxShootRange
            });
            
            print("Pressed left mouse button for " + _totalDownTime + "s");
            
            _player.TakeDamage((float) Time2LightLevel()*10);
            _playerAnimator.SetBool("isShooting", false);
            _isClicking = false;
            _totalDownTime = 0f;
        }
    }

    private bool first, second, third = false;
     private void HandleAbsorbing()
     {
         if (!_isClicking && Input.GetMouseButtonDown(1))
         {
             Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
             // 如果鼠标右键在player范围外，则不执行吸收光量的操作
             if ((transform.position - mousePosition).sqrMagnitude > maxShootRange * maxShootRange)
             {
                 Debug.Log("Not in range");
                 return;
             }
             // 如果player已经满血，则不执行吸收光量的操作
             if (_player.isFullHP())
             {
                 Debug.Log("Player has full hp");
                 return;
             }
             _totalDownTime = 0f;
             _isClicking = true;
         }
         
         // NEED TO CHANGE
         if (_isClicking && Input.GetMouseButton(1))
         {
             _totalDownTime += Time.deltaTime;
             if (_totalDownTime >= 0.5 && _totalDownTime < 1 && !first)
             {
                 HandleAbsorbingHelper(1); first = true;
             } else if (_totalDownTime >= 1 && _totalDownTime < 2 && !second)
             {
                 HandleAbsorbingHelper(1); second = true;
             } else if (_totalDownTime >= 2 && !third)
             {
                 HandleAbsorbingHelper(1); third = true;
             }
         }
         
         if (_isClicking && Input.GetMouseButtonUp(1))
         {
             print(_totalDownTime);
             _totalDownTime = 0f;
             _isClicking = false;
             first = second = third = false;
         }
     }
     
    private void HandleAbsorbingHelper(int amount)
    {
        // Get current mouse position
        LightObject.LightObject res = FindNearestLightObj();
        if (res)
        {
            res.LoseLight(amount);
            _player.RecoverHealth(amount * 10);
        }
    }

    private LightObject.LightObject FindNearestLightObj()
    {
        float distance = Single.MaxValue;
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(mousePosition, maxAbsorbRange);
        LightObject.LightObject res = null;
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<LightObject.LightObject>(out LightObject.LightObject lb))
            {
                float tmp = (lb.transform.position - mousePosition).magnitude;
                if ( tmp < distance)
                {
                    res = lb;
                    distance = tmp;
                }
            }
        }
        return res;
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
