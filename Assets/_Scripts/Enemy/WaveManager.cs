using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class WaveManager : MonoBehaviour
{
    [SerializeField] private Enemy regularEnemy;
    [SerializeField] private Enemy shooterEnemy;
    [SerializeField] private Enemy soldierEnemy;
    [SerializeField] private Enemy tankEnemy;
    [SerializeField] private Enemy zigZagEnemy;
    [SerializeField] private Enemy miniEnemy;
    [SerializeField] private Enemy spawnerEnemy;

    [Space]
    [SerializeField] private float spawnRange;

    private Wave[] _waves;
    private int _wavesLength;

    private Boss _boss;
    private EnemyStorage _enemyStorage;
    private WaveText _waveText;

    private IEnumerator Start()
    {
        _boss = Boss.Instance;
        _enemyStorage = EnemyStorage.Instance;
        _waveText = WaveText.Instance;

        _waves = GetComponentsInChildren<Wave>();
        _wavesLength = _waves.Length;

        yield return new WaitForSeconds(4);

        for (int i = 0; i < _wavesLength; i++)
        {
            Wave wave = _waves[i];

            _waveText.Appear(i, wave.WaveName);

            yield return new WaitForSeconds(3);

            int miniWavesLength = wave.Waves.Length;

            for (int miniWaveIndex = 0; miniWaveIndex < miniWavesLength; miniWaveIndex++)
            {
                Wave.MiniWave miniWave = wave.Waves[miniWaveIndex];

                float timer = miniWave.Delay;

                while (timer > 0 && _enemyStorage.EnemyAmount > 0)
                {
                    timer -= Time.deltaTime;

                    yield return null;
                }

                int enemyLength = miniWave.Enemies.Length;

                if (miniWave.Time <= 0)
                {
                    for (int enemyIndex = 0; enemyIndex < enemyLength; enemyIndex++)
                    {
                        SpawnEnemy(miniWave.Enemies[enemyIndex % enemyLength]);
                    }
                }
                else
                {
                    float waitTime = (float)miniWave.Amount / miniWave.Time;

                    for (int enemyIndex = 0; enemyIndex < enemyLength; enemyIndex++)
                    {
                        yield return new WaitForSeconds(waitTime);
                        SpawnEnemy(miniWave.Enemies[enemyIndex % enemyLength]);
                    }
                }
            }

            yield return new WaitUntil(() => _enemyStorage.EnemyAmount <= 0);
            yield return new WaitForSeconds(2);

            if (wave.SpawnBoss)
            {
                MusicPlayer.StopSong(2);

                yield return new WaitForSeconds(2);

                _boss.EnableBoss(wave.BossStage);

                yield return new WaitForSeconds(2);
                yield return new WaitUntil(() => _boss.Dead);
                yield return new WaitForSeconds(4);

                _waveText.CanPlayMusic = true;
            }
        }
    }

    private void SpawnEnemy(EnemyType type)
    {
        Enemy enemy = regularEnemy;

        switch (type)
        {
            case EnemyType.Shooter:
                enemy = shooterEnemy;
                break;
            case EnemyType.Soldier:
                enemy = soldierEnemy;
                break;
            case EnemyType.Tank:
                enemy = tankEnemy;
                break;
            case EnemyType.ZigZag:
                enemy = zigZagEnemy;
                break;
            case EnemyType.Mini:
                enemy = miniEnemy;
                break;
            case EnemyType.Spawner:
                enemy = spawnerEnemy;
                break;
        }

        Instantiate(enemy, transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0), Quaternion.identity);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 pos = transform.position;
        Vector2 offset = new Vector2(spawnRange, 0);
        Vector2 yOffset = new Vector2(0, 1);

        Gizmos.DrawLine(pos + offset + yOffset, pos + offset - yOffset);
        Gizmos.DrawLine(pos - offset + yOffset, pos - offset - yOffset);
    }
#endif
}
