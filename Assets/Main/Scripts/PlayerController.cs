using Main.Scripts.Mech;
using Main.Scripts.Player;
using Mirror;
using UnityEngine;

namespace Main.Scripts
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private MovementController movementController;
        [SerializeField] private PlayerInputHandler inputHandler;

        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            
            HandleWASD(inputHandler.WasdInput);
            HandleStrafe(inputHandler.StrafeInput);
        }

        private void HandleWASD(Vector2 input)
        {
            movementController.MoveForward(input.y);
            movementController.RotateY(input.x);
        }

        private void HandleStrafe(float input) => 
            movementController.MoveHorizontal(input);
    }
}