using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 20;
    [SerializeField] private float lifeTime = 3;
    [SerializeField] private float damage = 25;

    private Action<Projectile> onReturnToPool;
    private CancellationTokenSource cts;
    private Vector3 moveDirection;

    public void Initialize(Action<Projectile> returnAction, Vector3 direction)
    {
        onReturnToPool = returnAction;
        moveDirection = direction.normalized;

        transform.SetParent(null);
    }

    private void OnEnable()
    {
        cts = new CancellationTokenSource();
        LifeTImeRoutine(cts.Token).Forget();
    }

    private void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * (speed * Time.deltaTime));

        // рухаємо кулю у світовому просторі за зафіксованим вектором
        transform.position += moveDirection * (speed * Time.deltaTime);
    }

    private async UniTaskVoid LifeTImeRoutine(CancellationToken token)
    {
        try
        {
            // чекаємо час життя за допомогою UniTask без створення зайвої памяті як у корутинах
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime), cancellationToken: token);

            ReturnSelf();
        }
        catch (OperationCanceledException)
        {
            // обробка скасування таски при вимкненні обєкта
        }
    }

    private void ReturnSelf()
    {
        onReturnToPool?.Invoke(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet collide");

        // перевіряємо, чи обєкт, у який влучили, має компонент здоровя (реалізує IDamageable)
        // також переконуємося, що куля не влучає в самого гравця (якщо у гравця є свій колайдер)
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            // не наносимо шкоду власному гравцю (якщо у гравця теж стоїть Health)
            if (other.CompareTag("Player")) return;

            damageable.TakeDamage(damage);
        }

        // незалежно від того, влучили у ворога чи в перешкоду, повертаємо кулю в пул
        ReturnSelf();
    }
}
