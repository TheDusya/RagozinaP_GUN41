using Assets.Scripts.Parameters;
using Assets.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovementController : Character //TODO: fix classes
    {
        [Inject]
        PlayerParameters _playerParameters;
        [Inject]
        PlayerSignalBus _signalBus;

        float _originalColliderHeight;
        float _originalColliderY;
        float _smallColliderY;
        float _runningSpeed;
        float _currentSpeed;
        Vector3 _horisontalMovingVector = Vector3.zero;
        
        int _groundAndSceneryLayer;
        Vector3 _feetPosition; 
        bool _isGrounded = true;

        Rigidbody _rigidbody;
        CapsuleCollider _collider; 

        [Inject]
        public void OnEnable()
        {
            _groundAndSceneryLayer = LayerMask.GetMask(NameConstants.LayerNames.GroundLayer, NameConstants.LayerNames.SceneryLayer);
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
            _originalColliderHeight = _collider.height;
            _originalColliderY = _collider.center.y;
            _smallColliderY = (_collider.center.y * _playerParameters.CrouchColliderShlinkCoeff);
            SetParameters();
            SetSubscriptions();
        }

        public override void SetParameters()
        {
            _health = _playerParameters.MaxHealth;
            _maxHealth = _playerParameters.MaxHealth;
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
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(_feetPosition, -transform.up);
        }

        public override void Attack()
        {
            throw new System.NotImplementedException();
        }

        public override void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }
        
        public override void Die()
        {
            throw new System.NotImplementedException();
        }

        public Weapon GetWeapon<Weapon>() 
        {
            throw new System.NotImplementedException("GetWeaponOfType");
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
                _collider.height = _originalColliderHeight * _playerParameters.CrouchColliderShlinkCoeff;
                _collider.center = new Vector3(_collider.center.x, _smallColliderY, _collider.center.z);
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
