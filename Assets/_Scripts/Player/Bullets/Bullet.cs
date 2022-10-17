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
    protected LayerMask wallLayer;
    protected LayerMask playerLayer;

    protected Rigidbody2D rb;
    protected GameManager gameManager;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        wallLayer = gameManager.WallLayer;
        playerLayer = gameManager.PlayerLayer;

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

    protected bool CheckLayer(GameObject obj, LayerMask mask) => mask == (mask | (1 << obj.layer));

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Check if gameobject is in the WALL layermask
        if (CheckLayer(col.gameObject, wallLayer))
        {
            OnCollideWithWall(col);

            return;
        }
        // Check if gameobject is in the PLAYER layermask
        else if (CheckLayer(col.gameObject, playerLayer))
        {
            OnCollideWithPlayer(col);

            return;
        }
        // Check if we collided with an enemy
        else if (col.CompareTag("Enemy") && col.TryGetComponent(out Enemy enemy))
        {
            OnCollideWithEnemy(col, enemy);

            return;
        }
    }

    protected virtual void OnCollideWithWall(Collider2D col)
    {
        Die();
    }

    protected virtual void OnCollideWithPlayer(Collider2D col)
    {

    }

    protected virtual void OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }


}
