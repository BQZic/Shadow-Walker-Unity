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
        
        private int _lightBallLevel;
        private Light2D _light2D;

        public Dictionary<int, float> LevelToRadius = new Dictionary<int, float>()
        {
            {0, 0}, {1, 0.1f}, {2, 0.2f}, {3, 0.3f}
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


        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddHP(_lightBallLevel * 10);
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
            
             _lightBallLevel = lightBallLevel;
            _light2D = GetComponent<Light2D>();
            _light2D.pointLightOuterRadius = LevelToRadius[_lightBallLevel];
        }
    }
}

