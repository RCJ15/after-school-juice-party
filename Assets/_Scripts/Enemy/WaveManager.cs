using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [SerializeField] private Enemy regularEnemy;
    [SerializeField] private Enemy shooterEnemy;
    [SerializeField] private Enemy soldierEnemy;
    [SerializeField] private Enemy tankEnemy;
    [SerializeField] private Enemy zigZagEnemy;
    [SerializeField] private Enemy miniEnemy;
    [SerializeField] private Enemy spawnerEnemy;

    [Space]
    [SerializeField] private float spawnRange;
    [SerializeField] private HighScore highscore;

    [Space]
    [SerializeField] private Boss bossPrefab;

    private Wave[] _waves;
    private int _wavesLength;

    private PlayerMove _player;
    private PlayerShootManager _playerShoot;
    private EnemyStorage _enemyStorage;
    private WaveText _waveText;

    public int CurrentWave { get; private set; }
    public string WaveName { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        _player = PlayerMove.Instance;
        _playerShoot = PlayerShootManager.Instance;
        _enemyStorage = EnemyStorage.Instance;
        _waveText = WaveText.Instance;

        _waves = GetComponentsInChildren<Wave>();
        _wavesLength = _waves.Length;

        WaveName = "Awaiting A Threat...";

        yield return new WaitForSeconds(4);

        Timer.Enabled = true;

        PauseScreen.CanPause = true;

        for (int i = 0; i < _wavesLength; i++)
        {
            Wave wave = _waves[i];

            _waveText.Appear(i + 1, wave.WaveName);
            CurrentWave = i + 1;
            WaveName = wave.WaveName;

            _player.ResetHP();

            yield return new WaitForSeconds(3);

            int miniWavesLength = wave.Waves.Length;

            for (int miniWaveIndex = 0; miniWaveIndex < miniWavesLength; miniWaveIndex++)
            {
                Wave.MiniWave miniWave = wave.Waves[miniWaveIndex];

                float timer = miniWave.Delay;

                while (timer > 0)
                {
                    timer -= Time.deltaTime;

                    if (_enemyStorage.EnemyAmount <= 0)
                    {
                        break;
                    }

                    yield return null;
                }

                int enemyLength = miniWave.Enemies.Length;

                if (miniWave.Time <= 0)
                {
                    for (int enemyIndex = 0; enemyIndex < enemyLength; enemyIndex++)
                    {
                        Enemy enemy = SpawnEnemy(miniWave.Enemies[enemyIndex % enemyLength]);

                        // last enemy
                        if (miniWave.LastEnemyDropWeapon && enemyIndex <= enemyLength - 1)
                        {
                            enemy.GiveNewWeapon = true;
                        }
                    }
                }
                else
                {
                    float waitTime = (float)miniWave.Amount / miniWave.Time;

                    for (int enemyIndex = 0; enemyIndex < enemyLength; enemyIndex++)
                    {
                        Enemy enemy = SpawnEnemy(miniWave.Enemies[enemyIndex % enemyLength]);

                        // last enemy
                        if (enemyIndex <= enemyLength - 1)
                        {
                            if (miniWave.LastEnemyDropWeapon)
                            {
                                enemy.GiveNewWeapon = true;
                            }
                        }
                        else
                        {
                            yield return new WaitForSeconds(waitTime);
                        }

                    }
                }
            }

            yield return new WaitUntil(() => _enemyStorage.EnemyAmount <= 0);
            yield return new WaitForSeconds(2);

            if (wave.SpawnBoss)
            {
                MusicPlayer.StopSong(2);

                yield return new WaitForSeconds(2.5f);

                Boss boss = Instantiate(bossPrefab, bossPrefab.transform.position, bossPrefab.transform.rotation);
                boss.Stage = wave.BossStage;

                yield return new WaitForSeconds(2);
                yield return new WaitUntil(() => boss == null);

                _waveText.CanPlayMusic = true;
            }

            if (wave.GiveNewWeaponCard)
            {
                _playerShoot.AddWeapon(0);

                yield return new WaitForSeconds(2);
            }
        }

        _player.ResetHP();

        _waveText.CanPlayMusic = false;
        _waveText.YouWin();

        Timer.Enabled = false;

        yield return new WaitForSeconds(3);

        MusicPlayer.PlaySong(1);

        highscore.ShowHighScoreTable(true);
        PlayerMove.Dead = true;
    }

    private Enemy SpawnEnemy(EnemyType type)
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

        return Instantiate(enemy, transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0), Quaternion.identity);
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
