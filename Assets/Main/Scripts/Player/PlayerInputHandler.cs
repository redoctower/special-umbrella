using System;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Scripts.Player
{
    public class PlayerInputHandler : NetworkBehaviour
    {
        [SerializeField] private InputActionReference wasdInput;
        [SerializeField] private InputActionReference strafeInput;
    
        public Vector2 WasdInput { get; private set; }
        public float StrafeInput { get; private set; }

        public override void OnStartLocalPlayer()
        {
            if (!isLocalPlayer) return;
            
            EnableAndSubscribe(wasdInput, OnWasdInput);
            EnableAndSubscribe(strafeInput, OnStrafeInput);
        }
    
        private void EnableAndSubscribe(InputActionReference inputRef, Action<InputAction.CallbackContext> callback)
        {
            if (inputRef?.action != null)
            {
                inputRef.action.Enable();
                inputRef.action.performed += callback;
                inputRef.action.canceled += callback;
            }
        }
    
        private void DisableAndUnsubscribe(InputActionReference inputRef, Action<InputAction.CallbackContext> callback)
        {
            if (inputRef?.action != null)
            {
                inputRef.action.performed -= callback;
                inputRef.action.canceled -= callback;
                inputRef.action.Disable();
            }
        }

        private void OnDestroy()
        {
            if (!isLocalPlayer) return;
            
            DisableAndUnsubscribe(wasdInput, OnWasdInput);
            DisableAndUnsubscribe(strafeInput, OnStrafeInput);
        }

        private void OnWasdInput(InputAction.CallbackContext context) => 
            WasdInput = context.ReadValue<Vector2>();

        private void OnStrafeInput(InputAction.CallbackContext context) => 
            StrafeInput = context.ReadValue<float>();
    }
}