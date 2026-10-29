using UnityEngine;

public class EnemyChaseState : IState
{
    private readonly EnemyController _enemy;
    private readonly Transform _playerTransform;
    private readonly CharacterController _characterController;
    private readonly float _moveSpeed;
    private readonly float _rotationSpeed;

    private readonly float _attackRange;

    public EnemyChaseState(EnemyController enemy, Transform playerTransform, CharacterController characterController, float moveSpeed, float rotationSpeed, float attackRange)
    {
        _enemy = enemy;
        _playerTransform = playerTransform;
        _characterController = characterController;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
        _attackRange = attackRange;
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (_playerTransform == null) return;

        Vector3 toPlayer = (_playerTransform.position - _enemy.transform.position);
        toPlayer.y = 0;
        float sqrDist = toPlayer.sqrMagnitude;

        // якщо підійшли достатньо близько - перемикаємось на стан атаки
        if (sqrDist <= _attackRange * _attackRange)
        {
            _enemy.SwitchToAttackState();
            return;
        }

        // рух до гравця
        if (sqrDist > 0.01)
        {
            Vector3 direction = toPlayer.normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _characterController.Move(direction * (_moveSpeed * Time.deltaTime));
        }
    }
}
