using UnityEngine;
using TMPro;
using System;

/// <summary>
/// 
/// </summary>
public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public static bool Enabled { get => Instance._enabled; set => Instance._enabled = value; }

    private bool _enabled = false;

    [SerializeField] private TMP_Text text;

    private float _timeElapsed;
    private float _oldTimeElapsed;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!_enabled)
        {
            return;
        }

        _timeElapsed += Time.deltaTime;

        if (_timeElapsed == _oldTimeElapsed)
        {
            return;
        }

        _oldTimeElapsed = _timeElapsed;

        text.text = "Time:\n" + TimeSpan.FromSeconds(_timeElapsed).ToString("mm':'ss'.'ff");
    }
}
