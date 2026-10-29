using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Projectile bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Combat Settings")]
    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private int initialPoolSize = 15;

    private ObjectPool<Projectile> bulletPool;
    private bool isFiring;
    private CancellationTokenSource firingCts;

    private void Awake()
    {
        // ініціалізуємо пул куль
        if (bulletPrefab != null)
        {
            bulletPool = new ObjectPool<Projectile>(bulletPrefab, initialPoolSize, transform);
        }
    }

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.AttackEvent += OnFire;
        }

        firingCts = new CancellationTokenSource();
        FiringLoop(firingCts.Token).Forget();
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.AttackEvent -= OnFire;
        }

        firingCts?.Cancel();
        firingCts?.Dispose();
    }

    private void OnFire(bool isFiring)
    {
        this.isFiring = isFiring;
    }

    // асинхронний цикл стрільби на UniTask замість важких корутин чи перевірок у Update
    private async UniTaskVoid FiringLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (isFiring)
            {
                Shoot();
                // чекаємо інтервал між пострілами без Garbage Collection
                await UniTask.Delay(System.TimeSpan.FromSeconds(fireInterval), cancellationToken: token);
            }
            else
            {
                // якщо не стріляємо, перевіряємо стан щокадру, щоб не навантажувати CPU
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
            }
        }
    }

    private void Shoot()
    {
        if (bulletPool == null || firePoint == null) return;

        // дістаємо кулю з пулу
        Projectile bullet = bulletPool.Get();

        // ставимо її на позицію спавну та орієнтуємо туди ж, куди дивиться гравець
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = transform.rotation;

        // ініціалізуємо кулю, передаючи їй метод повернення назад в пул
        bullet.Initialize(ReturnBulletToPool, firePoint.forward);
    }

    private void ReturnBulletToPool(Projectile bullet)
    {
        bulletPool.ReturnToPool(bullet);
    }
}
