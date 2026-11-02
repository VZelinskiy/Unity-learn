using System;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 10;

    [Header("Combat Settings")]
    [SerializeField] private float damageToPlayer = 10;
    //[SerializeField] private float attackCooldown = 1;
    [SerializeField] private float attackRange = 1.5f;

    //private float nextAttackTime;
    public Transform PlayerTransform { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Health Health { get; private set; }
    public Animator Animator { get; private set; }

    public float MoveSpeed => moveSpeed;
    public float RotationSpeed => rotationSpeed;
    public float DamageToPlayer => damageToPlayer;
    //public float AttackCooldown => attackCooldown;
    public float AttackRange => attackRange;

    private Action<EnemyController> onReturnToPool;
    private StateMachine stateMachine;
    private IState chaseState;
    private IState attackState;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        Health = GetComponent<Health>();
        Animator = GetComponent<Animator>();
        stateMachine = new StateMachine();
    }

    // ініціалізація при отриманні з пулу
    public void Initialize(Action<EnemyController> returnAction, Transform player, Vector3 spawnPosition)
    {
        onReturnToPool = returnAction;
        PlayerTransform = player;

        if (Health != null)
        {
            Health.ResetHealth();
        }

        // вимикаємо CharacterController перед телепортацією з пулу
        CharacterController.enabled = false;
        transform.position = spawnPosition;
        CharacterController.enabled = true;

        // скидання аніматора
        if (Animator  != null)
        {
            Animator.enabled = true;
            Animator.ResetTrigger("Attack");
            Animator.SetBool("IsDead", false);
        }

        // ініціалізуємо стани
        //chaseState = new EnemyChaseState(this, PlayerTransform, CharacterController, moveSpeed , rotationSpeed, attackRange);
        //attackState = new EnemyAttackState(this, PlayerTransform, attackRange, damageToPlayer, attackCooldown);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);

        // плчинаємо з переслідування
        stateMachine.Initialize(chaseState);
    }

    public void SwitchToChaseState()
    {
        stateMachine.ChangeState(chaseState);
    }

    public void SwitchToAttackState()
    {
        stateMachine.ChangeState(attackState);
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
        if (Health  != null) Health.OnDie += HandleDeath;
    }

    private void OnDisable()
    {
        if (Health != null) Health.OnDie -= HandleDeath;
    }

    private void HandleDeath()
    {
        if (Animator != null)
        {
            Animator.SetBool("IsDead", true);
        }

        // замість деактивації напряму повертаємо в пул через делегат
        onReturnToPool?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerTransform == null) return;

        //MoveAndRotateTowardsPlayer();

        stateMachine.Update();
    }

    //private void MoveAndRotateTowardsPlayer()
    //{
    //    // рахуємо вектор напрямку до гравця
    //    Vector3 direction = (playerTransform.position - transform.position);
    //    // фіксуємо висоту щоб ворог не літав і не нахилявся по вертикалі
    //    direction.y = 0;

    //    if (direction.sqrMagnitude > 0.01)
    //    {
    //        direction.Normalize();

    //        // плавний поворот до гравця
    //        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    //        // рух за допомогою CharacterController
    //        characterController.Move(direction * (moveSpeed * Time.deltaTime));
    //    }
    //}

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    // якщо ворог врізався у гравця, наносимо йому урон
    //    if (hit.gameObject.CompareTag("Player"))
    //    {
    //        // перевіряємо, чи минув кулдаун
    //        if (Time.time >= nextAttackTime)
    //        {
    //            if (hit.gameObject.TryGetComponent<IDamageable>(out var playerDamageable))
    //            {
    //                playerDamageable.TakeDamage(damageToPlayer);

    //                // встановлюємо час наступної можливої атаки
    //                nextAttackTime = Time.time + attackCooldown;
    //            }
    //        }
    //    }
    //}
}
