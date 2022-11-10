using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour  //Emma. Den här koden spawn:ar fienderna i "Vågor"/"Formationer" ifall vi vill ha en
                                         //konstant ström av fiender måste vi fixa felspawn koden eftersom det är det den har.
{
    [Tooltip("Hur mycket från den första fienden på raden som den andra fienden ska placeras (blir noll mellan varje rad)")]
    [SerializeField] float EnemyPlaceOffset = 0;

    //Nummer på formation (Just nu finns det bara 2).
    [SerializeField] float timeInbetweenWaves;
    private int _wave = 1;
    private int _previousWave = 0;
    private bool _waveEnded = true;

    [SerializeField] GameObject enemy;
    [Space]
    [Header("Formation One")]
    [SerializeField] Vector3 spawnLocation1;
    [SerializeField] float timeBetweenEnemySpawns1;
    [SerializeField] float distanceBetweenRows1;
    [SerializeField] int totalAmountOfEnemys1;
    [SerializeField] int enemysOnFirstRow1;
    [Space]
    [Header("Formation Two")]
    [SerializeField] Vector3 spawnLocation2;
    [SerializeField] float timeBetweenEnemySpawns2;
    [SerializeField] float distanceBetweenRows2;
    [SerializeField] int totalAmountOfEnemys2;
    [SerializeField] int enemysOnFirstRow2;
    [SerializeField] int enemysOnSecondRow2;
    [SerializeField] int enemysOnThirdRow2;
    [SerializeField] int enemysOnFourthRow2;
    [Space]
    [Header("Formatios")] // Really mesy waves
                          // Array of paramaters ""GameObject[] enemies, float[] spawnWait, Vector3 spawnLocation, Vector3[] offset" maybe?
                          // Or animation that calls an event, whilst animating paramaters?
    [SerializeField] GameObject[][] enemysArr;
    [SerializeField] float[][] spawnWaits;
    [SerializeField] Vector3[] spawnPoints;
    [SerializeField] Vector3[][] offsets;


    private void Start()
    {
        ChangeWave();
    }
    //Antagligen den lättaste (att besegra)
    public IEnumerator FormationOne()
    {
        for (int i = 0; i < totalAmountOfEnemys1; i++)
        {
            //Första "raden". Två fiender.
            if (i < enemysOnFirstRow1)
            {
                Instantiate(enemy, spawnLocation1 + new Vector3(EnemyPlaceOffset, 0, 0), Quaternion.identity);

                EnemyPlaceOffset += 1;

                //enemyPlace blir noll inför placeringen av rad 2
                if (i == 1)
                {
                    EnemyPlaceOffset = 0;
                }
            }
            //Andra "raden". Tre fiender.
            else if (i >= enemysOnFirstRow1)
            {
                Instantiate(enemy, spawnLocation1 + new Vector3(EnemyPlaceOffset, distanceBetweenRows1, 0), Quaternion.identity);

                EnemyPlaceOffset++;

                //Gör båda till 0 på slutet ifall vi vill göra så att en ny våg kommer efter (Vilket vi antagligen vill?)
                //(Just nu kommer det ingen ny våg)
                if (i == totalAmountOfEnemys1 - 1)
                {
                    EnemyPlaceOffset = 0;
                }
            }
            yield return new WaitForSeconds(timeBetweenEnemySpawns1); //Vänta inför nästa fiende
        }
        StartCoroutine(WaveEnded());
    }

    public IEnumerator FormationTwo()
    {
        for (int i = 0; i < totalAmountOfEnemys2; i++)
        {
            yield return new WaitForSeconds(timeBetweenEnemySpawns2);
            //Första "raden". En fiende.
            if (i <= enemysOnFirstRow2)
            {
                Instantiate(enemy, spawnLocation2 + new Vector3(EnemyPlaceOffset, 0, 0), Quaternion.identity);
                EnemyPlaceOffset++;

                if (i == enemysOnFirstRow2 - 1)
                {
                    EnemyPlaceOffset = 0;
                }

            }
            //Andra "raden". Två fiender.
            else if (i >= enemysOnFirstRow2 && i < enemysOnSecondRow2)
            {
                Instantiate(enemy, new Vector3(EnemyPlaceOffset, distanceBetweenRows2, 0), Quaternion.identity);

                EnemyPlaceOffset++;

                if (i == enemysOnFourthRow2 - 1)
                {
                    EnemyPlaceOffset = 0;
                }
            }
            //Tre fiender
            else if (i >= enemysOnSecondRow2 && i < enemysOnThirdRow2)
            {
                Instantiate(enemy, new Vector3(EnemyPlaceOffset, distanceBetweenRows2, 0), Quaternion.identity);

                EnemyPlaceOffset++;

                if (i == enemysOnThirdRow2 - 1)
                {
                    EnemyPlaceOffset = 0;
                }
            }
            //Åtta fiender
            else if (i >= enemysOnThirdRow2 && i < enemysOnFourthRow2)
            {
                Instantiate(enemy, new Vector3(EnemyPlaceOffset, distanceBetweenRows2, 0), Quaternion.identity);

                EnemyPlaceOffset++;

                if (i == enemysOnFourthRow2 - 1)
                {
                    EnemyPlaceOffset = 0;
                }

            }
            //Nio fiender
            else if (i >= enemysOnFourthRow2)
            {
                Instantiate(enemy, new Vector3(EnemyPlaceOffset, distanceBetweenRows2, 0), Quaternion.identity);

                EnemyPlaceOffset++;

                //Gör båda till 0 på slutet ifall vi vill göra så att en ny våg kommer efter (Vilket vi antagligen vill?)
                //(Just nu kommer det ingen ny våg)
                if (i == totalAmountOfEnemys2 - 1)
                {
                    EnemyPlaceOffset = 0;
                }
            }
        }
        StartCoroutine(WaveEnded());
    }

    IEnumerator Waves(GameObject[] enemies, float[] spawnWait, Vector3 spawnLocation, Vector3[] offset = null)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            yield return new WaitForSeconds(spawnWait[i]); // Wait for its turn
            Instantiate(enemies[i], spawnLocation + offset[i] != null ? offset[i] : Vector3.zero, Quaternion.identity); // Spawn an enemy
        }
        StartCoroutine(WaveEnded());
    }
    IEnumerator WaveEnded()
    {
        yield return new WaitForSeconds(timeInbetweenWaves);
        _waveEnded = true;
            _wave++;
        ChangeWave();
    }
    void ChangeWave()
    {
        if (_wave > _previousWave && _waveEnded)
        {
            _previousWave = _wave;
            _waveEnded = false;
            print("Wave" + _wave);
            if (_wave == 1)
            {
                StartCoroutine(FormationOne());
            }
            else if (_wave == 2)
            {
                StartCoroutine(FormationTwo());
            }
            else if (_wave - 1 <= enemysArr.Length) // Find better solution, im tired and cant think
            {
                StartCoroutine(Waves(enemysArr[_wave - 1], spawnWaits[_wave - 1], spawnPoints[_wave - 1], offsets[_wave - 1])); // Spawn new wave
            }
            else
            {
                // Continue with random waves
                _wave = 1;
                _previousWave = 0;
            }
        }
    }
}