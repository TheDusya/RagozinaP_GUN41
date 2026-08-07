using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCharacter : Character
    {
        [Inject]
        PlayerParameters _playerParameters;

        float _runningSpeed;
        Rigidbody _rigidbody;
        PlayerInput _playerInput;

        float _currentSpeed;
        bool _isMovingHorisontally = false;

        [Inject]
        public void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
            SetParameters();
        }

        public override void SetParameters()
        {
            _health = _playerParameters.MaxHealth;
            _maxHealth = _playerParameters.MaxHealth;
            _runningSpeed = _playerParameters.RunningSpeed;
            _walkingSpeed = _playerParameters.WalkingSpeed;
            _currentSpeed = _walkingSpeed;
        }

        public void FixedUpdate()
        {
            if (_isMovingHorisontally)
                DoHorisontalMovement();
        }
        public void DoHorisontalMovement()
        {
            Vector3 movement = transform.forward * _currentSpeed;
            Vector3 velocity = _rigidbody.velocity;
            velocity.x = movement.x;
            velocity.z = movement.z;
            _rigidbody.velocity = velocity;
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
        public void OnForward(InputValue value)
        {
            bool stopPressing = !value.isPressed;
            if (stopPressing)
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            _isMovingHorisontally = value.isPressed;
        }

        private void Rotate(float degree)
        {
            transform.Rotate(new Vector3(0, degree, 0));
        }

        public void OnBack()
        {
            Rotate(180);
        }
        
        public void OnRight()
        {
            Rotate(90);
        }
        
        public void OnLeft()
        {
            Rotate(-90);
        }

        public void OnRun(InputValue value)
        {
            _currentSpeed = value.isPressed ? _runningSpeed : _walkingSpeed;
        }

        public void OnJump()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
