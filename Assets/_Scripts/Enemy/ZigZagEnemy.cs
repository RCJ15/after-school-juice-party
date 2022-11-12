using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ZigZagEnemy : Enemy
{
    [SerializeField] private float horizontalSpeed;
    private bool _movingRight;

    [SerializeField] private LayerMask wallLayer;

    protected override void Start()
    {
        base.Start();

        _movingRight = Random.Range(0, 2) == 1;
    }

    /*
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.velocity = new Vector2(_movingRight ? horizontalSpeed : -horizontalSpeed, rb.velocity.y);
    }
    */

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Hit wall
        if (wallLayer == (wallLayer | (1 << col.gameObject.layer)))
        {
            _movingRight = !_movingRight;
        }
    }
}
