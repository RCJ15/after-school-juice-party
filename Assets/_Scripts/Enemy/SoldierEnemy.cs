using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SoldierEnemy : ShooterEnemy
{
    [SerializeField] private float perlinIntensity;
    [SerializeField] private float perlinSpeed;
    private float _perlin;

    protected override void Start()
    {
        base.Start();

        _perlin = Random.Range(-10000f, 10000f);
    }

    protected override void Update()
    {
        base.Update();

        _perlin += perlinSpeed * Time.deltaTime;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        rb.velocity = new Vector2(((Mathf.PerlinNoise(_perlin, _perlin) * 2) - 1) * perlinIntensity, rb.velocity.y);
    }
}
