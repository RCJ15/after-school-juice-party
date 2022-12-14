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
    private bool _hasExplosion;

    protected Vector3 explosionStartScale;
    protected float startRot;

    [Space]
    [SerializeField] protected float pitchIncrease = 0.1f;
    [SerializeField] protected float startPitch = 1f;
    private float _currentPitch;

    protected override void Awake()
    {
        base.Awake();

        _currentPitch = startPitch;

        _hasExplosion = explosion != null;

        if (_hasExplosion)
        {
            explosionStartScale = explosion.transform.localScale;
        }
        startRot = transform.eulerAngles.z;
    }

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        if (_hasExplosion)
        {
            // EXPLODE!!!
            explosion.HitEnemies.Add(col.gameObject);
        }

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

        if (_hasExplosion)
        {
            // EXPLODE!!!
            explosion.Explode(damage, explosionStartScale);
        }

        base.Die();
    }

    public void PomegranateBeepSfx()
    {
        SoundManager.PlaySound("Cling", _currentPitch);

        _currentPitch += pitchIncrease;
    }
}
