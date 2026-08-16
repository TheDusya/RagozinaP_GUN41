using Assets.Scripts.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.NPCs
{
    [RequireComponent(typeof(NPCSignalBus))]
    public class EnemyLookController : MonoBehaviour
    {
        NPCSignalBus _signalBus;
        LayerMask _obstacleMask;
        LayerMask _playerMask;
        bool _targetWasSeen = false;
        bool _isLookingEnabled = true;

        [SerializeField]
        private float _FOVRadius;
        [SerializeField, Range(0, 360)]
        private float _FOVAngle;

        [Inject]
        private void Inject()
        {
            _playerMask = LayerMask.GetMask(NameConstants.LayerNames.PlayerLayer);
            _obstacleMask = LayerMask.GetMask(NameConstants.LayerNames.WallsLayer, NameConstants.LayerNames.SceneryLayer);
            _signalBus = GetComponent<NPCSignalBus>();
            _signalBus.Look += SwitchLookingMode;
        }

        private void Update()
        {
            if (!_isLookingEnabled)
                return;
            bool targetIsSeen = IsTargetSeen(out var target);
            if (!_targetWasSeen && targetIsSeen)
                _signalBus.OnTargetDetected(target.transform);
            else if (_targetWasSeen && !targetIsSeen)
                _signalBus.OnTargetLost();
            _targetWasSeen = targetIsSeen;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_isLookingEnabled)
                return;
            Handles.color = Color.yellow;
            var position = transform.position;
            Handles.DrawWireDisc(transform.position, Vector3.up, _FOVRadius);
            var leftVector = RotateVector(transform.eulerAngles.y, -_FOVAngle / 2);
            var rightVector = RotateVector(transform.eulerAngles.y, _FOVAngle / 2);
            Handles.DrawLine(position, position + leftVector * _FOVRadius);
            Handles.DrawLine(position, position + rightVector * _FOVRadius);
        }
#endif
        private bool IsTargetSeen(out Collider target)
        {
            target = null;
            Collider[] results = new Collider[1]; //No multiplayer
            int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, _FOVRadius, results, _playerMask);
            if (overlapCount == 0)
                return false;
            if (overlapCount > 1)
                Debug.LogError("too many targets for an enemy!");
            var vectorToTarget = results[0].transform.position - transform.position;
            if (Vector3.Angle(transform.forward, vectorToTarget.normalized) > _FOVAngle / 2)
                return false;
            if (Physics.Raycast(transform.position, vectorToTarget, _FOVRadius, _obstacleMask))
                return false;
            target = results[0];
            return true;
        }
        private Vector3 RotateVector(float orig, float angle)
        {
            angle += orig;
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),  0, Mathf.Cos(angle * Mathf.Deg2Rad));
        }
        private void SwitchLookingMode(bool isLooking) => _isLookingEnabled = isLooking;

        private void OnDestroy()
        {
            _signalBus.Look -= SwitchLookingMode;
        }
    }
}
