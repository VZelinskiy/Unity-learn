using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 50;
    private float currentHealth;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDie;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        NotifyHealthChanged();
        //OnHealthChanged?.Invoke(currentHealth);
        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        gameObject.SetActive(true);
        NotifyHealthChanged();
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        OnDie?.Invoke();
        Debug.Log($"{gameObject.name} died!");

        // для початку просто вимикаємо обєкт (пізніше повертатимо його у свій пул ворогів)
        gameObject.SetActive( false );
    }
}
