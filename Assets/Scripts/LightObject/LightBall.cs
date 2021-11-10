using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace LightObject
{
    public class LightBall : LightObject
    {
        public static void Create(Transform parent, Vector3 localPosition, int level, 
            Vector3 destination)
        {
            Transform lightBallTransform = Instantiate(GameAssets.i.pfLightBall, 
                parent);
            lightBallTransform.localPosition = localPosition;
            
            lightBallTransform.GetComponent<LightBall>().Setup(level, destination);
            
            // after 5 seconds destroy it - for testing purpose
            // Destroy(lightBallTransform.gameObject, 5f);
        }

        // for testing purpose - may need to change
        private readonly Dictionary<int, float> _level2Radius = new 
            Dictionary<int, float>() {
            {0, 0}, {1, 0.35f}, {2, 0.65f}, {3, 1f}
        };

        private bool _attachedOnSth;
        private Vector3 _destination;
        private Light2D _light2D;
        private Transform _transform;
        private CircleCollider2D _collider;
        
        [Header("Light ball properties")]
        [SerializeField] private float speed = 1;

        private void Awake()
        {
            _light2D = GetComponent<Light2D>();
            _transform = GetComponent<Transform>();
            _collider = GetComponent<CircleCollider2D>();
            _attachedOnSth = _transform.parent != null;

            if (_attachedOnSth) _collider.enabled = false;
        }

        private void Update()
        {
            if (!_attachedOnSth)
                _transform.position = Vector3.MoveTowards(_transform.position, 
                    _destination, Time.deltaTime * speed);
            
            if (currentLight <= 0) Destroy(gameObject);
        }

        private void Setup(int level, Vector3 destination)
        {
            _light2D.pointLightOuterRadius = _level2Radius[level];
            _destination = destination;
            _collider.radius = _level2Radius[level];
            
            // TODO: 现在假设光量直接和光球等级挂钩，需要后续确认
            currentLight = level;
        }
        
        public void GainLight(int lightAmount)
        {
            if (currentLight + lightAmount > maxLight) return;
            currentLight += lightAmount;
            UpdateUI();
        }

        public void LoseLight(int lightAmount)
        {
            if (currentLight - lightAmount < minLight) return;
            currentLight -= lightAmount;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _light2D.pointLightOuterRadius = _level2Radius[currentLight];
            _collider.radius = _level2Radius[currentLight];
        }
    }
}

