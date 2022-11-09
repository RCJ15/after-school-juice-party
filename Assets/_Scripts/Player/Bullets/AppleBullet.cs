using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBullet : Bullet
{
    [Space]
    [SerializeField] float angularVelocity;
    [SerializeField] GameObject apple;
    [SerializeField] float delayTimer = 1;
    public float curveAngle;
    List<GameObject> seeds = new List<GameObject>(); // Seeds
[Header("Explosion")]
    [SerializeField] private Explosion explosion;
    [SerializeField] private Vector2 explosionSize;


    protected override void Awake()
    {
        seeds.Add(this.gameObject);
        base.Awake();
    }

    protected override void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }

        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.angularVelocity = angularVelocity * curveAngle;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("AppleSeed") && curveAngle == 0 && delayTimer <= 0) // Collided with another seed and is the middel seed
        {
            seeds.Add(collision.gameObject);
            if (seeds.Count >= 3) // Has colided with two other seeds
            {
                Instantiate(apple, transform.position, transform.rotation);
                explosion.Explode(damage, explosionSize);

                foreach (var seed in seeds)
                {
                    Destroy(seed);
                }
            }
        }
    }
}
