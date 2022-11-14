using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ZigZagEnemy : Enemy
{
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float fastMoveSpeed;
    private float _currentSpeed;
    [SerializeField] private float horizontalSpeed;
    private bool _movingRight;

    private Vector3 _startPos;

    protected override void Start()
    {
        base.Start();

        _movingRight = Random.Range(0, 2) == 1;

        _currentSpeed = fastMoveSpeed;

        _startPos = transform.position;
    }

    protected override void Move()
    {
        // Do not move in the traditional way
        //base.Move();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(_movingRight ? horizontalSpeed : -horizontalSpeed, -_currentSpeed);

        _currentSpeed = Mathf.Lerp(moveSpeed, fastMoveSpeed, Mathf.InverseLerp(startYPos, _startPos.y, transform.position.y));
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
