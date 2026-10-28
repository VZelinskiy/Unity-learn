using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
{
    public event Action<Vector2> MoveEvent = delegate { };
    public event Action<bool> AttackEvent = delegate { };

    private InputSystem_Actions gameInput;

    //private void OnEnable()
    //{
    //    Debug.Log("InputReader Enabled");
    //    if (gameInput == null)
    //    {
    //        gameInput = new InputSystem_Actions();
    //        gameInput.Player.SetCallbacks(this);
    //    }
    //    gameInput.Player.Enable();
    //}

    private void OnDisable()
    {
        gameInput.Player.Disable();
    }

    public void EnableInput()
    {
        Debug.Log("InputReader Enabled");
        if (gameInput == null)
        {
            gameInput = new InputSystem_Actions();
            gameInput.Player.SetCallbacks(this);
        }
        gameInput.Player.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 val = context.ReadValue<Vector2>();
        //Debug.Log($"Input received : {val}");
        MoveEvent.Invoke(val);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) AttackEvent.Invoke(true);
        else if (context.canceled) AttackEvent.Invoke(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //throw new NotImplementedException();
    }
}
