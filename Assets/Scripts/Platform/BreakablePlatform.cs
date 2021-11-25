using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    private float _onBoardTime = 0f;
    public float maxStandingTime;
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _onBoardTime += Time.deltaTime;
            if (_onBoardTime >= maxStandingTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
