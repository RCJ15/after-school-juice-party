using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{   
    [SerializeField] GameObject bullet;
    Animator attackAnim;
    [SerializeField] Vector2 timeInbetweenAttack;

    float attackFreq;
    private float attackTimer = 0;
    protected override void Start()
    {
        base.Start();
        attackFreq = Random.Range(timeInbetweenAttack.x, timeInbetweenAttack.y);

        attackAnim = GetComponent<Animator>();
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
        GameObject obj = Instantiate(bullet, transform.position - new Vector3(0, 0.5f, 0), transform.rotation * Quaternion.Euler(1,1,180));
        Enemy enemy = EnemyStorage.Get(obj);

        if (enemy != null)
        {
            enemy.GivePoints = false;
            enemy.SpawnedByBossOrSpawner = true;
        }
        
        attackFreq = Random.Range(timeInbetweenAttack.x, timeInbetweenAttack.y);
        attackTimer = 0;
        attackAnim.SetTrigger("Shoot");
    }
}
