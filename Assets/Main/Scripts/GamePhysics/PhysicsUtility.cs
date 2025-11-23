using UnityEngine;

namespace Main.Scripts.GamePhysics
{
    public class PhysicsUtility
    {
        private const float DAMPING_FACTOR = 0.5F;
        private const float MIN_SPEED = 0.5F;
        
        public static void ApplyHorizontalDamping(Rigidbody rb)
        {
            var velocity = rb.linearVelocity;
            velocity.x *= DAMPING_FACTOR;
            velocity.z *= DAMPING_FACTOR;
    
            if (Mathf.Abs(velocity.x) < MIN_SPEED) velocity.x = 0;
            if (Mathf.Abs(velocity.z) < MIN_SPEED) velocity.z = 0;
    
            rb.linearVelocity = velocity;
        }
        
        public static void DropVelocity(Rigidbody rb)
        {
            var newVelocity = rb.linearVelocity;
            newVelocity.y = 0;
            rb.linearVelocity = newVelocity;
        }
    }
}