using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform portal;
    public GameObject player;
    public Vector3 offset;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Teleportation());
        }
    }

    private IEnumerator Teleportation()
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = portal.transform.position + offset;
    }
}
