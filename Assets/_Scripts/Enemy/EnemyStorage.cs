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

    public int EnemyAmount;
    private Dictionary<GameObject, Enemy> _enemiesStored = new Dictionary<GameObject, Enemy>();

    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Add an enemy to listof enemies
    /// </summary>
    /// <param name="enemy"></param>
    public static void AddEnemy(Enemy enemy)
    {
        Instance.EnemyAmount++;

        Instance._enemiesStored.Add(enemy.gameObject, enemy);
    }
    /// <summary>
    /// Remove a enemy from the list of enemies
    /// </summary>
    /// <param name="enemy"></param>
    public static void RemoveEnemy(GameObject enemy)
    {
        Instance.EnemyAmount--;

        Instance._enemiesStored.Remove(enemy);
    }

    public static Enemy Get(GameObject obj)
    {
        return Instance.LocalGet(obj);
    }
    /// <summary>
    /// Get enemy component from game object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private Enemy LocalGet(GameObject obj)
    {
        if (Boss.Instance != null && obj == Boss.Instance.gameObject)
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
