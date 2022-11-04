using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet that pierces an infinite or finite amount of enemies.
/// </summary> - Ruben
public class PierceBullet : Bullet
{
    [Header("Pierce")]
    [SerializeField] protected bool infinitePierce;
    [SerializeField] protected int pierce;
    protected int timesPierced;

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        // TEMPORARY
        enemy.GetComponent<DummyEnemy>().Hurt(damage);

        HandlePierce();

        return true;
    }

    protected virtual void HandlePierce()
    {
        if (infinitePierce)
        {
            return;
        }

        if (pierce > 0)
        {
            timesPierced++;

            if (timesPierced >= pierce)
            {
                Die();
            }
        }
        else
        {
            Die();
        }
    }
}
