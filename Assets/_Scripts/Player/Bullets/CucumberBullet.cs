using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The cucumber
/// </summary> - Ruben
public class CucumberBullet : Bullet
{
    [Header("Explosion")]
    [SerializeField] private Explosion explosion;
    [SerializeField] private Vector2 explosionSize;

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        explosion.HitEnemies.Add(enemy.gameObject);

        return base.OnCollideWithEnemy(col, enemy);
    }

    protected override void Die()
    {
        // EXPLODE!!!
        explosion.Explode(damage, explosionSize);

        base.Die();
    }
}
