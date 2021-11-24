using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed = 10f;
    public float jumpingHeight = 8f;
    public float hp = 100;
    public float minusHp = 0.01f;
    //public float lightBallLevel = 1f;
    private int jumpNum = 0;
    private int Light = 0;
    private float xVelocity;

    public const float maxHp = 100;


    public Text textLife;

    private Animator _animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        xVelocity = Input.GetAxis("Horizontal");
        if (xVelocity >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (xVelocity <= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (Light == 0)
        {
            if (xVelocity != 0)
            {
                hp -= minusHp;
            }
        }
        Movement();
        textLife.text = hp.ToString("00");

        // 新加：
        CheckDeath();
    }

    void Movement()
    {
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        if (jumpNum == 1)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingHeight);
                jumpNum = 0;
            }
        }
        if (hp > 100)
        {
            hp = 100;
        }

        // 新加：TODO - 由于跑步动画过长 需要中途停止？
        _animator.SetBool("isRun", rb.velocity != Vector2.zero);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WalkablePlatform"))
        {
            jumpNum = 1;
        }
        if (other.gameObject.CompareTag("Damage"))
        {
            hp -= 20;
            if (hp < 0)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            Light = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            Light = 0;
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

    public void AddHP(int lightBallLevel)
    {
        hp += lightBallLevel;
    }

    public bool IsFullHP()
    {
        return Math.Abs(hp - maxHp) <= 0.0001f;
    }
}
