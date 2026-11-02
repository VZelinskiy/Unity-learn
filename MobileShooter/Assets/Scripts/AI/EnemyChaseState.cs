using UnityEngine;

public class EnemyChaseState : IState
{
    private readonly EnemyController _enemy;
    //private readonly Transform _playerTransform;
    //private readonly CharacterController _characterController;
    //private readonly float _moveSpeed;
    //private readonly float _rotationSpeed;

    //private readonly float _attackRange;

    public EnemyChaseState(EnemyController enemy)
    {
        _enemy = enemy;
        //_playerTransform = playerTransform;
        //_characterController = characterController;
        //_moveSpeed = moveSpeed;
        //_rotationSpeed = rotationSpeed;
        //_attackRange = attackRange;
    }

    public void Enter()
    {
        _enemy.Animator.SetFloat("Speed", _enemy.MoveSpeed);
    }

    public void Exit()
    {
        _enemy.Animator.SetFloat("Speed", 0);
    }

    public void Update()
    {
        if (_enemy.PlayerTransform == null) return;

        Vector3 toPlayer = (_enemy.PlayerTransform.position - _enemy.transform.position);
        toPlayer.y = 0;
        float sqrDist = toPlayer.sqrMagnitude;

        // якщо підійшли достатньо близько - перемикаємось на стан атаки
        if (sqrDist <= _enemy.AttackRange * _enemy.AttackRange)
        {
            _enemy.SwitchToAttackState();
            return;
        }

        // рух до гравця
        if (sqrDist > 0.01)
        {
            Vector3 direction = toPlayer.normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRotation, _enemy.RotationSpeed * Time.deltaTime);

            _enemy.CharacterController.Move(direction * (_enemy.MoveSpeed * Time.deltaTime));
        }
    }
}
