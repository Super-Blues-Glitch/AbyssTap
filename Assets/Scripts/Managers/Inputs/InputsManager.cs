using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    InputSystem_Actions inputs;

    [SerializeField] CursorLockMode cursorMode;

    private void Awake()
    {
        ChangeCursorState(cursorMode);

        inputs = new();

        inputs.Enable();
    }

    public void Subscribe(GameEnums.InputKey key, InputActionPhase phase, Action<InputAction.CallbackContext> func)
    {
        switch (key)
        {
            case GameEnums.InputKey.Move:
                switch (phase)
                {
                    case InputActionPhase.Started:
                        inputs.Player.Move.started += func;
                    break;

                    case InputActionPhase.Performed:
                        inputs.Player.Move.performed += func;
                    break;

                    case InputActionPhase.Canceled:
                        inputs.Player.Move.canceled += func;
                    break;
                }
            break;

            case GameEnums.InputKey.Jump:
                switch (phase)
                {
                    case InputActionPhase.Started:
                        inputs.Player.Jump.started += func;
                        break;

                    case InputActionPhase.Performed:
                        inputs.Player.Jump.performed += func;
                        break;

                    case InputActionPhase.Canceled:
                        inputs.Player.Jump.canceled += func;
                        break;
                }
                break;
        }
    }

    public void Unsubscribe(GameEnums.InputKey key, InputActionPhase phase, Action<InputAction.CallbackContext> func)
    {
        switch (key)
        {
            case GameEnums.InputKey.Move:
                switch (phase)
                {
                    case InputActionPhase.Started:
                        inputs.Player.Move.started -= func;
                        break;

                    case InputActionPhase.Performed:
                        inputs.Player.Move.performed -= func;
                        break;

                    case InputActionPhase.Canceled:
                        inputs.Player.Move.canceled -= func;
                        break;
                }
                break;

            case GameEnums.InputKey.Jump:
                switch (phase)
                {
                    case InputActionPhase.Started:
                        inputs.Player.Jump.started -= func;
                        break;

                    case InputActionPhase.Performed:
                        inputs.Player.Jump.performed -= func;
                        break;

                    case InputActionPhase.Canceled:
                        inputs.Player.Jump.canceled -= func;
                        break;
                }
                break;
        }
    }

    public void ChangeCursorState(CursorLockMode mode) => Cursor.lockState = mode;

    public Vector2 GetMoveValue() => inputs.Player.Move.ReadValue<Vector2>();
}
