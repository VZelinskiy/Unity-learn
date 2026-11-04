using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System;
using System.Threading;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private const float lightDuration = 0.05f;

    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Projectile bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Combat Settings")]
    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private int initialPoolSize = 15;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;

    private ObjectPool<Projectile> bulletPool;
    private bool isFiring;
    private CancellationTokenSource firingCts;
    private Animator animator;
    private ParticleSystem muzzleFlashParticles;
    private Light muzzleLight;
    private CancellationTokenSource flashCts;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // отримуємо посилання на ефекти з FirePoint, якщо вони є

        if (firePoint != null)
        {
            muzzleFlashParticles = firePoint.GetComponentInChildren<ParticleSystem>();
            muzzleLight = firePoint.GetComponentInChildren<Light>();

            if (muzzleLight != null) muzzleLight.enabled = false;
        }

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

        if (animator != null)
        {
            animator.SetBool("IsFiring", isFiring);
        }
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

        // відтворення звуку пострілу
        if (audioSource != null && shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }

        // активуємо візуальні ефекти на дулі
        if (muzzleLight != null)
        {
            muzzleFlashParticles.Play();
        }

        if (muzzleLight != null)
        {
            muzzleLight.enabled = true;

            // скасовуємо попередній таймер вимкнення світла, якщо він іще ішов
            flashCts?.Cancel();
            flashCts = new CancellationTokenSource();

            // запускаємо новий асинхронний таймер вимкнення світла
            TurnOffLightAfterDelay(flashCts.Token).Forget();
        }

        // дістаємо кулю з пулу
        Projectile bullet = bulletPool.Get();

        // ставимо її на позицію спавну та орієнтуємо туди ж, куди дивиться гравець
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = transform.rotation;

        // ініціалізуємо кулю, передаючи їй метод повернення назад в пул
        bullet.Initialize(ReturnBulletToPool, transform.forward);
    }

    private async UniTaskVoid TurnOffLightAfterDelay(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lightDuration), cancellationToken: token);

            if (muzzleLight != null)
            {
                muzzleLight.enabled = false;
            }
        }
        catch (OperationCanceledException)
        {
            // це нормально: якщо відбувся новий постріл до того, як минув lightDuration
            // попередній таймер скасовується без помилокб а світло залишається увімкненим для наступнргр пострілу
        }
    }

    private void ReturnBulletToPool(Projectile bullet)
    {
        bulletPool.ReturnToPool(bullet);
    }
}
