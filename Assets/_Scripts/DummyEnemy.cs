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
    [SerializeField] private TMP_Text dpsText;

    private float _dps;
    private int _timesHurt;

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

        if (dpsText != null)
        {
            StartCoroutine(DpsTimer(damage));
        }
    }

    private IEnumerator DpsTimer(float damage)
    {
        _dps += damage;
        _timesHurt++;

        UpdateText();

        yield return new WaitForSeconds(1);

        _dps -= damage;
        _timesHurt--;

        UpdateText();
    }

    private void UpdateText()
    {
        dpsText.text = "DPS: " + Mathf.Round(_dps * 10) / 10;
    }
}
