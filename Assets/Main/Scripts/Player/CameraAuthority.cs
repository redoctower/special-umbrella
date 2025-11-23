using Mirror;
using UnityEngine;

namespace Main.Scripts.Player
{
    [RequireComponent(typeof(Camera))]
    public class CameraAuthority : NetworkBehaviour
    {
        private Camera _targetCamera;

        public override void OnStartClient()
        {
            if(!isLocalPlayer)
                Destroy(gameObject);
        }
    }
}