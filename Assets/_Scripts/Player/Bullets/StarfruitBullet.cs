using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class StarfruitBullet : Bullet
{
    [SerializeField] protected ParticleSystem popBurtEffect;

    [Header("Starfruit")]
    [SerializeField] protected float timeUntilBurst;
    [SerializeField] protected int pierce = 3;
    protected int _timesPierced;
    [SerializeField] protected Vector2 smallPierceSize = new Vector2(0.3f, 0.3f);

    [Space]
    [SerializeField] protected float rotateSpeed;
    [SerializeField] protected float targetGravScale = 2.5f;

    [Space]
    [SerializeField] protected ParticleSystem particles;
    [SerializeField] protected TrailRenderer trail;
    [SerializeField] protected Sprite transparentStarfruit;
    [SerializeField] protected SpriteRenderer sprite;
    private Animator _anim;

    [Space]
    [SerializeField] protected GameObject burst;
    protected bool fallingDown;
    protected bool canDamage;

    protected override void Start()
    {
        base.Start();

        _anim = GetComponent<Animator>();

        if (Random.value >= 0.5f)
        {
            rotateSpeed *= -1;
        }

        sprite.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        burst.transform.rotation = sprite.transform.rotation;
    }

    protected override void Update()
    {
        base.Update();

        sprite.transform.Rotate(Vector3.forward, rb.velocity.y * rotateSpeed * Time.deltaTime);
        burst.transform.rotation = sprite.transform.rotation;

        if (fallingDown)
        {
            if (speed > 0)
            {
                speed = 0;//Mathf.MoveTowards(speed, 0, slowdownDelta);
            }

            if (rb.gravityScale != targetGravScale)
            {
                rb.gravityScale = targetGravScale;
            }

            if (trail.enabled == false)
            {
                trail.enabled = true;
            } 

            if (particles.isPlaying)
            {
                particles.Stop();
            }
        }

        if (burst.activeSelf)
        {
            if (gameObject.layer != 0)
            {
                gameObject.layer = 0;
            }

            return;
        }

        if (timeUntilBurst > 0)
        {
            timeUntilBurst -= Time.deltaTime;
        }
        else
        {
            burst.SetActive(true);
        }
    }

    protected override void FixedUpdate()
    {
        if (!fallingDown)
        {
            base.FixedUpdate();
        }
    }

    public void ChangeTransparency()
    {
        _anim.enabled = false;

        sprite.sprite = transparentStarfruit;
        sprite.color = Color.white;

        fallingDown = true;
    }

    public void EnableCollision()
    {
        canDamage = true;

        popBurtEffect.Play();
    }

    protected override void OnCollideWithWall(Collider2D col)
    {
        burst.SetActive(true);
    }

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        if (!canDamage)
        {
            return false;
        }

        // TEMPORARY
        enemy.GetComponent<DummyEnemy>().Hurt(damage);

        _anim.enabled = false;

        _timesPierced++;

        if (_timesPierced == pierce)
        {
            Die();
        }
        else
        {
            transform.localScale = Vector2.Lerp(Vector2.one, smallPierceSize, (float)_timesPierced / ((float)pierce - 1f));

            deathParticles.Play();
        }

        return true;
    }
}
