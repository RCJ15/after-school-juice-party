using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The cucumber
/// </summary> - Ruben
public class ExplosionBullet : Bullet
{
    [Header("Explosion")]
    [SerializeField] private Explosion explosion;
    [SerializeField] private Vector2 explosionSize;
    public Vector2 ExplosionSize { get => explosionSize; set => explosionSize = value; }

    protected override void Awake()
    {
        base.Awake();
    }

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
