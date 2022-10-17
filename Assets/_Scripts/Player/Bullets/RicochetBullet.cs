using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class RicochetBullet : Bullet
{
    [SerializeField] protected int maxBounces = 0;
    protected int timesBounced;

    protected override void OnCollideWithWall(Collider2D col)
    {
        
    }


    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (!CheckLayer(col.gameObject, wallLayer))
        {
            return;
        }

        // Die by bounce
        // Only die by bounce if the max bounces is set to above 0
        if (maxBounces > 0)
        {
            // Increase times bounced
            timesBounced++;

            // Die after the maximum amount of bounces is done
            if (timesBounced >= maxBounces)
            {
                Die();
            }
        }

        Vector2 meanNormal = Vector2.zero;

        foreach (ContactPoint2D contactPoint in col.contacts)
        {
            meanNormal += contactPoint.normal;
        }

        meanNormal /= col.contactCount;

        transform.up = Vector2.Reflect(transform.up, meanNormal);
    }
}
