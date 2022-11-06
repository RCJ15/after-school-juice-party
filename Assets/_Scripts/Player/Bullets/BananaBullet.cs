using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The bullet for the banana flavor
/// </summary> - Ruben
public class BananaBullet : Bullet
{
    [SerializeField] protected Transform rotate;
    [SerializeField] protected float rotateSpeed;

    [Space]
    [SerializeField] protected float timeUntilReturn;
    [SerializeField] protected Vector2 colliderReturnSize = new Vector2(1, 1);
    private float _returnLerpValue;
    private bool _return;

    [HideInInspector] public BananaShoot Shoot;

    private BoxCollider2D _col;

    protected override void Awake()
    {
        base.Awake();

        _col = GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (_return)
        {
            if (_returnLerpValue != 1)
            {
                _returnLerpValue += Time.deltaTime;
                _returnLerpValue = Mathf.Clamp01(_returnLerpValue);
            }

            return;
        }

        if (timeUntilReturn > 0)
        {
            timeUntilReturn -= Time.deltaTime;
        }
        else
        {
            Return();
        }
    }

    protected override void FixedUpdate()
    {
        rotate.Rotate(Vector3.forward, rotateSpeed * Time.fixedDeltaTime);

        Vector2 regVelocity = transform.up * speed;

        if (_return)
        {
            Vector2 returnVelocity = (player.transform.position - transform.position).normalized * speed * 1.5f;

            rb.velocity = Vector2.Lerp(regVelocity, returnVelocity, _returnLerpValue);
        }
        else
        {
            rb.velocity = regVelocity;
        }
    }

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        // TEMPORARY
        enemy.GetComponent<DummyEnemy>().Hurt(damage);

        if (!_return)
        {
            Return();

            _returnLerpValue = 1;
        }

        deathParticles.Play();

        return true;
    }

    protected override void OnCollideWithPlayer(Collider2D col)
    {
        if (!_return)
        {
            return;
        }

        Die();
    }

    protected override void OnCollideWithWall(Collider2D col)
    {
        if (_return)
        {
            return;
        }

        Return();

        _returnLerpValue = 1;

        deathParticles.Play();
    }

    private void Return()
    {
        _col.size = colliderReturnSize;
        _return = true;
    }

    protected override void Die()
    {
        Destroy(gameObject);

        DetachParticleSystem(deathParticles);

        Shoot.BananaDies(this);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, colliderReturnSize);
    }
#endif
}
