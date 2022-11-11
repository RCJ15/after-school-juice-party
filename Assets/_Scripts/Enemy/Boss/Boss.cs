using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 
/// </summary>
public class Boss : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float health = 300;
    [SerializeField] private int DEBUGATTACKPRIORITY;
    [SerializeField] [Range(1, 5)] private int stage = 1;
    public int Stage => stage;

    [Header("States")]
    [SerializeField] private BossState idleState;

    [Space]
    [SerializeField] private BossState[] attackStates;

    [Header("Other")]
    [SerializeField] private float speedLerp;
    [HideInInspector] public float LerpMultiplier = 1;
    [SerializeField] private Animator anim;

    [SerializeField] private RuntimeAnimatorController[] animControllers;

    private BossState _currentState;

    private Rigidbody2D _rb;

    public float TargetSpeed { get; set; }
    public float CurrentSpeed { get; set; }
    
    private void Awake()
    {
        UpdateStage();

        _currentState = idleState;
        idleState.Boss = this;
        idleState.Anim = anim;

        foreach (BossState attackState in attackStates)
        {
            attackState.Boss = this;
            attackState.Anim = anim;
        }

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        idleState.enabled = true;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, speedLerp * LerpMultiplier);

        _rb.velocity = new Vector2(CurrentSpeed, 0);
    }

    public void Attack()
    {
        idleState.enabled = false;

        _currentState = attackStates[Random.Range(0, attackStates.Length)];
        //_currentState = attackStates[DEBUGATTACKPRIORITY];
        _currentState.enabled = true;

        Debug.Log(_currentState);
    }

    public void Idle()
    {
        _currentState.enabled = false;

        _currentState = idleState;
        _currentState.enabled = true;

        anim.SetTrigger("Idle");
    }

    private void UpdateStage()
    {
        anim.runtimeAnimatorController = animControllers[stage - 1];

        idleState.UpdateStage(stage);

        foreach (BossState attackState in attackStates)
        {
            attackState.UpdateStage(stage);
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
