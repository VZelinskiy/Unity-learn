using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        [Header("Wave Settings")]
        public string WaveName = "Wave 1";
        public EnemyController EnemyPrefab;
        public int EnemyCount = 5;
        public float SpawnInterval = 1.5f;

    }

    [Header("Configuration")]
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnRadius = 12f;
    [SerializeField] private float timeBetweenWaves = 3;
    [SerializeField] private int initialPoolSizePerType = 10;
    [SerializeField] private Transform playerTransform;

    public event Action<int> OnWaveStarted;
    public event Action<int> OnWaveCompleted;
    public event Action OnAllWavesCompleted;

    private int currentWaveIndex = 0;
    private int enemiesAliveCount = 0;
    private CancellationTokenSource waveCTS;
    private bool isWaveRunning = false;

    // словник пуллів для різних типів ворогів (на випадок кількох префабів у різних хвилях)
    private Dictionary<EnemyController, ObjectPool<EnemyController>> enemyPools = new Dictionary<EnemyController, ObjectPool<EnemyController>>();

    private void Awake()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("WaveManager: Player with tag 'Player' not found on the scene!");
            }
        }

        InitializePools();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (waves ==  null || waves.Count == 0)
        {
            Debug.LogWarning("WaveManager: No waves configured!");
            return;
        }

        //InitializePools();

        waveCTS = new CancellationTokenSource();
        StartNextWaveAsync(waveCTS.Token).Forget();
    }

    private void InitializePools()
    {
        foreach (var wave in waves)
        {
            if (wave.EnemyPrefab != null && !enemyPools.ContainsKey(wave.EnemyPrefab))
            {
                // створюємо пул ждя кожного унікального префаба ворога
                // батьківським обєктом робимо сам WaveManager, щоб ієрархія була чистою
                var pool = new ObjectPool<EnemyController>(wave.EnemyPrefab, initialPoolSizePerType, transform);
                enemyPools.Add(wave.EnemyPrefab, pool);
            }
        }
    }

    private void OnDestroy()
    {
        waveCTS?.Cancel();
        waveCTS?.Dispose();
    }

    private async UniTaskVoid StartNextWaveAsync(CancellationToken token)
    {
        // чекаємо паузу перед стартом першої хвилі або між хвилями
        if (currentWaveIndex > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeBetweenWaves), cancellationToken: token);
        }

        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All waves copleted successfully!");
            OnAllWavesCompleted?.Invoke();
            return;
        }

        Wave currentWave = waves[currentWaveIndex];
        isWaveRunning = true;
        enemiesAliveCount = currentWave.EnemyCount;

        Debug.Log($"Starting {currentWave.WaveName} with {currentWave.EnemyCount} enemies.");
        OnWaveStarted?.Invoke(currentWaveIndex + 1);

        // спавнимо ворогів хвилі у фоновому фсинхронному потоці
        SpawnWaveEnemiesAsync(currentWave, token).Forget();
    }

    private async UniTaskVoid SpawnWaveEnemiesAsync(Wave wave, CancellationToken token)
    {
        for (int i = 0; i < wave.EnemyCount; i++)
        {
            if (token.IsCancellationRequested) return;

            SpawnEnemy(wave.EnemyPrefab);

            // інтервал між спавном окремих ворогів у хвилі
            if (i < wave.EnemyCount - 1)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(wave.SpawnInterval), cancellationToken: token);
            }
        }
    }

    private void SpawnEnemy(EnemyController enemyPrefab)
    {
        if (enemyPrefab == null || !enemyPools.ContainsKey(enemyPrefab) || playerTransform == null) return;

        // визначаємо позицію спавну (або з масиву точок, або навколо гравця
        Vector3 spawnPosition;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform sp = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            spawnPosition = sp.position;
        }
        else
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
            spawnPosition = playerTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        }

        EnemyController enemy = enemyPools[enemyPrefab].Get();

        // ініціалізуємо ворога та передаємо йому посилання на повернення в пул, гравця та позицію спавну
        enemy.Initialize(ReturnEnemyToPool, playerTransform,spawnPosition);
    }

    private void ReturnEnemyToPool(EnemyController enemy)
    {
        foreach (var pair in enemyPools)
        {
            pair.Value.ReturnToPool(enemy);
            break;
        }

        enemiesAliveCount--;
        Debug.Log($"Enemy defeated. Remaining in wave: {enemiesAliveCount}");

        if (enemiesAliveCount <= 0 && isWaveRunning)
        {
            CompleteCurrentWave();
        }
    }

    private void CompleteCurrentWave()
    {
        isWaveRunning = false;
        Debug.Log($"Wave {currentWaveIndex + 1} completed!");
        OnWaveCompleted?.Invoke(currentWaveIndex + 1);

        currentWaveIndex++;

        if (waveCTS != null && !waveCTS.IsCancellationRequested)
        {
            StartNextWaveAsync(waveCTS.Token).Forget();
        }
    }
}
