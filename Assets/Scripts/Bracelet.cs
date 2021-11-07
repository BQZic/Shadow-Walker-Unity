using System.Collections;
using System.Collections.Generic;
using LightObject;
using UnityEngine;

public class Bracelet : MonoBehaviour
{
    private Transform _transform;
    private Player _player;

    // LongClick Detection
    private float _totalDownTime;
    private bool _isClicking;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        // 这边默认所有道具都会附属在player gameobject上，可能后续会需要改动
        _player = _transform.parent.GetComponent<Player>();
    }

    private void Update()
    {
        LongClick(0); // left click
        LongClick(1); // right click
    }

    private void LongClick(int clicknum)
    {
        if (Input.GetMouseButtonDown(clicknum))
        {
            _totalDownTime = 0;
            _isClicking = true;
        }
        
        if (_isClicking && Input.GetMouseButton(clicknum))
        {
            _totalDownTime += Time.deltaTime;
        }
        
        if (_isClicking && Input.GetMouseButtonUp(clicknum))
        {
            print(_totalDownTime);
            if (clicknum == 0)
                CreateLightBall(Time2LightLevel());
            else 
                AbsorbLight(Time2LightLevel());
            _totalDownTime = 0f;
            _isClicking = false;
        }
    }

    private int Time2LightLevel()
    {
        if (_totalDownTime >= 0 && _totalDownTime < 0.5)
            return 0;
        else if (_totalDownTime >= 0.5 && _totalDownTime < 1)
            return 1;
        else if (_totalDownTime >= 1 && _totalDownTime < 2)
            return 2;
        else
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

    private void AbsorbLight(int level)
    {
        if (level == 0) return;
        
    }
}
