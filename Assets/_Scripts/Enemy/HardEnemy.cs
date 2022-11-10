using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardEnemy : Enemy
{[SerializeField] GameObject bullet;
    [SerializeField] Animator anim;
    [SerializeField] Vector2 timeInbetweenAttack;

    float attackFreq;
    private float attackTimer = 0;
    protected override void Start()
    {
        base.Start();
        attackFreq = Random.Range(timeInbetweenAttack.x, timeInbetweenAttack.y);
    }
    protected override void Update()
    {
        base.Update();

        attackTimer += Time.deltaTime;
        if (attackTimer > attackFreq) // Attack
        {
            Attack();
        }
    }
    void Attack()
    {
        Instantiate(bullet, transform.position - new Vector3(0, -1, 0), transform.rotation);
        anim.Play(0);
        attackFreq = Random.Range(timeInbetweenAttack.x, timeInbetweenAttack.y);
        attackTimer = 0;
    }
}
