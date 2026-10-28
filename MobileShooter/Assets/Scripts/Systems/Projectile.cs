using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20;
    [SerializeField] private float lifeTime = 3;

    private Action<Projectile> onReturnToPool;
    private CancellationTokenSource cts;

    public void Initialize(Action<Projectile> returnAction)
    {
        onReturnToPool = returnAction;
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
        // рухаємо кулю вперед по її власній осі z
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
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
}
