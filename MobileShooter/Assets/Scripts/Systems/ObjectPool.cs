using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Transform _parentTransform;
    private readonly Queue<T> _pool = new Queue<T>();

    public ObjectPool(T prefab, int initialSize = 10, Transform parentTransform  = null)
    {
        _prefab = prefab;
        _parentTransform = parentTransform;

        // попередньо заповнюємо пул обєктами на старті, щоб уникнути ривків під час гри
        for (int i = 0; i < initialSize; i++)
        {
            T instance = GameObject.Instantiate(_prefab, _parentTransform);
            instance.gameObject.SetActive(false);
            _pool.Enqueue(instance);
        }
    }

    public T Get()
    {
        T instance;

        if (_pool.Count > 0)
        {
            instance = _pool.Dequeue();
        }
        else
        {
            // якщо пул спорожнів, створюємо новий екземпляр динамічно
            instance = GameObject.Instantiate(_prefab, _parentTransform);
        }

        instance.gameObject.SetActive(true);
        return instance;
    }

    public void ReturnToPool(T instance)
    {
        instance.gameObject.SetActive(false);
        _pool.Enqueue(instance);
    }
}
