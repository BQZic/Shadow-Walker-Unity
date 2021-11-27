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

        public bool autoMove = true;

        public bool oneWayMove = false;

        public bool frozen = false;

        private void Update()
        {
            if (frozen) return;
            
            if (oneWayMove) {
                transform.position = Vector2.MoveTowards(transform.position,
                    waypoints[1].transform.position, Time.deltaTime * speed);
            }

            if (autoMove && !oneWayMove) {
                if ((waypoints[_currentWaypointIndex].transform.position - transform.position).sqrMagnitude < .1f * .1f) 
                {
                    _currentWaypointIndex++;
                    if (_currentWaypointIndex >= waypoints.Length) _currentWaypointIndex = 0;
                }

                transform.position = Vector2.MoveTowards(transform.position,
                    waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * speed);
            }
            
            
        }

    }
}

