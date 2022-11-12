using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 
/// </summary>
public class Boss : MonoBehaviour
{
    public static Boss Instance;

    [Header("Stats")]
    [SerializeField] private float[] health;
    private float _currentHealth;
    private float _currentMaxHealth;

    //[SerializeField] private int DEBUGATTACKPRIORITY;
    [SerializeField] [Range(1, 5)] private int stage = 1;
    [SerializeField] private GameObject mainObj;
    public int Stage => stage;

    [Header("States")]
    [SerializeField] private BossState idleState;

    [Space]
    [SerializeField] private BossState[] attackStates;
    private int _attackStatesLength;
    private List<BossState> _attackOrder = new List<BossState>();

    private int _currentAttack;

    [Header("Other")]
    [SerializeField] private float timeDeath;
    [SerializeField] private float timeFadeDeath;
    [SerializeField] private float[] speedLerp;
    private float _currentSpeedLerp;
    [HideInInspector] public float LerpMultiplier = 1;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator hurtAnim;

    public void Die()
    {
        //mainObj.SetActive(false);
    }

    [SerializeField] private BossAnimEvents animEvents;

    [SerializeField] private float[] animSpeed;

    private Slider _healthbar;
    private Animator _healthbarAnim;

    private BossState _currentState;

    private Rigidbody2D _rb;

    private bool _dead;

    public float TargetSpeed { get; set; }
    public float CurrentSpeed { get; set; }
    public bool Dead => _dead;

    private void Awake()
    {
        Instance = this;

        animEvents.Boss = this;

        _healthbar = GameObject.FindWithTag("Boss Healthbar").GetComponent<Slider>();
        _healthbarAnim = _healthbar.GetComponent<Animator>();

        UpdateStage();

        _currentState = idleState;
        idleState.Boss = this;
        idleState.Anim = anim;

        foreach (BossState attackState in attackStates)
        {
            attackState.Boss = this;
            attackState.Anim = anim;

            _attackOrder.Add(attackState);
        }

        _attackStatesLength = attackStates.Length;

        ShuffleAttacks();

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_dead)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Boss Die"))
            {
                anim.SetTrigger("Die");
            }

            CurrentSpeed = 0;
            TargetSpeed = 0;

            idleState.enabled = false;

            foreach (BossState state in attackStates)
            {
                state.enabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, _currentSpeedLerp * LerpMultiplier);

        _rb.velocity = new Vector2(CurrentSpeed, 0);
    }

    public void Attack()
    {
        if (_dead || PlayerMove.Dead)
        {
            return;
        }
        
        idleState.enabled = false;

        _currentState = _attackOrder[_currentAttack++];
        //_currentState = attackStates[DEBUGATTACKPRIORITY];
        _currentState.enabled = true;

        if (_currentAttack >= _attackStatesLength)
        {
            ShuffleAttacks();
            _currentAttack = 0;
        }
    }

    public void EnableBoss(int stage)
    {
        _dead = false;

        anim.Play("Boss Entrance");
        this.stage = stage;
        mainObj.SetActive(true);
        UpdateStage();
    }

    public void StartBoss()
    {
        _healthbarAnim.SetTrigger("Appear");

        MusicPlayer.PlayBossSong();

        SoundManager.PlaySound("Basement Clang");
        SoundManager.PlaySound("Space Zipper");
    }

    public void Idle()
    {
        if (_dead)
        {
            return;
        }

        _currentState.enabled = false;

        _currentState = idleState;
        _currentState.enabled = true;

        anim.SetTrigger("Idle");
    }

    public void Hurt(float damage)
    {
        if (_dead)
        {
            return;
        }

        _currentHealth -= damage;

        hurtAnim.SetTrigger("Hurt");

        _healthbar.value = _currentHealth;

        // Die
        if (_currentHealth <= 0)
        {
            _dead = true;

            Score.AddPoints((int)_currentMaxHealth, transform.position);

            anim.SetTrigger("Die");

            MusicPlayer.StopSong();

            _healthbarAnim.SetTrigger("Disappear");

            SoundManager.PlaySound("Boss Defeat", 1);

            PauseScreen.CanPause = false;

            SetTimeScale(0.1f);

            StartCoroutine(DeathSlowMotion());

            foreach (Bullet bullet in FindObjectsOfType<Bullet>())
            {
                bullet.ExternalDie();
            }

            foreach (Enemy enemy in FindObjectsOfType<Enemy>())
            {
                enemy.Die(false);
            }

            CameraEffects.Flash(timeFadeDeath, true, true);
            CameraEffects.Zoom(60, 70, 0.3f, timeDeath, timeFadeDeath, Vector3.zero, true);

            idleState.enabled = false;
            idleState.Die();

            foreach (BossState state in attackStates)
            {
                state.enabled = false;
                state.Die();
            }

            return;
        }

        SoundManager.PlaySound("Basement Fart");

        _healthbarAnim.SetTrigger("Hurt");
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = 0.02f / Time.timeScale;
    }

    private IEnumerator DeathSlowMotion()
    {
        yield return new WaitForSecondsRealtime(timeDeath);

        float timer = timeFadeDeath;

        while (timer > 0)
        {
            SetTimeScale(Mathf.Lerp(0.1f, 1, 1 - (timer / timeDeath)));

            timer -= Time.unscaledDeltaTime;

            yield return null;
        }

        SetTimeScale(1);

        PauseScreen.CanPause = true;
    }

    public void Animate(string animName)
    {
        if (_dead)
        {
            return;
        }

        anim.SetTrigger(animName);
    }

    private void UpdateStage()
    {
        _currentHealth = health[stage - 1];
        _currentMaxHealth = _currentHealth;

        _currentSpeedLerp = speedLerp[stage - 1];

        _healthbar.maxValue = _currentHealth;
        _healthbar.value = _currentHealth;

        idleState.UpdateStage(stage);

        anim.SetFloat("Speed", animSpeed[stage - 1]);

        foreach (BossState attackState in attackStates)
        {
            attackState.UpdateStage(stage);
        }
    }

    private void ShuffleAttacks()
    {
        int n = _attackStatesLength;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            BossState value = _attackOrder[k];
            _attackOrder[k] = _attackOrder[n];
            _attackOrder[n] = value;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Boss))]
    public class BossEditor : Editor
    {
        private Boss _boss;

        private void OnEnable()
        {
            _boss = (Boss)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            using (new EditorGUI.DisabledScope(!EditorApplication.isPlaying))
            {
                if (GUILayout.Button("Update Stage"))
                {
                    _boss.UpdateStage();
                }
            }
        }
    }
#endif
}
