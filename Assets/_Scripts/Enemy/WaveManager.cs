using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns all the enemies
/// </summary> - Ruben
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
        // Set sinlgeton instance
        Instance = this;
    }

    // Start is a coroutine
    private IEnumerator Start()
    {
        // Get instances
        _player = PlayerMove.Instance;
        _playerShoot = PlayerShootManager.Instance;
        _enemyStorage = EnemyStorage.Instance;
        _waveText = WaveText.Instance;

        // Get waves
        _waves = GetComponentsInChildren<Wave>();
        _wavesLength = _waves.Length;

        // Set default text in case it's going to be displayed (spoiler: It's not)
        WaveName = "Awaiting A Threat...";

        yield return new WaitForSeconds(4);

        // Enable timer and the ability to pause
        Timer.Enabled = true;

        PauseScreen.CanPause = true;

        // Loop through all available waves
        for (int i = 0; i < _wavesLength; i++)
        {
            // Get the current wave
            Wave wave = _waves[i];

            // Make the wave text appear
            _waveText.Appear(i + 1, wave.WaveName);

            // Set the wave data a
            CurrentWave = i + 1;
            WaveName = wave.WaveName;

            // Heal the player
            _player.ResetHP(i != 0);

            // Wait a bit
            yield return new WaitForSeconds(3);

            // Loop through all mini waves
            int miniWavesLength = wave.Waves.Length;

            for (int miniWaveIndex = 0; miniWaveIndex < miniWavesLength; miniWaveIndex++)
            {
                // Get the current mini wave
                Wave.MiniWave miniWave = wave.Waves[miniWaveIndex];

                // Wait the delay for hte miniwave
                float timer = miniWave.Delay;

                while (timer > 0)
                {
                    timer -= Time.deltaTime;

                    // Alternatively, if there are no enemies, then skip this delay altogether
                    if (_enemyStorage.EnemyAmount <= 0)
                    {
                        break;
                    }

                    yield return null;
                }

                // Cache some data about the miniwave
                int amount = miniWave.Amount;
                int enemyLength = miniWave.Enemies.Length;
                
                // Calculate the amount of time between each enemy spawn
                float waitTime = (float)miniWave.Time / ((float)amount);

                // Spawn as many enemies as listed in the miniwave
                for (int enemyIndex = 0; enemyIndex < amount; enemyIndex++)
                {
                    // Use a modulo operator to ensure the index doesn't go out of range
                    Enemy enemy = SpawnEnemy(miniWave.Enemies[enemyIndex % enemyLength]);

                    // Check if this is the last enemy and if we are going to drop a weapon on the last enemy
                    if (miniWave.LastEnemyDropWeapon && enemyIndex >= amount - 1)
                    {
                        // Ensure the enemy drops a weapon
                        enemy.GiveNewWeapon = true;
                    }

                    // Wait
                    if (waitTime > 0)
                    {
                        yield return new WaitForSeconds(waitTime);

                        /*
                        timer = waitTime;

                        while (timer > 0)
                        {
                            timer -= Time.deltaTime;

                            // Alternatively, if there are no enemies, then skip this wait altogether
                            if (_enemyStorage.EnemyAmount <= 0)
                            {
                                break;
                            }

                            yield return null;
                        }
                        */
                    }

                }
            }

            // Wait until all enemies are killed
            yield return new WaitUntil(() => _enemyStorage.EnemyAmount <= 0);
            yield return new WaitForSeconds(2); // Extra wait time

            // Spawn a boss if we are going to spawn a boss at the end of the wave
            if (wave.SpawnBoss)
            {
                MusicPlayer.StopSong(2);

                yield return new WaitForSeconds(2.5f);

                // Spawn the boss at the correct stage
                Boss boss = Instantiate(bossPrefab, bossPrefab.transform.position, bossPrefab.transform.rotation);
                boss.Stage = wave.BossStage;

                yield return new WaitForSeconds(2);
                yield return new WaitUntil(() => boss == null);

                // Play new music
                _waveText.CanPlayMusic = true;
            }

            // Give a new weapon card to the player if the boolean is set to true
            if (wave.GiveNewWeaponCard)
            {
                _playerShoot.AddWeapon(0);

                yield return new WaitForSeconds(2);
            }
        }

        // All waves are now over, which means that the player wins!

        _player.ResetHP();

        _waveText.CanPlayMusic = false;
        _waveText.YouWin(); // Win text!

        Timer.Enabled = false;

        yield return new WaitForSeconds(3);

        MusicPlayer.PlaySong(1);

        // Win highscore screen
        highscore.ShowHighScoreTable(true);
        PlayerMove.Dead = true; // This is to ensure the player can't move during the win screen
    }

    private Enemy SpawnEnemy(EnemyType type)
    {
        Enemy enemy = regularEnemy;

        // Hard coded
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

        // Return an instantiate of the enemy
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
