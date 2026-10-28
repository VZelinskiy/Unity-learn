using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private Transform playerTransform;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3;
    [SerializeField] private float spawnRadius = 12;
    [SerializeField] private int initialPoolSize = 10;

    private ObjectPool<EnemyController> enemyPool;
    private CancellationTokenSource cts;

    private void Awake()
    {
        if (enemyPrefab != null)
        {
            enemyPool = new ObjectPool<EnemyController>(enemyPrefab, initialPoolSize, transform);
        }

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null )
            {
                playerTransform = player.transform;
            }
        }
    }

    private void OnEnable()
    {
        cts = new CancellationTokenSource();
        SpawnLoop(cts.Token).Forget();
    }

    private void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    private async UniTaskVoid SpawnLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            // чекаємо інтервал без створення сміття в памяті
            await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval), cancellationToken: token);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPool == null || playerTransform == null) return;

        // генеруємо випадкову точку на колі навколо гравця
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
        //Debug.Log($"random circle = {randomCircle}");
        Vector3 spawnPosition = playerTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        // дістаємо ворога з пулу
        EnemyController enemy = enemyPool.Get();

        //enemy.transform.position = spawnPosition;
        //enemy.transform.rotation = Quaternion.identity;

        // ініціалізуємо ворога та передаємо йому посилання на повернення в пул, гравця та позицію спавну
        enemy.Initialize(ReturnEnemyToPool, playerTransform, spawnPosition);
    }

    private void ReturnEnemyToPool(EnemyController enemy)
    {
        enemyPool.ReturnToPool(enemy);
    }
}
