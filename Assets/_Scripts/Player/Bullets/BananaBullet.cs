using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
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

        rotate.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

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

            RefreshColliderCheck();
        }
    }

    protected override void FixedUpdate()
    {
        Vector2 regVelocity = transform.up * speed;

        if (_return)
        {
            Vector2 returnVelocity = (player.transform.position - transform.position).normalized * speed;

            rb.velocity = Vector2.Lerp(regVelocity, returnVelocity, _returnLerpValue);
        }
        else
        {
            rb.velocity = regVelocity;
        }
    }

    protected override void OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        // TEMPORARY
        enemy.GetComponent<DummyEnemy>().Hurt(damage);

        _return = true;
        _returnLerpValue = 1;

        RefreshColliderCheck();
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
        _return = true;
        _returnLerpValue = 1;

        RefreshColliderCheck();
    }

    protected override void Die()
    {
        base.Die();

        Shoot.BananaDies(this);
    }
}
