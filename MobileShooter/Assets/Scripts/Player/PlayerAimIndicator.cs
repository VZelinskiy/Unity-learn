using UnityEngine;

public class PlayerAimIndicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float distanceFromPlayer = 1.5f;

    private void Awake()
    {
        // якщо гравець не вказаний в інспекторі, автоматично беремо батьківський обєкт
        if (playerTransform == null && transform.parent != null)
        {
            playerTransform = transform.parent;
        }
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        // розміщуємо індикатор перед гравцем у напрямку, куди він дивиться
        Vector3 forwardDir = playerTransform.forward;
        forwardDir.y = 0f;
        forwardDir.Normalize();

        transform.position = playerTransform.position + forwardDir * distanceFromPlayer;
        transform.rotation = playerTransform.rotation;
    }
}
