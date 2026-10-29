using UnityEngine;
using UnityEngine.UI;

public class HaelthBarUI : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        // якщо Health не призначено в інспекторі, шукаємо його на цьому ж обєкті або у батьківському
        if (health == null)
        {
            health = GetComponentInParent<Health>();
        }
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnHealthChanged += UpdateHealthBar;
            // оновлюємо слайдер одразу при активації
            UpdateHealthBar(health.CurrentHealth, health.MaxHealth);
        }
    }

    private void OnDisable()
    {
        if(health != null)
        {
            health.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthSlider  != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }
}
