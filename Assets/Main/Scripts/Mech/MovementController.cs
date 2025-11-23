using Main.Scripts.GamePhysics;
using UnityEngine;

namespace Main.Scripts.Mech
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;

        private Rigidbody _rb;

        private void Awake() => 
            _rb = GetComponent<Rigidbody>();

        private void FixedUpdate() =>
            PhysicsUtility.ApplyHorizontalDamping(_rb);

        public void MoveForward(float input) => 
            _rb.AddForce(transform.forward * (input * movementSpeed));

        public void MoveHorizontal(float input) => 
            _rb.AddForce(transform.right * (input * movementSpeed));

        public void RotateY(float input)
        {
            input = Mathf.Clamp(input, -1f, 1f);
            var rotationAmount = input * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
    }
}