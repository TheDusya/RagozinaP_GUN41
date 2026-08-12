using Assets.Scripts.Parameters;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Inject]
        PlayerParameters _playerParameters;
        [Inject]
        PlayerSignalBus _signalBus;

        float _originalColliderHeight;
        float _originalColliderY;

        float _runningSpeed;
        float _walkingSpeed;
        float _currentSpeed;
        Vector3 _horisontalMovingVector = Vector3.zero;
        
        int _groundAndSceneryLayer;
        bool _isGrounded = true;

        Rigidbody _rigidbody;
        CapsuleCollider _collider; 

        [Inject]
        public void Inject()
        {
            _groundAndSceneryLayer = LayerMask.GetMask(NameConstants.LayerNames.GroundLayer, NameConstants.LayerNames.SceneryLayer);
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
            _originalColliderHeight = _collider.height;
            _originalColliderY = _collider.center.y;
            SetParameters();
            SetSubscriptions();
        }

        public void SetParameters()
        {
            _runningSpeed = _playerParameters.RunningSpeed;
            _walkingSpeed = _playerParameters.WalkingSpeed;
            _currentSpeed = _walkingSpeed;
        }

        public void SetSubscriptions()
        {
            _signalBus.Move += Move;
            _signalBus.Run += Run;
            _signalBus.Jump += Jump;
            _signalBus.Crouch += Crouch;
        }

        public void FixedUpdate()
        {
            DoHorisontalMovement();
            UpdateGroundedStatus();
        }

        private void DoHorisontalMovement()
        {
            if (_horisontalMovingVector.z == 0 && _horisontalMovingVector.x == 0)
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0); //to optimize just a little
                return;
            }
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 movement = (cameraForward * _horisontalMovingVector.z +
                                cameraRight * _horisontalMovingVector.x) * _currentSpeed;

            _rigidbody.velocity = new Vector3(movement.x, _rigidbody.velocity.y, movement.z);
        }
        private void UpdateGroundedStatus()
        {
            bool wasGrounded = _isGrounded;
            var feetPosition = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y+_playerParameters.FeetOffset, _collider.bounds.center.z);
            _isGrounded = Physics.Raycast(feetPosition, -transform.up, out _, _playerParameters.RaycastGroundDetectionDist, _groundAndSceneryLayer);
            if (_isGrounded && !wasGrounded)
                _signalBus.OnLand();
        }
        #region PlayerInput

        private void Move(MovementDirection direction, bool isStarted)
        {
            switch (direction)
            {
                case MovementDirection.Forward: _horisontalMovingVector.z = isStarted ? 1 : 0;
                    break;
                case MovementDirection.Backward: _horisontalMovingVector.z = isStarted ? -1 : 0;
                    break;
                case MovementDirection.Right: _horisontalMovingVector.x = isStarted ? 1 : 0;
                    break;
                case MovementDirection.Left: _horisontalMovingVector.x = isStarted ? -1 : 0;
                    break;
                default: Debug.LogError("Unknown direction!");
                    break;
            }
        }

        public void Run(bool isStarted) => _currentSpeed = isStarted ? _runningSpeed : _walkingSpeed;
        public void Jump(bool isStarted)
        {
            if (isStarted && _isGrounded)
                _rigidbody.AddForce(Vector3.up * _playerParameters.JumpForce, ForceMode.Impulse);
        }
        public void Crouch(bool isStarted)
        {
            if (isStarted)
            {
                _collider.height = _originalColliderHeight * _playerParameters.CrouchColliderShrinkCoeff;
                _collider.center = new Vector3(_collider.center.x, (_collider.center.y * _playerParameters.CrouchColliderShrinkCoeff), _collider.center.z);
            }
            else
            {
                _collider.height = _originalColliderHeight;
                _collider.center = new Vector3(_collider.center.x, _originalColliderY, _collider.center.z);
            }
        }
        #endregion

        public void OnDestroy()
        {
            _signalBus.Move -= Move;
            _signalBus.Run -= Run;
            _signalBus.Jump -= Jump;
            _signalBus.Crouch -= Crouch;
        }
    }
}
