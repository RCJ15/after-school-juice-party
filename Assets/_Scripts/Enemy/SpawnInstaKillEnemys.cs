using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInstaKillEnemys : MonoBehaviour
{
    public static List<GameObject> savedEnemys = new List<GameObject>();
    [SerializeField] Vector2 maxEnemySpawn;
    [SerializeField] Vector2 minEnemySpawn;
    [SerializeField] Transform enemyParent;
    int level = 1;

    public void SpawnAllSavedEnemys()
    {
        StartCoroutine(Spwan());
    }
    IEnumerator Spwan()
    {
        foreach (var enemy in savedEnemys)
        {
            Vector3 pos = new Vector3(Random.Range(minEnemySpawn.x, maxEnemySpawn.x + 1), Random.Range(minEnemySpawn.y, maxEnemySpawn.y + 1), 0); // Spwan on random position
            GameObject newEnemy = Instantiate(enemy, pos, Quaternion.identity, enemyParent);

            //    newEnemy.hp = 100 %;

            yield return new WaitForSeconds(Wait());// Wait untill next
        }
            yield return new WaitForSeconds(Wait()*3);// Wait untill next
        StartCoroutine(Spwan());
    }
    float Wait()
    {
        if (level < 3)
        {
            return 5;
        }
        else if (level < 5)
        {
            return 2; 
        }
        else if (level < 10)
        {
            return 1;
        }
        else
        {
            return 0.5f;
        }

    }
}