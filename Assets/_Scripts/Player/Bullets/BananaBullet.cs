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
    private float _returnLerpValue;
    private bool _return;

    [HideInInspector] public BananaShoot Shoot;

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
            _return = true;
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

        _return = true;
        _returnLerpValue = 1;

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

        _return = true;
        _returnLerpValue = 1;

        deathParticles.Play();
    }

    protected override void Die()
    {
        Destroy(gameObject);

        DetachParticleSystem(deathParticles);

        Shoot.BananaDies(this);
    }
}
