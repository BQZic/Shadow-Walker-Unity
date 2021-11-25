using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform
{
    public class WaypointFollower : MonoBehaviour
    {
        [SerializeField] private GameObject[] waypoints;
        private int _currentWaypointIndex = 0;

        [SerializeField] private float speed = 1f;

        public bool movable = true;

        private void Update()
        {
            // if (!movable) return;
            
            if (Vector3.Distance(waypoints[_currentWaypointIndex].transform.position, transform.position) < .1f)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= waypoints.Length) _currentWaypointIndex = 0;
            }

            transform.position = Vector2.MoveTowards(transform.position,
                waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * speed);
        }
    }
}

