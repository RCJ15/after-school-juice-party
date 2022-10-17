using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Essentially, the power up manager.
/// </summary> - Ruben
public class PlayerShootManager : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private GameObject priority;
#endif

    private PlayerShoot[] _playerShoots;
    private int _playerShootsLength;

    private void Start()
    {
        _playerShoots = GetComponentsInChildren<PlayerShoot>();
        _playerShootsLength = _playerShoots.Length;

#if UNITY_EDITOR
        foreach (PlayerShoot shoot in _playerShoots)
        {
            shoot.gameObject.SetActive(false);
        }

        priority.SetActive(true);
#else
        // Disable all but the first powerups
        for (int i = 1; i < _playerShootsLength; i++)
        {
            _playerShoots[i].gameObject.SetActive(false);
        }
#endif
    }

    private void Update()
    {
        
    }
}
