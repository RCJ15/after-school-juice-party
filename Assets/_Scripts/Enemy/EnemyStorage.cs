using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a dictionary of every enemy in the scene.
/// </summary> - Ruben
public class EnemyStorage : MonoBehaviour
{
    public static EnemyStorage Instance;
    private GameObject _boss;

    public int EnemyAmount;
    private Dictionary<GameObject, Enemy> _enemiesStored = new Dictionary<GameObject, Enemy>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _boss = Boss.Instance.gameObject;
    }

    public static void AddEnemy(Enemy enemy)
    {
        Instance.EnemyAmount++;

        Instance._enemiesStored.Add(enemy.gameObject, enemy);
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        Instance.EnemyAmount--;

        Instance._enemiesStored.Remove(enemy);
    }

    public static Enemy Get(GameObject obj)
    {
        return Instance.LocalGet(obj);
    }

    private Enemy LocalGet(GameObject obj)
    {
        if (obj == _boss)
        {
            return null;
        }

        if (!_enemiesStored.TryGetValue(obj, out Enemy enemy))
        {
            // Not even an enemy
            if (!obj.TryGetComponent(out enemy))
            {
                return null;
            }

            _enemiesStored[obj] = enemy;
        }

        return enemy;
    }
}