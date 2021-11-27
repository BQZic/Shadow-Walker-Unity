using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform portal;
    public GameObject player;
    public Vector3 offset;

    private IEnumerator Teleportation()
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = portal.transform.position + offset;
    }

    public void StartTeleportation() {
        StartCoroutine(Teleportation());
    }
}
