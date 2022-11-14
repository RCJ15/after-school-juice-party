using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ZigZagEnemy : Enemy
{
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float horizontalSpeed;
    private bool _movingRight;

    protected override void Start()
    {
        base.Start();

        _movingRight = Random.Range(0, 2) == 1;
    }

    protected override void Move()
    {
        // Do not move in the traditional way
        //base.Move();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(_movingRight ? horizontalSpeed : -horizontalSpeed, -moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Hit wall
        if (wallLayer == (wallLayer | (1 << col.gameObject.layer)))
        {
            _movingRight = !_movingRight;
        }
    }
}
