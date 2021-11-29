using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Platform {
    public class InteractableObj : MonoBehaviour
    {
        private bool _hasCollided;
        private string _labelText = "";
        
        public bool autoInteract = false;

        [Tooltip("Press E to interact with ... ?")]
        public GameObject interactableObj;

        [Tooltip("1 is on, 0 is off")]
        public Sprite[] onoff;

        private void Update() {
            if (autoInteract && _hasCollided) {
                if (interactableObj != null && 
                    interactableObj.TryGetComponent<WaypointFollower>(out WaypointFollower wp)) 
            {
                wp.frozen = false;
                if (TryGetComponent<Light2D>(out Light2D light))  
                {
                    light.enabled = true;
                }

                if (transform.Find("Sprite").TryGetComponent<SpriteRenderer>(out SpriteRenderer sr)) 
                {
                    sr.sprite = onoff[1];
                }
            }
            }
            if (!autoInteract && _hasCollided && Input.GetButtonDown("Interact")) {
                InteractWith();
            }
        }

        private void InteractWith() {
            if (TryGetComponent<Teleport>(out Teleport t)) 
            {
                t.StartTeleportation();
            } 
            else if (interactableObj != null && 
                    interactableObj.TryGetComponent<WaypointFollower>(out WaypointFollower wp)) 
            {
                wp.frozen = false;
                if (TryGetComponent<Light2D>(out Light2D light))  
                {
                    light.enabled = true;
                }

                if (transform.Find("Sprite").TryGetComponent<SpriteRenderer>(out SpriteRenderer sr)) 
                {
                    sr.sprite = onoff[1];
                }
            }
        }

        private void OnGUI() {
            if (_hasCollided && !autoInteract) {
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
