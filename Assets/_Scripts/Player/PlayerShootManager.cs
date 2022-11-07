using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Essentially, the power up manager.
/// </summary>
public class PlayerShootManager : MonoBehaviour // - Ruben
{
#if UNITY_EDITOR
    // DEBUG!!!
    [SerializeField] private bool haveAllWeapons;
    [SerializeField] private GameObject prioritizedWeapon;
#endif

    private PlayerShoot[] _playerShoots;
    private int _currentShotIndex;
    private int _playerShootsLength;

    public PlayerShoot CurrentWeapon => _playerShoots[_currentShotIndex];

    private int _selectedWeapon = 0;
    private List<int> _equippedWeapons = new List<int>();
    private int _weaponAmount;

    private WeaponPickupPopup _popup;
    private WeaponHUD _hud;

    private void Start()
    {
        _playerShoots = GetComponentsInChildren<PlayerShoot>();
        _playerShootsLength = _playerShoots.Length;
        _popup = WeaponPickupPopup.Instance;
        _hud = WeaponHUD.Instance;

#if UNITY_EDITOR
        for (int i = 0; i < _playerShootsLength; i++)
        {
            PlayerShoot shoot = _playerShoots[i];

            if (haveAllWeapons)
            {
                AddWeapon(i, false);
            }

            if (prioritizedWeapon != null && shoot.gameObject == prioritizedWeapon)
            {
                _currentShotIndex = i;
                continue;
            }

            shoot.gameObject.SetActive(false);
        }

        if (!haveAllWeapons)
        {
            AddWeapon(_currentShotIndex, false);
        }

        if (prioritizedWeapon == null)
        {
            _playerShoots[0].gameObject.SetActive(true);
        }
#else
        // Disable all but the first shots
        for (int i = 1; i < _playerShootsLength; i++)
        {
            _playerShoots[i].gameObject.SetActive(false);
        }

        AddWeapon(0, false);
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && _weaponAmount < _playerShootsLength)
        {
            AddWeapon(_weaponAmount, true);
        }

        if (_weaponAmount <= 1)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            int index = _selectedWeapon;
            index--;

            if (index < 0)
            {
                index = _weaponAmount - 1;
            }

            _hud.SelectedCard = index;

            _hud.DisappearTutorialText();

            ChangeShot(_equippedWeapons[index]);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            int index = _selectedWeapon;
            index++;

            if (index >= _weaponAmount)
            {
                index = 0;
            }

            _hud.SelectedCard = index;

            _hud.DisappearTutorialText();

            ChangeShot(_equippedWeapons[index]);
        }
    }

    public void AddWeapon(int weaponIndex, bool doAnims = true)
    {
        _equippedWeapons.Add(weaponIndex);

        _weaponAmount++;

        _hud.AddCard(_playerShoots[weaponIndex], doAnims);

        _selectedWeapon = weaponIndex;
        _hud.SelectedCard = weaponIndex;

        ChangeShot(_equippedWeapons[weaponIndex]);

        if (doAnims)
        {
            // Popup!
            _popup.Popup(_playerShoots[_currentShotIndex]);
        }
    }

    public void ChangeWeapon(int index, int newWeapon)
    {
        _equippedWeapons[index] = newWeapon;

        _hud.SetCard(index, _playerShoots[newWeapon]);

        _selectedWeapon = newWeapon;
        _hud.SelectedCard = newWeapon;

        ChangeShot(_equippedWeapons[newWeapon]);

        // Popup!
        _popup.Popup(_playerShoots[_currentShotIndex]);
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

        _selectedWeapon = index;
    }
}
