using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player move script
/// </summary> - Ruben
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    [SerializeField] private float frictionAmount;

    [Header("Sine wave bobbing")]
    [SerializeField] private float sineWaveSpeed = 0.3f;
    [SerializeField] private float sineWaveIntensity = 0.75f;

    private float _sineTime;

    private Rigidbody2D _rb;

    private Vector3 _startPos;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _startPos = transform.localPosition;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Stolen from this youtub video - Ruben
        // https://www.youtube.com/watch?v=KbtcEVCM7bw

        float input = Input.GetAxisRaw("Horizontal");

        #region Run
        float targetSpeed = input * speed;

        float speedDif = targetSpeed - _rb.velocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        _rb.AddForce(movement * Vector2.right);

        #endregion

        #region Friction
        if (Mathf.Abs(input) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(frictionAmount));

            amount *= Mathf.Sign(_rb.velocity.x);

            _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion

        // Rotate based on X velocity - Ruben
        transform.rotation = Quaternion.Euler(0, 0, _rb.velocity.x * -2);

        // Sine wave bobbing - Ruben
        float yPos = _startPos.y + Mathf.Sin(_sineTime) * sineWaveIntensity;

        _sineTime += Time.deltaTime * sineWaveSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, yPos);
    }
}
