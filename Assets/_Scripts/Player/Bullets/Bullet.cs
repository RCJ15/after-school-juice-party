using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float lifetime;
    protected LayerMask wall;

    protected Rigidbody2D rb;
    protected GameManager gameManager;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        wall = gameManager.Wall;

        Timer();
    }

    protected virtual void Timer()
    {
        Invoke(nameof(Die), lifetime);
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Check if gameobject is in the WALL layermask
        if (wall == (wall | (1 << col.gameObject.layer)))
        {
            OnCollision(col);
        }
    }

    protected virtual void OnCollision(Collider2D col)
    {
        Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
