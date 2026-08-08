using Assets.Scripts.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts
{
    class LookAtController : MonoBehaviour
    {
        [Inject]
        PlayerParameters _playerParameters;

        int _layerMask;
        Vector2 _prevMousePos = Vector2.zero;

        private void OnEnable()
        {
            _layerMask = ~(1 << LayerMask.NameToLayer(NameConstants.LayerNames.PlayerLayerName));
        }

        public void FixedUpdate()
        {
            var mousePos = Mouse.current.position.ReadValue();
            if (_prevMousePos != Vector2.zero && Vector2.Distance(_prevMousePos, mousePos) > _playerParameters.mouseDeadZone)
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
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * _playerParameters.TurningSpeed);
                    }
                }
            }
            _prevMousePos = mousePos;
        }
    }
}
