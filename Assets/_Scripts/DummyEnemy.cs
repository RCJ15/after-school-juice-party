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

    public bool IsAlive => health > 0;

    private Collider2D _col;
    private SpriteRenderer _sr;

    private void Start()
    {
        _maxHealth = health;

        if (healthBar != null)
        {
            healthBar.maxValue = _maxHealth;
            healthBar.value = _maxHealth;
        }

        _col = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            health = _maxHealth;
            if (healthBar != null)
            {
                healthBar.value = _maxHealth;
            }

            _col.enabled = true;
            _sr.enabled = true;
        }
    }

    public void Hurt(float damage)
    {
        health -= damage;
        
        if (healthBar != null)
        {
            healthBar.value = health;
        }

        if (health <= 0)
        {
            _col.enabled = false;
            _sr.enabled = false;
        }

        DPSCounter.Add(damage);
    }
}
