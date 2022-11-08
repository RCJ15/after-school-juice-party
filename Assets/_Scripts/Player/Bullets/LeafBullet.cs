using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Falling bullet (falling animation is handled via an Animator component)
/// </summary> - Ruben
public class LeafBullet : Bullet // - Ruben
{
    [SerializeField] private Gradient color = new Gradient();
    [SerializeField] private SpriteRenderer sprite;

    [Header("Leaf Falling")]
    [SerializeField] private float heightLimit = 7.5f;
    [SerializeField] private float deathZone = -10f;

    [Space]
    [SerializeField] private Vector2 rngOffsetMultiplier = new Vector2(-3, 3);
    [SerializeField] private Vector2 rngFallSpeed = new Vector2(2, 4);
    [SerializeField] private Vector2 rngAnimSpeed = new Vector2(0.5f, 1.5f);
    [SerializeField] private Transform leafObj;

    private float _offsetMultiplier;
    private float _fallSpeed;

    [Header("Animation Values")]
    [SerializeField] private float offset;
    [SerializeField] private float speedMultiplier = 1;

    private bool _falling;
    private Animator _anim;

    protected override void Awake()
    {
        base.Awake();

        sprite.color = color.Evaluate(Random.value);

        // Get anim
        _anim = GetComponent<Animator>();

        // Randomize values
        float multiplier = Random.Range(0, 2) == 1 ? -1 : 1;
        _offsetMultiplier = Random.Range(rngOffsetMultiplier.x, rngOffsetMultiplier.y) * multiplier;

        _fallSpeed = Random.Range(rngFallSpeed.x, rngFallSpeed.y);

        _anim.SetFloat("Speed", Random.Range(rngAnimSpeed.x, rngAnimSpeed.y)); // This includes animation speed
    }

    protected override void Update()
    {
        base.Update();

        if (_falling)
        {
            // Do nothing unless this leaf is below the death zone, then the leaf will die
            if (transform.position.y >= deathZone)
            {
                return;
            }

            // Die without any effects as the leaf is offscreen
            Destroy(gameObject);
            return;
        }

        // Do nothing unless this leaf is above the height limit, then we will start falling
        if (transform.position.y <= heightLimit)
        {
            return;
        }

        // Ensure we are facing DOWN
        transform.up = Vector3.down;

        // Enable animator (it's assumed to be disabled by default)
        _anim.enabled = true;

        _falling = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_falling)
        {
            return;
        }

        // Set falling speed
        speed = _fallSpeed * speedMultiplier;

        // Set offset
        leafObj.transform.localPosition = new Vector3(offset * _offsetMultiplier, 0);
    }

    // Ensure the bullet doesn't die on collision with a wall
    protected override void OnCollideWithWall(Collider2D col)
    {
        //base.OnCollideWithWall(col);
    }

#if UNITY_EDITOR
    // Ensure we only draw gizmos in the inspector
    private void OnDrawGizmosSelected()
    {
        // Draw the height limit and death zone
        Gizmos.color = Color.yellow;

        Vector3 offset = Vector3.right * 3;

        // Height limit
        Vector3 point = transform.position + new Vector3(0, heightLimit);

        Gizmos.DrawLine(point - offset, point + offset);

        // Death zone
        point = transform.position + new Vector3(0, deathZone);

        Gizmos.DrawLine(point - offset, point + offset);
    }
#endif
}
