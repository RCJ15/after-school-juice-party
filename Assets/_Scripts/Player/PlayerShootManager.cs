using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Essentially, the power up manager.
/// </summary>
public class PlayerShootManager : MonoBehaviour // - Ruben
{
    public static PlayerShootManager Instance;

    /*
#if UNITY_EDITOR
    // DEBUG!!!
    [SerializeField] private bool haveAllWeapons;
    [SerializeField] private GameObject prioritizedWeapon;
#endif
    */

    public bool BoostedByHoney => HoneyTimer > 0;
    [HideInInspector] public float HoneyTimer;

    private PlayerShoot[] _playerShoots;
    private int _currentShotIndex;
    private int _playerShootsLength;

    public PlayerShoot CurrentWeapon => _playerShoots[_currentShotIndex];

    private int _selectedWeapon = 0;
    private List<int> _equippedWeapons = new List<int>();
    private int _weaponAmount;

    private WeaponPickupPopup _popup;
    private WeaponHUD _hud;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _playerShoots = GetComponentsInChildren<PlayerShoot>();
        _playerShootsLength = _playerShoots.Length;
        _popup = WeaponPickupPopup.Instance;
        _hud = WeaponHUD.Instance;

        /*
#if UNITY_EDITOR
        int weapon = -1;

        for (int i = 0; i < _playerShootsLength; i++)
        {
            PlayerShoot shoot = _playerShoots[i];

            if (haveAllWeapons)
            {
                AddWeapon(i, false);
            }

            if (weapon <= -1 && prioritizedWeapon != null && shoot.gameObject == prioritizedWeapon)
            {
                weapon = i;
                continue;
            }

            shoot.gameObject.SetActive(false);
        }

        if (!haveAllWeapons && weapon > 0)
        {
            AddWeapon(weapon, false);
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
        */
        // Disable all but the first shots
        for (int i = 1; i < _playerShootsLength; i++)
        {
            _playerShoots[i].gameObject.SetActive(false);
        }

        AddWeapon(0, false);
    }

    private void Update()
    {
        if (HoneyTimer > 0)
        {
            HoneyTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.U) && _weaponAmount < _playerShootsLength)
        {
            AddWeapon(_weaponAmount, true);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            int newWeapon = _equippedWeapons[0];

            newWeapon++;

            if (newWeapon >= _playerShootsLength)
            {
                newWeapon = 0;
            }

            ChangeWeapon(0, newWeapon);
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
            _selectedWeapon = index;

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
            _selectedWeapon = index;

            _hud.DisappearTutorialText();

            ChangeShot(_equippedWeapons[index]);
        }
    }

    public void AddWeapon(int weaponIndex, bool doAnims = true)
    {
        _equippedWeapons.Add(weaponIndex);

        _weaponAmount++;

        WeaponCard card = _hud.AddCard(_playerShoots[weaponIndex], doAnims);
        _playerShoots[weaponIndex].weaponCard = card; // Assign card

        _playerShoots[weaponIndex].timesShot = 0;

        _selectedWeapon = _weaponAmount - 1;
        _hud.SelectedCard = _weaponAmount - 1;

        ChangeShot(weaponIndex);

        if (doAnims)
        {
            // Popup!
            _popup.Popup(_playerShoots[_currentShotIndex]);
        }
    }

    /*
    public void RemoveWeapon(Transform weapond, bool doAnims = true)
    {
        ChangeShot(_weaponAmount - 2); // Change weapond

        int weaponIndex = -1;
        for (int i = 0; i < _playerShootsLength; i++)
        {
            if (_playerShoots[i].transform == weapond) // Search for the weapond in inventory
            {
                weaponIndex = i;
            }
        }
        if (weaponIndex == -1) // Could not find a matching weapond
        {
            return;
        }

        _hud.RemoveCard(weaponIndex, doAnims); // Remove its card

        _equippedWeapons.Remove(weaponIndex); // Remove it from our selection
        _weaponAmount--;

        _hud.SelectedCard = _weaponAmount - 1;
        _selectedWeapon = _weaponAmount - 1;
    }
    */

    public void ChangeWeapon(int index, int newWeapon)
    {
        _equippedWeapons[index] = newWeapon;

        WeaponCard card = _hud.SetCard(index, _playerShoots[newWeapon]);
        _playerShoots[newWeapon].weaponCard = card; // Assign card

        _playerShoots[newWeapon].timesShot = 0;

        _selectedWeapon = index;
        _hud.SelectedCard = index;

        ChangeShot(newWeapon);

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
    }
}