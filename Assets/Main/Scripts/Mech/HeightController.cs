using Main.Scripts.GamePhysics;
using Mirror;
using UnityEngine;

namespace Main.Scripts.Mech
{
    [RequireComponent(typeof(Rigidbody))]
    public class HeightController : NetworkBehaviour
    {
        [SerializeField] private Vector3 rayOrigin;
        [SerializeField] private float hoverHeight = 2F;
        [SerializeField] private float heightSmooth = 3F;
        private Rigidbody _rb;
        
        private void Awake() => 
            _rb = GetComponent<Rigidbody>();

        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            
            ControlHeight();
        }

        private void ControlHeight()
        {
            var ray = new Ray(transform.position + rayOrigin, Vector3.down);

            if (Physics.Raycast(ray.origin, ray.direction, out var hit, hoverHeight + 1))
            {
                var newPosition = transform.position;
                newPosition.y = hit.point.y + hoverHeight;
                
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * heightSmooth);

                PhysicsUtility.DropVelocity(_rb);
                Debug.DrawRay(ray.origin, ray.direction * hoverHeight, Color.red);
            }
        }
    }
}