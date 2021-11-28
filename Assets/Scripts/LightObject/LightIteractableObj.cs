using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIteractableObj : MonoBehaviour
{
    public GameObject obj;

    public GameObject fire;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("LightBall")) {
            if (obj.TryGetComponent<Lifter>(out Lifter lifter)) {
                lifter.MoveRightPlatform();
            }
            fire.SetActive(true);
        }    
    }
}
