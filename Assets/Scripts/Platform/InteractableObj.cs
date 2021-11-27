using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform {
    public class InteractableObj : MonoBehaviour
    {
        private bool _hasCollided;
        private string _labelText = "";

        [Tooltip("Press E to interact with ... ?")]
        public GameObject interactableObj;

        private void Update() {
            if (_hasCollided && Input.GetButtonDown("Interact")) {
                if (TryGetComponent<Teleport>(out Teleport t)) {
                    t.StartTeleportation();
                } else if (interactableObj != null && 
                        interactableObj.TryGetComponent<WaypointFollower>(out WaypointFollower wp)) {
                    wp.frozen = false;
                }
            }
        }

        private void OnGUI() {
            if (_hasCollided) {
                GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),(_labelText));
            }    
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                _hasCollided = true;
                _labelText = "Hit E to interact";    
                
            }    
        }

        private void OnTriggerExit2D(Collider2D other) {
            _hasCollided = false;
        }


    }

}
