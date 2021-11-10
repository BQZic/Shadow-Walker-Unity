using System.Collections;
using System.Collections.Generic;
using LightObject;
using Unity.Mathematics;
using UnityEngine;
using Priority_Queue;

public class Bracelet : MonoBehaviour
{
    private Transform _transform;
    private Player _player;
    
    // LongClick Detection
    private float _totalDownTime;
    private bool _isClicking;

    [SerializeField] private float maxRange = 0.4f;

    // private List<LightBall> _priority = new List<LightBall>();
    private SimplePriorityQueue<LightBall> _priority = new SimplePriorityQueue<LightBall>();

    private void Start()
    {
        _transform = GetComponent<Transform>();
        // 这边默认所有道具都会附属在player gameobject上，可能后续会需要改动
        _player = _transform.parent.GetComponent<Player>();
    }

    private void Update()
    {
        LongLeftClick();

        LongRightClick();
    }

    private void LongLeftClick()
    {
        if (!_isClicking && Input.GetMouseButtonDown(0))
        {
            _totalDownTime = 0;
            _isClicking = true;
        }
        
        if (_isClicking && Input.GetMouseButton(0))
        {
            _totalDownTime += Time.deltaTime;
        }
        
        if (_isClicking && Input.GetMouseButtonUp(0))
        {
            print(_totalDownTime);
            CreateLightBall(Time2LightLevel());
            _totalDownTime = 0f;
            _isClicking = false;
        }
    }

    private bool first, second, third = false;
    private void LongRightClick()
    {
        if (!_isClicking && Input.GetMouseButtonDown(1))
        {
            _totalDownTime = 0;
            _isClicking = true;
            UpdateNearbyLightBalls();
        }
        // NEED TO CHANGE
        if (_isClicking && Input.GetMouseButton(1))
        {
            _totalDownTime += Time.deltaTime;
            if (_totalDownTime >= 0.5 && _totalDownTime < 1 && !first)
            {
                AbsorbLight(1); first = true;
            } else if (_totalDownTime >= 1 && _totalDownTime < 2 && !second)
            {
                AbsorbLight(1); second = true;
            } else if (_totalDownTime >= 2 && !third)
            {
                AbsorbLight(1); third = true;
            }
        }
        
        if (_isClicking && Input.GetMouseButtonUp(1))
        {
            print(_totalDownTime);
            _totalDownTime = 0f;
            _isClicking = false;
            _priority.Clear();
            first = second = third = false;
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
    
    // TODO：最好在蓄力的时候能有个蓄力条？这样可以更直观地看到到底续了多久
    private void CreateLightBall(int level)
    {
        // TODO: need to confirm
        if (level == 0) return;
        
        // Make sure player has HP to create a new ball
        if (_player.GetHP() - level * 10 <= 0)
        {
            Debug.Log("Player cannot create new light ball due to HP limit");
            return;
        }
        
        _player.TakeDamage(level * 10);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z += 10; // generated screen-to-world position z-axis is -10
        
        LightBall.Create(null, _transform.parent.position, level, worldPosition);
    }

    private void UpdateNearbyLightBalls()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z += 10;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(worldPosition, maxRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("LightBall"))
            {
                float dist = Vector2.Distance(hitCollider.transform.position, worldPosition);
                _priority.Enqueue(hitCollider.gameObject.GetComponent<LightBall>(), dist);
                // _priority.Add(hitCollider.gameObject.GetComponent<LightBall>());
                // nearbyLightBalls.Add(hitCollider.gameObject.GetComponent<LightBall>());
            }
        }
    }

    private void AbsorbLight(int amount)
    {
        if (_priority.Count == 0)
        {
            Debug.Log("No Lights Nearby");
            return;
        }

        LightBall target = _priority.First;
        if (target.GetCurrentLight() - amount <= 0)
            _priority.Dequeue();
        
        target.LoseLight(amount);
    }
    
    // private void AbsorbLight(int amount)
    // {
    //     if (_priority.Count == 0)
    //     {
    //         print("No lights nearby");
    //         return;
    //     }
    //     Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     worldPosition.z += 10;
    //     
    //     LightBall target = null;
    //     float minDist = 99999f;
    //     foreach (LightBall lb in _priority)
    //     {
    //         float dist = Vector2.Distance(lb.transform.position, worldPosition);
    //         if (dist < minDist)
    //         {
    //             target = lb;
    //             minDist = dist;
    //         }
    //     }
    //     // TODO: ADD PLAYER HP HERE
    //     if (target != null)
    //     {
    //         if (target.GetCurrentLight() - amount <= 0)
    //         {
    //             _priority.Remove(target);
    //         }
    //         target.LoseLight(amount);
    //     }
    // }
}
