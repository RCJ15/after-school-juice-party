using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBullet : Bullet
{
    [Header("Orange shoot")]
    [SerializeField] Vector2 sliceAmount;
    [SerializeField] GameObject slice;
    [SerializeField] float sliceSpeed;

    [SerializeField] protected ParticleSystem hitWallParticles;

    [Header("Ricochet")]
    [SerializeField] protected int maxBounces = 0;
    protected int timesBounced;
    [SerializeField] protected int pierce = 0;
    protected int timesPierce;

    protected override void OnCollideWithWall(Collider2D col)
    {
        hitEnemies.Clear();
    }

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        // TEMPORARY
        enemy.GetComponent<DummyEnemy>().Hurt(damage);

        if (pierce > 0)
        {
            timesPierce++;

            if (timesPierce >= pierce)
            {
                Split();
            }
        }
        else
        {
                Split();
        }

        return true;
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
                Split();
                return;
            }
        }

        Vector2 meanNormal = Vector2.zero;

        foreach (ContactPoint2D contactPoint in col.contacts)
        {
            meanNormal += contactPoint.normal;
        }

        meanNormal /= col.contactCount;

        Vector3 oldNormal = transform.up;
        transform.up = Vector2.Reflect(transform.up, meanNormal);

        hitWallParticles.transform.up = (oldNormal + transform.up) / 2;
        hitWallParticles.Play();
    }
    private void Split()
    {
        float amount = Random.Range(sliceAmount.x,sliceAmount.y);
        float rotation = 360 / amount;
        for (int i = 0; i <amount ; i++)
        {
            Vector3 v3Rotation = new Vector3(0, 0, rotation * i);
            GameObject newSlice = Instantiate(slice, transform.position, Quaternion.EulerAngles(v3Rotation), transform.parent);
            Rigidbody2D rb = newSlice.GetComponent<Rigidbody2D>();

            rb.AddForce(new Vector3( speed,0,0));
        }
        Die();
    }
    protected override void Die()
    {
        base.Die();

        DetachParticleSystem(hitWallParticles);
    }
}
