using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class DPSCounter : MonoBehaviour
{
    public static DPSCounter Instance;

    [SerializeField] private TMPro.TMP_Text dpsText;
    private float _dps;

    private void Awake()
    {
        Instance = this;
    }

    public static void Add(float damage)
    {
        Instance.StartCoroutine(Instance.DpsTimer(damage));
    }

    private static readonly WaitForSeconds _wait = new WaitForSeconds(1);
    private IEnumerator DpsTimer(float damage)
    {
        _dps += damage;

        UpdateText();

        yield return _wait;

        _dps -= damage;

        UpdateText();
    }

    private void UpdateText()
    {
        dpsText.text = "DPS: " + Mathf.Round(_dps * 10) / 10;
    }
}
