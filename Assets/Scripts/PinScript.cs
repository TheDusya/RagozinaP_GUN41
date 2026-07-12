using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    internal class PinScript : MonoBehaviour
    {
        private const float fallingAngle = 45f; 

        [Inject]
        private EventManager _eventManager;

        private Rigidbody _rigidbody;
        private bool _thePinHasFallen = false;
        private Vector3 _savedPosition;
        private Quaternion _savedRotation;

        [Inject]
        private void Construct()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _savedPosition = transform.position;
            _savedRotation = transform.rotation;
            _eventManager.EndCouple += HandleEndCouple;
        }
        private void Update()
        {
            if (_thePinHasFallen)
                return;
            var vec = Vector3.Angle(transform.up, Vector3.up);
            if (vec > fallingAngle)
            {
                _eventManager.PinFellInvoke();
                _thePinHasFallen = true;
            }
        }
        private void HandleEndCouple() => ResetPin();
        private void ResetPin()
        {
            _thePinHasFallen = false;
            transform.SetPositionAndRotation(_savedPosition, _savedRotation);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
        private void OnDestroy() => _eventManager.EndCouple -= HandleEndCouple;
    }
}
