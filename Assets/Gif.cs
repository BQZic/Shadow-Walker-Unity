using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gif : MonoBehaviour
{
    public Texture2D[] frames;

    private int framesPerSec = 10;

    private SpriteRenderer renderer;
    
    private void Start() {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnGUI() {
        int index = (int) (Time.time * framesPerSec) % frames.Length;
        renderer.material.mainTexture = frames[index];
    }
}
