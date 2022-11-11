using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet for the cherry flavor
/// </summary> - Ruben
public class CherryBullet : Bullet
{
    [SerializeField] protected float currentGravity = 0;
    [SerializeField] protected float moveDelta = 0.2f;
    [Space]
    [SerializeField] protected float rotationalDelta = 0.1f;
    [SerializeField] protected GameObject cherrySeedBullet;

    [Space]
    [SerializeField] protected Explosion explosion;

    protected override void Start()
    {
        base.Start();

        rb.velocity = transform.up * speed;
    }

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        // EXPLODE!!!
        explosion.HitEnemies.Add(col.gameObject);
        explosion.Explode(damage);

        return base.OnCollideWithEnemy(col, enemy);
    }

    protected override void Die()
    {
        GameObject obj = Instantiate(cherrySeedBullet, transform.position, Quaternion.identity);

        obj.transform.up = rb.velocity.normalized;

        base.Die();
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();

        rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, currentGravity), moveDelta);

        transform.up = Vector2.Lerp(transform.up, rb.velocity, rotationalDelta);
    }
}
