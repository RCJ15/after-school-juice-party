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

    private Vector3 _offset;

    protected override void Start()
    {
        base.Start();

        _perlin = Random.Range(-10000f, 10000f);
    }

    protected override void Update()
    {
        // Jitter
        transform.position -= _offset;

        base.Update();

        _perlin += perlinSpeed * Time.deltaTime;

        _offset = new Vector3(((Mathf.PerlinNoise(_perlin, _perlin) * 2) - 1) * perlinIntensity, 0);
        transform.position += _offset;
    }
}
