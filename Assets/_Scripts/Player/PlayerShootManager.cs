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
    private int _currentShotIndex;
    private int _playerShootsLength;

    private void Start()
    {
        _playerShoots = GetComponentsInChildren<PlayerShoot>();
        _playerShootsLength = _playerShoots.Length;

#if UNITY_EDITOR
        for (int i = 0; i < _playerShootsLength; i++)
        {
            PlayerShoot shoot = _playerShoots[i];
            
            if (shoot.gameObject == priority)
            {
                _currentShotIndex = i;
                continue;
            }

            shoot.gameObject.SetActive(false);
        }
#else
        // Disable all but the first shots
        for (int i = 1; i < _playerShootsLength; i++)
        {
            _playerShoots[i].gameObject.SetActive(false);
        }
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            int index = _currentShotIndex;
            index--;

            if (index < 0) index = _playerShootsLength;

            ChangeShot(index);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            int index = _currentShotIndex;
            index++;

            if (index >= _playerShootsLength) index = 0;

            ChangeShot(index);
        }
    }

    private void ChangeShot(int index)
    {
        // Weapon is already equipped so return
        if (_currentShotIndex == index)
        {
            return;
        }

        // Disable old shot
        _playerShoots[_currentShotIndex].gameObject.SetActive(false);

        // Set new shot
        _currentShotIndex = index;

        // Enable new shot
        _playerShoots[_currentShotIndex].gameObject.SetActive(true);
    }
}
