using Assets.Scripts.Parameters;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts.Player
{
    internal class ViewController : MonoBehaviour
    {
        [Inject]
        AimingParameters _aimingParameters;
        [Inject]
        PlayerParameters _playerParameters;
        [Inject]
        PlayerSignalBus _signalBus;
        Vector2 _prevMousePos;
        protected float _deadMouseZone;
        protected float _turningSpeed;
        protected int _layerMask;

        private void OnEnable()
        {
            _prevMousePos = Vector2.positiveInfinity;
            _layerMask = ~LayerMask.GetMask(NameConstants.LayerNames.PlayerLayer);
            _deadMouseZone = _playerParameters.MouseDeadZone;
            _turningSpeed = _playerParameters.TurningSpeed;
            _signalBus.Aim += ChangeState;
        }

        private void LateUpdate()
        {
            var mousePos = Mouse.current.position.ReadValue();
            bool isInsideGameWindow = mousePos.x >= 0 && mousePos.y >= 0 &&
                        mousePos.x <= Screen.width && mousePos.y <= Screen.height;
            if (!isInsideGameWindow)
                return;
            if (_prevMousePos != Vector2.positiveInfinity && Vector2.Distance(_prevMousePos, mousePos) > _deadMouseZone)
            {
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out var hit, maxDistance: Mathf.Infinity, layerMask: _layerMask))
                {
                    Vector3 targetPoint = hit.point;
                    targetPoint.y = transform.position.y;
                    Vector3 direction = (targetPoint - transform.position).normalized;
                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * _turningSpeed);
                    }
                }
            }
        }

        private void ChangeState(bool isAiming)
        {
            _deadMouseZone = isAiming? _aimingParameters.MouseDeadZone : _playerParameters.MouseDeadZone;
            _turningSpeed = isAiming ? _aimingParameters.TurningSpeed : _playerParameters.TurningSpeed;
        }
        public void OnDestroy()
        {
            _signalBus.Aim -= ChangeState;
        }
    }
}
