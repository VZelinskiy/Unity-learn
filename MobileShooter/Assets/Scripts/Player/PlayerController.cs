using System;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private InputReader inputReader;

    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float rotationSpeed = 10;

    private CharacterController characterController;
    private Vector2 moveInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if(inputReader != null)
        {
            inputReader.EnableInput();
        }
        else
        {
            Debug.Log("InputReader is not assigned in PlayerController");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
    }

    private void OnMove(Vector2 input)
    {
        Debug.Log($"PlayerController got input : {input}");
        moveInput = input;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);

        if (direction.sqrMagnitude > 0.01)
        {
            // поворот персонажа в напрямку руху
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // переміщення через CharacterController
            characterController.Move(direction * (moveSpeed * Time.deltaTime));
        }
    }
}
