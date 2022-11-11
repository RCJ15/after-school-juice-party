using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Deprecated]
/// </summary>
public class KillObjectAfterTime : MonoBehaviour
{
    [SerializeField] private float lifetime = 1;
    [SerializeField] private bool unscaledTime;

    public float Lifetime { get => lifetime; set => lifetime = value; }
    public bool UnscaledTime { get => unscaledTime; set => unscaledTime = value; }

    private void Start()
    {
        if (unscaledTime)
        {
            StartCoroutine(Coroutine());
        }
        else
        {
            Destroy(gameObject, lifetime);
        }
    }

    private IEnumerator Coroutine()
    {
        yield return new WaitForSecondsRealtime(lifetime);

        Destroy(gameObject);
    }
}