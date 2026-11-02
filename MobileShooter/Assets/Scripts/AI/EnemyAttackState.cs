using System;
using UnityEngine;

public class EnemyAttackState : IState
{
    private readonly EnemyController _enemy;
    //private readonly Transform _playerTransform;

    //private readonly float _attackRange;
    //private readonly float _damage;
    //private readonly float _attackCooldown;
    //private float nextAttackTime;
    private bool hasDealtDamage;
    private float attackTimer;
    private float currentAnimationLenght;

    public EnemyAttackState(EnemyController enemy)
    {
        _enemy = enemy;
        //_playerTransform = playerTransform;
        //_attackRange = attackRange;
        //_damage = damage;
        //_attackCooldown = attackCooldown;
    }

    public void Enter()
    {
        //nextAttackTime = 0;
        _enemy.Animator.SetFloat("Speed", 0f);
        _enemy.Animator.SetTrigger("Attack");

        attackTimer = 0;
        hasDealtDamage = false;

        AnimatorStateInfo stateInfo = _enemy.Animator.GetCurrentAnimatorStateInfo(0);
        currentAnimationLenght = stateInfo.length;
    }

    public void Exit()
    {
        _enemy.Animator.ResetTrigger("Attack");
    }

    public void Update()
    {
        if (_enemy.PlayerTransform == null) return;
        
        Vector3 toPlayer = _enemy.PlayerTransform.position - _enemy.transform.position;
        toPlayer.y = 0;
        float sqrDist = toPlayer.sqrMagnitude;

        //// якщо гравець відійшов далі за радіус атаки (+ невеликий буфер 20%), повертаємось у стан переслідування
        //float chaseBufferRange = _enemy.AttackRange * 1.2f;
        //if (sqrDist > chaseBufferRange * chaseBufferRange)
        //{
        //    _enemy.SwitchToChaseState();
        //    return;
        //}

        // плавний поворот обличчям до гравця під час атаки
        if (sqrDist > 0.01)
        {
            Quaternion targetRotation = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRotation, _enemy.RotationSpeed * Time.deltaTime);
        }

        // отримуємо інформацію про поточний стан аніматора (Layer 0)
        AnimatorStateInfo stateInfo = _enemy.Animator.GetCurrentAnimatorStateInfo(0);

        // перевіряємо чи ми дійсно граємо стан атаки
        if (stateInfo.IsName("attack1"))
        {
            attackTimer += Time.deltaTime;

            // урон наноситься у момент самої атаки
            if (attackTimer >= (currentAnimationLenght - 0.1) && !hasDealtDamage)
            {
                TryDealDamage();
                hasDealtDamage = true;
            }

            // коли час виконання всієї анімації закінчився
            if (attackTimer >= currentAnimationLenght)
            {
                CheckDistanceAndAct();
            }
        }


        //// нанесення шкоди з урахуванням кулдауну
        //if (Time.time >= nextAttackTime)
        //{
        //    _enemy.Animator.SetTrigger("Attack");

        //    if (_enemy.PlayerTransform.TryGetComponent<IDamageable>(out var playerDamageable))
        //    {
        //        playerDamageable.TakeDamage(_enemy.DamageToPlayer);
        //    }

        //    nextAttackTime = Time.time + _enemy.AttackCooldown;
        //}
    }

    private void TryDealDamage()
    {
        // перевіряємо дистанцію на момент удару
        float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _enemy.PlayerTransform.position);
        if (distanceToPlayer <= _enemy.AttackRange + 0.5f)
        {
            if (_enemy.PlayerTransform.TryGetComponent<IDamageable>(out var playerDamageable))
            {
                playerDamageable.TakeDamage(_enemy.DamageToPlayer);
            }
            //else
            //{
            //    // Якщо гравець досі поруч — запускаємо атаку повторно (цикл серії ударів)
            //    _enemy.Animator.SetTrigger("Attack");
            //    hasDealtDamage = false;
            //}
        }
    }

    private void CheckDistanceAndAct()
    {
        float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _enemy.PlayerTransform.position);

        if (distanceToPlayer > _enemy.AttackRange)
        {
            _enemy.SwitchToChaseState();
        }
        else
        {
            _enemy.Animator.SetTrigger("Attack");
            attackTimer = 0;
            hasDealtDamage = false;

            AnimatorStateInfo stateInfo = _enemy.Animator.GetCurrentAnimatorStateInfo(0);
            currentAnimationLenght = stateInfo.length;
        }
    }
}
