using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base bullet script.
/// </summary> - Ruben
public class Bullet : MonoBehaviour
{
    [SerializeField] protected float damage = 1;
    public float Damage => damage;

    [Space]
    [SerializeField] protected float speed;
    public float Speed { get => speed; set => speed = value; }

    [SerializeField] protected float lifetime;
    [SerializeField] protected bool hitPlayer;

    [Header("Juice")]
    [SerializeField] protected string spawnSoundEffect;
    [SerializeField] protected string deathSoundEffect;

    [Space]
    [SerializeField] protected ParticleSystem deathParticles;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] protected ParticleSystem constantParticles;

    protected LayerMask wallLayer;
    protected LayerMask playerLayer;

    protected Rigidbody2D rb;
    protected GameManager gameManager;
    protected PlayerMove player;
    protected PlayerShootManager shootManager;

    protected virtual bool CanBeBoostedByHoney => true;

    protected HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    public HashSet<GameObject> HitEnemies { get => hitEnemies; set => hitEnemies = value; }

    protected void ClearHitList() => hitEnemies.Clear();

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        wallLayer = gameManager.WallLayer;
        playerLayer = gameManager.PlayerLayer;
        player = PlayerMove.Instance;
        shootManager = PlayerShootManager.Instance;

        if (!string.IsNullOrEmpty(spawnSoundEffect))
        {
            SoundManager.PlaySound(spawnSoundEffect);
        }

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
        if (speed > 0)
        {
            rb.velocity = transform.up * speed * (shootManager.BoostedByHoney && CanBeBoostedByHoney && !hitPlayer ? 1.5f : 1f);
        }
    }

    protected bool CheckLayer(GameObject obj, LayerMask mask)
    {
        return mask == (mask | (1 << obj.layer));
    }

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
        else if (!hitEnemies.Contains(col.gameObject) && col.CompareTag("Enemy") )
        {
            col.TryGetComponent(out Enemy enemy);
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
        if (!hitPlayer)
        {
            return;
        }

        Debug.Log("Player hit!");

        Die();
    }

    protected virtual bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        if (hitPlayer)
        {
            return false;
        }

        // TEMPORARY
        try
        {
            col.GetComponent<Enemy>().Hurt(damage);
            enemy.GetComponent<DummyEnemy>().Hurt(damage);
        }
        catch (Exception)
        {

        }

        Die();

        return true;
    }

    protected virtual void Die()
    {
        if (!string.IsNullOrEmpty(deathSoundEffect))
        {
            SoundManager.PlaySound(deathSoundEffect);
        }

        Destroy(gameObject);

        if (deathParticles != null)
        {
            DetachAndPlayParticles(deathParticles);
        }

        if (constantParticles != null)
        {
            DetachParticleSystem(constantParticles);
            constantParticles.Stop();
        }

        if (trail != null)
        {
            DetachTrail(trail);
        }
    }

    protected virtual KillObjectAfterTime DetachAndPlayParticles(ParticleSystem particles)
    {
        particles.Play();

        return DetachParticleSystem(particles);
    }

    protected virtual KillObjectAfterTime DetachParticleSystem(ParticleSystem particles)
    {
        particles.transform.SetParent(null);

        if (particles.TryGetComponent(out KillObjectAfterTime killScript))
        {
            return killScript; 
        }

        killScript = particles.gameObject.AddComponent<KillObjectAfterTime>();

        ParticleSystem.MainModule main = particles.main;
        AnimationCurve curve = main.startLifetime.curveMax;
        killScript.Lifetime = main.duration + main.startLifetime.constantMax + (curve != null ? curve.Evaluate(1) : 0);

        return killScript;
    }

    protected virtual KillObjectAfterTime DetachTrail(TrailRenderer trail)
    {
        trail.transform.SetParent(null);

        if (trail.TryGetComponent(out KillObjectAfterTime killScript))
        {
            return killScript;
        }

        killScript = trail.gameObject.AddComponent<KillObjectAfterTime>();

        killScript.Lifetime = trail.time;

        return killScript;
    }
}
