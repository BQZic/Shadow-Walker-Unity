using System;
using System.Collections;
using System.Collections.Generic;
using LightObject;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed=10f;
    public float jumpingHeight=8f;
    public float hp = 100;
    private int jumpNum = 0;
    private float xVelocity;


    public Text textLife;

    private Animator _animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        textLife.text = hp.ToString("00");
        
        // 新加：
        CheckDeath();
    }

    void Movement()
    {
        xVelocity = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        if (jumpNum ==1)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingHeight);
                jumpNum = 0;
            }
        }
        
        // 新加：TODO - 由于跑步动画过长 需要中途停止？
        _animator.SetBool("isRun", rb.velocity != Vector2.zero);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpNum = 1;
        }
        if (other.gameObject.CompareTag("Damage"))
        {
            hp -= 20;
            if (hp<0)
            {
                Destroy(gameObject);
            }
        }
    }

    // 新加：
    private void CheckDeath()
    {
        if (hp <= 0) Destroy(gameObject);
    }
    
    public void TakeDamage(int damage)
    {
        hp -= damage;
    }

    public float GetHP()
    {
        return hp;
    }
}
