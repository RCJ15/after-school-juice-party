using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBullet : GravityBullet
{
    [Header("Orange shoot")]
    [SerializeField] protected Vector2Int sliceAmount;
    [SerializeField] protected float sliceRange = 90;
    [SerializeField] protected GameObject slice;
    [SerializeField] protected Explosion explosion;

    protected Vector3 explosionStartScale;
    protected float startRot;

    protected override void Awake()
    {
        base.Awake();

        explosionStartScale = explosion.transform.localScale;
        startRot = transform.eulerAngles.z;
    }

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        // EXPLODE!!!
        explosion.HitEnemies.Add(enemy.gameObject);

        return base.OnCollideWithEnemy(col, enemy);
    }

    protected override void Die()
    {
        int amount = Random.Range(sliceAmount.x,sliceAmount.y);
        float rotation = sliceRange / ((float)amount - 1);
        float offset = Random.Range(-5f, 5f) - sliceRange / 2;

        for (int i = 0; i < amount; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, startRot + (rotation * (float)i) + offset);
            Instantiate(slice, transform.position, rot, transform.parent);
        }

        // EXPLODE!!!
        explosion.Damage = damage;
        explosion.transform.SetParent(null);
        explosion.gameObject.SetActive(true);
        explosion.transform.localScale = explosionStartScale;

        base.Die();
    }
}
