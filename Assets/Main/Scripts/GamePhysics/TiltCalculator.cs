using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Main.Scripts.GamePhysics
{
    public class TiltCalculator : NetworkBehaviour
    {
        [SerializeField] private Transform[] rayPoints;
        [SerializeField] private float rayDistance;
        [SerializeField] private float scaler = 1F;
        [SerializeField] private float smoothTime;
        [SerializeField] Vector2 tiltLimits = new Vector2(30F, 30F);
        [SerializeField] private float detectTime = 1F;

        private Vector3[] _hitPoints;

        private Coroutine _detectRoutine;
        private Vector2 _tilts;

        public override void OnStartClient()
        {
            if (!isLocalPlayer) return;
            
            _detectRoutine = StartCoroutine(Detect());   
        }

        private IEnumerator Detect()
        {
            while (true)
            {
                yield return new WaitForSeconds(detectTime);
                _hitPoints = GetHitPoints();
                _tilts = CalculateTiltFromHeights(_hitPoints);
            
                _tilts.x = Mathf.Clamp(_tilts.x, -tiltLimits.x, tiltLimits.x);
                _tilts.y = Mathf.Clamp(_tilts.y, -tiltLimits.y, tiltLimits.y);
            }
        }
        
        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            
            var targetRotation = Quaternion.Euler(-_tilts.x, transform.eulerAngles.y, -_tilts.y);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothTime);
        }

        private Vector3[] GetHitPoints()
        {
            var hitPoints = new List<Vector3>(rayPoints.Length);
            
            foreach (var rayPoint in rayPoints)
            {
                var ray = new Ray
                {
                    origin = rayPoint.position,
                    direction = -rayPoint.up
                };

                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

                if (Physics.Raycast(ray.origin, ray.direction, out var hit, rayDistance))
                {
                    var hitPoint = hit.point;
                    hitPoints.Add(hitPoint);
                }
                else
                    hitPoints.Add(ray.direction * rayDistance);
            }

            return hitPoints.ToArray();
        }

        private void OnDrawGizmos()
        {
            DrawGizmoSpheres(_hitPoints);
        }

        private Vector2 CalculateTiltFromHeights(Vector3[] hitPoints)
        {
            if (hitPoints.Length != 4) return Vector2.zero;
    
            // Предполагаем порядок точек:
            // 0 - перед-левый, 1 - перед-правый, 2 - зад-левый, 3 - зад-правый
    
            // Расчет тангажа (pitch) - наклон вперед/назад
            float frontHeight = (hitPoints[0].y + hitPoints[1].y) / 2f;  // средняя высота передних точек
            float rearHeight = (hitPoints[2].y + hitPoints[3].y) / 2f;   // средняя высота задних точек
            float pitch = rearHeight - frontHeight;  // положительное значение = наклон назад
    
            // Расчет крена (roll) - наклон влево/вправо
            float leftHeight = (hitPoints[0].y + hitPoints[2].y) / 2f;   // средняя высота левых точек
            float rightHeight = (hitPoints[1].y + hitPoints[3].y) / 2f;  // средняя высота правых точек
            float roll = rightHeight - leftHeight;   // положительное значение = наклон вправо
            
            pitch *= scaler;
            roll *= scaler;
    
            return new Vector2(pitch, roll);
        }
        
        private void DrawGizmoSpheres(Vector3[] points, float radius = 0.1f, Color color = default)
        {
            if (points == null) return;
    
            Gizmos.color = color == default ? Color.red : color;
    
            foreach (Vector3 point in points)
            {
                Gizmos.DrawWireSphere(point, radius);
            }
        }
    }
}