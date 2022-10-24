using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] protected float damage = 1;

    [Space]
    [SerializeField] protected float speed;
    [SerializeField] protected float lifetime;

    [Header("Juice")]
    [SerializeField] protected ParticleSystem deathParticles;

    protected LayerMask wallLayer;
    protected LayerMask playerLayer;

    protected Rigidbody2D rb;
    protected GameManager gameManager;
    protected PlayerMove player;

    protected HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    protected void ClearHitList() => hitEnemies.Clear();

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        wallLayer = gameManager.WallLayer;
        playerLayer = gameManager.PlayerLayer;
        player = PlayerMove.Instance;

        Timer();
    }

    protected virtual void Timer()
    {
        // Do not die by lifetime if lifetime is equal or under 0
        if (lifetime <= 0)
        {
            return;
        }

        Invoke(nameof(Die), lifetime);
    }

    protected virtual void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    protected bool CheckLayer(GameObject obj, LayerMask mask) => mask == (mask | (1 << obj.layer));

    protected virtual void OnTriggerStay2D(Collider2D col)
    {
        CheckCol(col);
    }

    protected virtual void CheckCol(Collider2D col)
    {
        // Check if gameobject is in the WALL layermask
        if (CheckLayer(col.gameObject, wallLayer))
        {
            OnCollideWithWall(col);

            return;
        }
        // Check if gameobject is in the PLAYER layermask
        else if (CheckLayer(col.gameObject, playerLayer))
        {
            OnCollideWithPlayer(col);

            return;
        }
        // Check if we collided with an enemy
        else if (!hitEnemies.Contains(col.gameObject) && col.CompareTag("Enemy") && col.TryGetComponent(out Enemy enemy))
        {
            if (OnCollideWithEnemy(col, enemy))
            {
                hitEnemies.Add(col.gameObject);
            }

            return;
        }
    }

    protected virtual void OnCollideWithWall(Collider2D col)
    {
        Die();
    }

    protected virtual void OnCollideWithPlayer(Collider2D col)
    {

    }

    protected virtual bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        // TEMPORARY
        enemy.GetComponent<DummyEnemy>().Hurt(damage);

        Die();

        return true;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);

        DetachAndPlayParticles(deathParticles);
    }

    protected virtual void DetachAndPlayParticles(ParticleSystem particles)
    {
        particles.Play();

        DetachParticleSystem(particles);
    }

    protected virtual void DetachParticleSystem(ParticleSystem particles)
    {
        particles.transform.SetParent(null);

        if (particles.TryGetComponent<KillObjectAfterTime>(out _))
        {
            return;
        }

        KillObjectAfterTime killScript = particles.gameObject.AddComponent<KillObjectAfterTime>();

        ParticleSystem.MainModule main = particles.main;
        AnimationCurve curve = main.startLifetime.curveMax;
        killScript.Lifetime = main.duration + main.startLifetime.constantMax + (curve != null ? curve.Evaluate(1) : 0);
    }
}
