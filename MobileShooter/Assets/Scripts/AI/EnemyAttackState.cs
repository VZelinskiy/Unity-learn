using UnityEngine;

public class EnemyAttackState : IState, IAnimationEventReceiver
{
    private readonly EnemyController _enemy;
    //private bool hasDealtDamage;
    private float attackTimer;

    public EnemyAttackState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.Animator.SetFloat("Speed", 0f);
        _enemy.Animator.SetTrigger("Attack");
        //hasDealtDamage = false;
        attackTimer = 0f;
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

        // Плавний поворот обличчям до гравця під час атаки
        if (sqrDist > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRotation, _enemy.RotationSpeed * Time.deltaTime);
        }

        attackTimer += Time.deltaTime;

        //// Наносимо урон на середині атаки
        //if (attackTimer >= (_enemy.AttackDuration / 3) && !hasDealtDamage)
        //{
        //    TryDealDamage();
        //    hasDealtDamage = true;
        //}

        // Коли час атаки завершився
        if (attackTimer >= _enemy.AttackDuration)
        {
            float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _enemy.PlayerTransform.position);

            if (distanceToPlayer > _enemy.AttackRange)
            {
                // Якщо гравець відбіг — чітко перемикаємось на біг/переслідування
                _enemy.SwitchToChaseState();
            }
            else
            {
                // Якщо гравець досі поруч — повторюємо атаку (серія ударів)
                _enemy.Animator.SetTrigger("Attack");
                attackTimer = 0f;
                //hasDealtDamage = false;
            }
        }
    }

    public void TryDealDamage()
    {
        float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _enemy.PlayerTransform.position);
        if (distanceToPlayer <= _enemy.AttackRange + 0.5f)
        {
            if (_enemy.PlayerTransform.TryGetComponent<IDamageable>(out var playerDamageable))
            {
                playerDamageable.TakeDamage(_enemy.DamageToPlayer);
            }
        }
    }

    public void OnAnimationEvent()
    {
        TryDealDamage();
    }
}