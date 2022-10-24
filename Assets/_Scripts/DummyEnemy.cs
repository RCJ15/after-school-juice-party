using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TEMPORARY!!!!
/// </summary>
public class DummyEnemy : MonoBehaviour
{
    //-- TEMPORARY
    [SerializeField] private float health = 100;
    private float _maxHealth;

    [SerializeField] private Slider healthBar;

    private void Start()
    {
        _maxHealth = health;

        if (healthBar != null)
        {
            healthBar.maxValue = _maxHealth;
            healthBar.value = _maxHealth;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            health = _maxHealth;
            if (healthBar != null)
            {
                healthBar.value = _maxHealth;
            }
        }
    }

    public void Hurt(float damage)
    {
        health -= damage;
        
        if (healthBar != null)
        {
            healthBar.value = health;
        }

        DPSCounter.Add(damage);
    }
}
