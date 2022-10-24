using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public class AnimEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent[] @event;
    private int _index;

    public void Invoke()
    {
        @event[_index]?.Invoke();

        _index++;
    }
}
