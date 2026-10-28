using System;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
[RequireComponent(typeof(Health))]
public class EnemyController : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 10;

    [Header("Combat Settings")]
    [SerializeField] private float damageToPlayer = 10;
    [SerializeField] private float attackCooldown = 1;

    private float nextAttackTime;
    private Transform playerTransform;
    private CharacterController characterController;
    private Health health;
    private Action<EnemyController> onReturnToPool;

    // ініціалізація при отриманні з пулу
    public void Initialize(Action<EnemyController> returnAction, Transform player, Vector3 spawnPosition)
    {
        onReturnToPool = returnAction;
        playerTransform = player;

        if (health != null)
        {
            health.ResetHealth();
        }

        // вимикаємо CharacterController перед телепортацією з пулу
        characterController.enabled = false;
        transform.position = spawnPosition;
        characterController.enabled = true;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Health>();
    }

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    // шукаємо гравця на сцені за тегом "Player"
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    if (player != null)
    //    {
    //        playerTransform = player.transform;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("EnemyController: Player with tag 'Player' not found on the scene!");
    //    }
    //}

    private void OnEnable()
    {
        if(health  != null)
        {
            health.OnDie += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDie -= HandleDeath;
        }
    }

    private void HandleDeath()
    {
        // замість деактивації напряму повертаємо в пул через делегат
        onReturnToPool?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) return;

        MoveAndRotateTowardsPlayer();
    }

    private void MoveAndRotateTowardsPlayer()
    {
        // рахуємо вектор напрямку до гравця
        Vector3 direction = (playerTransform.position - transform.position);
        // фіксуємо висоту щоб ворог не літав і не нахилявся по вертикалі
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01)
        {
            direction.Normalize();

            // плавний поворот до гравця
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // рух за допомогою CharacterController
            characterController.Move(direction * (moveSpeed * Time.deltaTime));
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // якщо ворог врізався у гравця, наносимо йому урон
        if (hit.gameObject.CompareTag("Player"))
        {
            // перевіряємо, чи минув кулдаун
            if (Time.time >= nextAttackTime)
            {
                if (hit.gameObject.TryGetComponent<IDamageable>(out var playerDamageable))
                {
                    playerDamageable.TakeDamage(damageToPlayer);

                    // встановлюємо час наступної можливої атаки
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }
}
