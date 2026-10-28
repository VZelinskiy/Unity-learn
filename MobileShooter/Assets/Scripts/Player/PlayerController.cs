using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private InputReader inputReader;
    [SerializeField] private Projectile bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float rotationSpeed = 10;

    [Header("Combat Settings")]
    private float fireInterval = 0.2f;
    private int initialPoolSize = 15;

    private CharacterController characterController;
    private ObjectPool<Projectile> bulletPool;
    private Vector2 moveInput;
    private bool isFiring;
    private CancellationTokenSource firingCts;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        // ініціалізуємо пул куль
        if (bulletPrefab != null )
        {
            bulletPool = new ObjectPool<Projectile>(bulletPrefab, initialPoolSize, transform);
        }

        if(inputReader != null)
        {
            inputReader.EnableInput();
        }
        else
        {
            Debug.Log("InputReader is not assigned in PlayerController");
        }
    }

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
        inputReader.AttackEvent += OnFire;

        firingCts = new CancellationTokenSource();
        FiringLoop(firingCts.Token).Forget();
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
        inputReader.AttackEvent -= OnFire;

        firingCts?.Cancel();
        firingCts?.Dispose();
    }

    private void OnMove(Vector2 input)
    {
        Debug.Log($"PlayerController got input : {input}");
        moveInput = input;
    }

    private void OnFire(bool isFiring)
    {
        this.isFiring = isFiring;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        if (direction.sqrMagnitude > 0.01)
        {
            // поворот персонажа в напрямку руху
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // переміщення через CharacterController
            characterController.Move(direction * (moveSpeed * Time.deltaTime));
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

        // дістаємо кулю з пулу
        Projectile bullet = bulletPool.Get();

        // ставимо її на позицію спавну та орієнтуємо туди ж, куди дивиться гравець
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = transform.rotation;

        // ініціалізуємо кулю, передаючи їй метод повернення назад в пул
        bullet.Initialize(ReturnBulletToPool);
    }

    private void ReturnBulletToPool(Projectile bullet)
    {
        bulletPool.ReturnToPool(bullet);
    }
}
