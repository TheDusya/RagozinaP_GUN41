using Assets.Scripts.Parameters;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerMovementController : Character //TODO: fix classes
    {
        [Inject]
        PlayerParameters _playerParameters;
        [Inject]
        PlayerSignalBus _signalBus;

        float _runningSpeed;
        Rigidbody _rigidbody;
        Collider _collider; 
        int _groundAndSceneryLayer;
        Vector3 _feetPosition; 

        float _currentSpeed;

        bool _isGrounded = true;
        Vector3 _horisontalMovingVector = Vector3.zero;

        [Inject]
        public void OnEnable()
        {
            _groundAndSceneryLayer = LayerMask.GetMask(NameConstants.LayerNames.GroundLayer, NameConstants.LayerNames.SceneryLayer);
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
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
            _signalBus.StopMoving += StopMoving;
            _signalBus.Run += Run;
            _signalBus.StopRunning += StopRunning;
            _signalBus.Jump += Jump;
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
            var feetPosition = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y + 0.03f, _collider.bounds.center.z); //и таак сойдет
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

        private void Move(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Forward: _horisontalMovingVector.z = 1;
                    break;
                case MovementDirection.Backward: _horisontalMovingVector.z = -1;
                    break;
                case MovementDirection.Right: _horisontalMovingVector.x = 1;
                    break;
                case MovementDirection.Left: _horisontalMovingVector.x = -1;
                    break;
                default: Debug.LogError("Unknown direction!");
                    break;
            }
        }

        private void StopMoving(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Forward or MovementDirection.Backward: _horisontalMovingVector.z = 0;
                    break;
                case MovementDirection.Right or MovementDirection.Left: _horisontalMovingVector.x = 0;
                    break;
                default: Debug.LogError("Unknown direction!");
                    break;
            }
        }

        public void Run() => _currentSpeed = _runningSpeed;
        public void StopRunning() => _currentSpeed = _walkingSpeed;
        public void Jump()
        {
            if (_isGrounded)
                _rigidbody.AddForce(Vector3.up * _playerParameters.JumpForce, ForceMode.Impulse);
        }
        #endregion

        public void OnDestroy()
        {
            _signalBus.Move -= Move;
            _signalBus.StopMoving -= StopMoving;
            _signalBus.Run -= Run;
            _signalBus.StopRunning -= StopRunning;
            _signalBus.Jump -= Jump;
        }
    }
}
