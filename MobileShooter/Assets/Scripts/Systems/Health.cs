using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 50;
    private float currentHealth;

    public event Action<float> OnHealthChanged;
    public event Action OnDie;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth);
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
    }

    private void Die()
    {
        OnDie?.Invoke();
        Debug.Log($"{gameObject.name} died!");

        // для початку просто вимикаємо обєкт (пізніше повертатимо його у свій пул ворогів)
        gameObject.SetActive( false );
    }
}
