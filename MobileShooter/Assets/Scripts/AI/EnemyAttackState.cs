using UnityEngine;

public class EnemyAttackState : IState
{
    private readonly EnemyController _enemy;
    private readonly Transform _playerTransform;

    private readonly float _attackRange;
    private readonly float _damage;
    private readonly float _attackCooldown;
    private float nextAttackTime;

    public EnemyAttackState(EnemyController enemy, Transform playerTransform, float attackRange, float damage, float attackCooldown)
    {
        _enemy = enemy;
        _playerTransform = playerTransform;
        _attackRange = attackRange;
        _damage = damage;
        _attackCooldown = attackCooldown;
    }

    public void Enter()
    {
        // при вході в атаку обнуляємо таймер, щоб ворог бив одразу
        nextAttackTime = 0;
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (_playerTransform == null) return;
        
        Vector3 toPlayer = _playerTransform.position - _enemy.transform.position;
        toPlayer.y = 0;
        float sqrDist = toPlayer.sqrMagnitude;

        // якщо гравець відійшов далі за радіус атаки (+ невеликий буфер 20%), повертаємось у стан переслідування
        float chaseBufferRange = _attackRange * 1.2f;
        if (sqrDist > chaseBufferRange * chaseBufferRange)
        {
            _enemy.SwitchToChaseState();
            return;
        }

        // плавний поворот обличчям до гравця під час атаки
        if (sqrDist > 0.01)
        {
            Quaternion targetRotation = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRotation, 15 * Time.deltaTime);
        }

        // нанесення шкоди з урахуванням кулдауну
        if (Time.time >= nextAttackTime)
        {
            if (_playerTransform.TryGetComponent<IDamageable>(out var playerDamageable))
            {
                playerDamageable.TakeDamage(_damage);
                nextAttackTime = Time.time + _attackCooldown;
            }
        }
    }
}
