using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet that will ricochet.
/// </summary> - Ruben
public class RicochetBullet : Bullet
{
    [SerializeField] protected ParticleSystem hitWallParticles;
    [SerializeField] protected string bounceSoundEffect;

    [Header("Ricochet")]
    [SerializeField] protected int maxBounces = 0;
    protected int timesBounced;
    [SerializeField] protected int pierce = 0;
    protected int timesPierce;
    [SerializeField] protected Explosion explosion;

    private Animator _anim;

    protected override void Start()
    {
        base.Start();

        _anim = GetComponent<Animator>();
    }

    protected override void OnCollideWithWall(Collider2D col)
    {
        hitEnemies.Clear();
    }

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        HurtEnemy(enemy);

        if (pierce > 0)
        {
            timesPierce++;

            if (timesPierce >= pierce)
            {
                Die();
            }
        }
        else
        {
            Die();
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
                Die();
                return;
            }
        }

        Vector2 meanNormal = Vector2.zero;

        foreach (ContactPoint2D contactPoint in col.contacts)
        {
            meanNormal += contactPoint.normal;
        }

        meanNormal /= col.contactCount;

        transform.up = Vector2.Reflect(transform.up, meanNormal);

        hitWallParticles.transform.up = transform.up;
        hitWallParticles.Play();

        if (!string.IsNullOrEmpty(bounceSoundEffect))
        {
            SoundManager.PlaySound(bounceSoundEffect);
        }

        if (_anim != null)
        {
            _anim.SetTrigger("Boing");
        }
    }

    protected override void Die()
    {
        // EXPLODE (if it exists)
        if (explosion != null)
        {
            explosion.Explode(damage);
        }

        base.Die();

        DetachParticleSystem(hitWallParticles);
    }
}
