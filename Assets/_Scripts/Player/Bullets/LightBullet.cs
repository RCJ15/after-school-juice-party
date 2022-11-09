using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBullet : Bullet
{
    [SerializeField] float attackRate = 0.1f;
    [SerializeField] float radius = 4f;
    [SerializeField] LayerMask enemyLayer;

    protected override void Start()
    {
        base.Start();

        InvokeRepeating(nameof(Check), 0, attackRate);
    }
    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        return false;
    }
    private void Check()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<DummyEnemy>().Hurt(damage);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
