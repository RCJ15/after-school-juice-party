using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player move script
/// </summary> - Ruben
public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance;

    public static bool Dead { get => Instance._dead; set => Instance._dead = value; }
    public bool IsDead => _dead;

    private bool _dead;

    [SerializeField] private int hp = 5;
    private int _hp = 5;
    [SerializeField] private float speed = 5;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    [SerializeField] private float frictionAmount;

    [SerializeField] private LayerMask deathLayer;

    [Header("Sine wave bobbing")]
    [SerializeField] private float sineWaveSpeed = 0.3f;
    [SerializeField] private float sineWaveIntensity = 0.75f;

    [Header("Rotation")]
    [SerializeField] private float rotationIntensity = 2;
    [SerializeField] [Range(0, 1)] private float rotationLerpDelta = 0.3f;
    private float _currentRot;

    private float _sineTime;

    private Rigidbody2D _rb;
    private PlayerShootManager _shootManager;

    private Vector3 _startPos;
    [SerializeField] HighScore highScore;
    [SerializeField] Animator[] hearts;

    [SerializeField] private int hpPointAmount = 200;
    private float _iFrames;

    private void Awake()
    {
        Instance = this;
        _hp = hp;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _shootManager = PlayerShootManager.Instance;

        _startPos = transform.localPosition;

        MusicPlayer.StopSong(0);
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HitPlayer();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetHP();
        }
        */

        if (_iFrames > 0)
        {
            _iFrames -= Time.deltaTime;
        }
    }

    public void HitPlayer()
    {
        if (_iFrames > 0 || _dead)
        {
            return;
        }

        _iFrames = 1.5f;

        hp--;

        if (hp <= 0)
        {
            // End game
            highScore.ShowHighScoreTable(false);

            SoundManager.PlaySound("Boss Defeat", 1);

            WaveManager.Instance.StopAllCoroutines();

            gameObject.SetActive(false);

            _dead = true;

            MusicPlayer.StopSong(2);

            Timer.Enabled = false; // Disable timer
        }
        else
        {
            CameraEffects.Shake(0.5f, 0.5f);
            CameraEffects.Flash(1, new Color(1, 0, 0, 0.5f), false, true);
            CameraEffects.Zoom(65, 0.3f, Vector3.zero);

            SoundManager.PlaySound("Crumble Pot");
        }

        hearts[hp].Play("LoseLife"); // play animation
    }
    public void ResetHP(bool givePoints = true)
    {
        if (givePoints)
        {
            int points = hp * hpPointAmount;

            Score.AddPoints(points, transform.position + new Vector3(0, 3), $"<size=50>HP Bonus!</size>\n{hp} x {hpPointAmount}");;
        }

        for (hp = 0; hp < _hp; hp++)
        {
            var state = hearts[hp].GetCurrentAnimatorStateInfo(0);

            if (state.IsName("Empty") || state.IsName("GetLife"))
            {
                continue;
            }

            hearts[hp].Play("GetLife"); // play animation
        }

        SoundManager.PlaySound("Space Glass Shatter", 1);
        CameraEffects.Flash(1, new Color(0, 1, 0, 0.5f));

        hp = _hp;
    }
    private void FixedUpdate()
    {
        // Stolen from this youtub video - Ruben
        // https://www.youtube.com/watch?v=KbtcEVCM7bw

        float input = _dead ? 0 : Input.GetAxisRaw("Horizontal");

        #region Run
        float targetSpeed = input * speed;

        float speedDif = targetSpeed - _rb.velocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        _rb.AddForce(movement * Vector2.right * (_shootManager.BoostedByHoney ? 1.5f : 1f));

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
        _currentRot = Mathf.Lerp(_currentRot, _rb.velocity.x * -rotationIntensity, rotationLerpDelta);

        transform.rotation = Quaternion.Euler(0, 0, _currentRot);

        // Sine wave bobbing - Ruben
        float yPos = _startPos.y + Mathf.Sin(_sineTime) * sineWaveIntensity;

        _sineTime += Time.deltaTime * sineWaveSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, yPos);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_iFrames > 0)
        {
            return;
        }

        if (deathLayer == (deathLayer | (1 << collision.gameObject.layer)))
        {
            HitPlayer();
        }
    }
}