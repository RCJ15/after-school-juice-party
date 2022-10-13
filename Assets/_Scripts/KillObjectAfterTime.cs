using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class KillObjectAfterTime : MonoBehaviour
{
    [SerializeField] private float lifetime = 1;

    private void Start()
    {
        StartCoroutine(Coroutine());
    }

    private IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(gameObject);
    }
}
