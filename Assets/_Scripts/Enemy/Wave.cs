using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public float waveLenght;
    public int amountOFEasyEnemies;
    public int amountOFMediumEnemies;
    public int amountOFHardEnemies;
    public Vector3 spawnLocation;
    [Tooltip("Prefabs of enemys")]
    public GameObject easyEnemy;
    public GameObject mediumEnemy;
    public GameObject hardEnemy;

    float timeInbetweenSpawning = 0;
    List<GameObject> enemysToSpawn = new List<GameObject>();
    void SetValues(float waveLenght, int amountOFEasyEnemies, int amountOFMediumEnemies, int amountOFHardEnemies, Vector3 spawnLocation, GameObject easyEnemy, GameObject mediumEnemy, GameObject hardEnemy)
    {
        waveLenght = this.waveLenght;
        amountOFMediumEnemies = this.amountOFMediumEnemies;
        amountOFMediumEnemies = this.amountOFMediumEnemies;
        amountOFHardEnemies = this.amountOFHardEnemies;
        spawnLocation = this.spawnLocation;
        easyEnemy = this.easyEnemy;
        mediumEnemy = this.mediumEnemy;
        hardEnemy = this.hardEnemy;
    }
    /// <summary>
    /// Start a wave with the given
    /// </summary>
 public   void StartWave()
    {
        timeInbetweenSpawning = waveLenght / (amountOFEasyEnemies + amountOFMediumEnemies + amountOFHardEnemies);

        for (int i = 0; i < amountOFEasyEnemies; i++)
        {
            enemysToSpawn.Add(easyEnemy);
        }
        for (int i = 0; i < amountOFMediumEnemies; i++)
        {
            enemysToSpawn.Add(mediumEnemy);
        }
        for (int i = 0; i < amountOFHardEnemies; i++)
        {
            enemysToSpawn.Add(hardEnemy);
        }

        StartCoroutine(Running());
    }
    IEnumerator Running()
    {
        while (enemysToSpawn.Count > 0) // While we have enemys to spawn
        {
            Instantiate(RandomEnemy(), spawnLocation, Quaternion.identity); // Spawn the enemy
            yield return new WaitForSeconds(timeInbetweenSpawning);
        }
            }

    private GameObject RandomEnemy() // Get random enemy
    {
        int chance = UnityEngine.Random.Range(0, enemysToSpawn.Count + 1);
      GameObject  chosenForSpawn = enemysToSpawn[chance];
        enemysToSpawn.Remove(enemysToSpawn[chance]);
        return chosenForSpawn;
    }
}