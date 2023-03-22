using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using CodeMonkey.Utils;
using Unity.Mathematics;

namespace LightObject
{
    public class LightBall : LightObject
    {
        public float moveSpeed = 1.0f;
        
        private Vector3 _shootDir;
        private Vector3 _startPos;
        private Vector3 _endPos;
        private float _maxRange;
        
        private Light2D _light2D;

        private readonly Dictionary<int, float> _level2Radius = new Dictionary<int, float>()
        {
            {0, 0}, {1, 0.2f}, {2, 0.4f}, {3, 0.6f}
        };

        private void Start()
        {
            _light2D = GetComponent<Light2D>();
        }

        private void Update()
        {
            if ((transform.position - _startPos).sqrMagnitude <= _maxRange * _maxRange)
                transform.position += _shootDir * moveSpeed * Time.deltaTime;
        }

        private void LateUpdate()
        {
            if (currentLight <= 0) Destroy(gameObject);
            _light2D.pointLightOuterRadius = _level2Radius[currentLight];
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.RecoverHealth(currentLight * 10);
                Destroy(gameObject);
            }
        }
        
        public void SetUp(Vector3 shootDir, int lightBallLevel, Vector3 startPos, Vector3 endPos, float range)
        {
            _shootDir = shootDir;
            transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(shootDir));
            
            _startPos = startPos;
            _endPos = endPos;
            _maxRange = Math.Min(range, (_startPos - _endPos).magnitude);
            
            currentLight = lightBallLevel;
            _light2D = GetComponent<Light2D>();
            _light2D.pointLightOuterRadius = _level2Radius[currentLight];
        }


    }
}

