using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    internal class FieldOfView : MonoBehaviour
    {
        [SerializeField] 
        private float _radius;
        [SerializeField, Range(0, 360)]
        private float _angle;
        [SerializeField]
        private Transform _enemy; //one for simplicity
        [SerializeField]
        private LayerMask _obstacleMask;

        private void OnDrawGizmos()
        {
            var position = transform.position;
            Handles.color = Color.white;
            Handles.DrawWireDisc(transform.position, Vector3.up, _radius);
            var isSeen = IsPlayerSeen();
            Handles.color = isSeen ? Color.red : Color.green;
            var leftVector = RotateVector(transform.eulerAngles.y, -_angle/2);
            var rightVector = RotateVector(transform.eulerAngles.y, _angle/2);
            Handles.DrawLine(position, position + leftVector * _radius);
            Handles.DrawLine(position, position + rightVector * _radius);
            if (isSeen)
                Handles.DrawLine(position, _enemy.position);
        }

        private bool IsPlayerSeen()
        {
            var vectorToTarget = _enemy.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, vectorToTarget.normalized) > _angle / 2)
                return false;
            if (Physics.Raycast(transform.position, vectorToTarget, _radius, _obstacleMask))
                return false;
            return true;
        }

        private Vector3 RotateVector(float orig, float angle)
        {
            angle += orig;
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        } 
    }
}
