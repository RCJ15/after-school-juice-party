using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GravityBullet : Bullet
{
    [Header("Gravity")]
    [SerializeField] protected bool rotateTowardsVelocity;
    [SerializeField] protected bool disabled;

    protected override void Start()
    {
        base.Start();

        base.FixedUpdate();
    }

    protected override void FixedUpdate()
    {
        // Do nothing
        // This means that the rigidbody can handle all the gravity stuff by itself

        if (rotateTowardsVelocity)
        {
            transform.up = rb.velocity;
        }

        if (disabled)
        {
            base.FixedUpdate();
        }
    }
}
