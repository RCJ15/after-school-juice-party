using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The idle state for the boss
/// </summary> - Ruben
public class BossIdleState : BossState
{
    [Header("Left Right Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float range;

    [Space]
    [SerializeField] private float speedMaxDelta = 0.1f;
    private bool _movingRight;

    [Header("Sine wave bobbing")]
    [SerializeField] private float sineWaveSpeed = 0.3f;
    [SerializeField] private float sineWaveIntensity = 0.75f;

    [Space]
    [SerializeField] private float resetYPosLerp = 0.1f;

    [Header("Attacking")]
    [SerializeField] private float timeBeforeAttack;
    [SerializeField] private Vector2 rngTimer = new Vector2(2, 3);
    private float _attackTimer;

    private float _thisTargetSpeed;

    private bool _isAttacking;
    private float _sineTime;
    private Vector3 _startPos;

    protected override void Start()
    {
        base.Start();

        _startPos = transform.localPosition;
    }

    private void OnEnable()
    {
        _sineTime = 0;
        _isAttacking = false;
        _attackTimer = Random.Range(rngTimer.x, rngTimer.y);
    }

    protected override void Update()
    {
        base.Update();

        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }
        else if (!_isAttacking)
        {
            _isAttacking = true;

            StartCoroutine(AttackDelay());
        }

        if (_isAttacking)
        {
            TargetSpeed = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, _startPos.y, resetYPosLerp * Time.deltaTime));

            return;
        }

        _thisTargetSpeed = moveSpeed * (_movingRight ? 1 : -1);
        TargetSpeed = Mathf.MoveTowards(TargetSpeed, _thisTargetSpeed, speedMaxDelta * Time.deltaTime);

        if (_movingRight && transform.position.x >= range)
        {
            _movingRight = false;
        }
        else if (!_movingRight && transform.position.x <= -range)
        {
            _movingRight = true;
        }

        // Sine wave bobbing - Ruben
        float yPos = _startPos.y + Mathf.Sin(_sineTime) * sineWaveIntensity;

        _sineTime += Time.deltaTime * sineWaveSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, yPos);
    }

    private void FixedUpdate()
    {
        
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(timeBeforeAttack);

        Boss.Attack();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 offset = new Vector3(range, 0);
        Vector3 yOffset = new Vector3(0, 1);

        Gizmos.DrawLine(offset + yOffset, offset - yOffset);
        Gizmos.DrawLine(-offset + yOffset, -offset - yOffset);
    }
#endif
}
