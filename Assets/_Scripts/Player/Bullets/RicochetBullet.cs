using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class RicochetBullet : Bullet
{
    protected override void OnCollideWithWall(Collider2D col)
    {
        
    }


    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (!CheckLayer(col.gameObject, wallLayer))
        {
            return;
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
