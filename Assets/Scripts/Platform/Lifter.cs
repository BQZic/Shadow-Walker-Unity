using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platform;
public class Lifter : MonoBehaviour
{
    // public LineRenderer LineRenderer;

    public GameObject[] movingPlatforms;

    public void MoveRightPlatform() {
        if (movingPlatforms[1].TryGetComponent<WaypointFollower>(out WaypointFollower wp)) {
            wp.autoMove = true;
        }
    }
    
}
