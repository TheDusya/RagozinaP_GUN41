using Assets.Scripts.Parameters;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerCharacter : Character
    {
        [Inject]
        PlayerParameters _playerParameters;

        float _runningSpeed;
        Rigidbody _rigidbody;
        Collider _collider;
        int _groundLayer;

        float _currentSpeed;
        float _oldRotation;
        float _rotationGoal;
        float _alreadyRotated;

        bool _isMovingHorisontally = false;
        bool _isGrounded = true;
        bool _isTurning = false;

        [Inject]
        public void OnEnable()
        {
            _groundLayer = LayerMask.NameToLayer(NameConstants.LayerNames.GroundLayerName);
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
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
            if (_isTurning)
                DoTurningMovement();
        }
        private void DoHorisontalMovement()
        {
            Vector3 movement = transform.forward * _currentSpeed;
            Vector3 velocity = _rigidbody.velocity;
            velocity.x = movement.x;
            velocity.z = movement.z;
            _rigidbody.velocity = velocity;
        }

        private void DoTurningMovement()
        {
            if (Mathf.Abs(_rotationGoal) - Mathf.Abs(_alreadyRotated) >= _playerParameters.TurningThreshold)
            {
                var rotatedThisTime = _playerParameters.TurningSpeed * Time.deltaTime * Math.Sign(_rotationGoal);
                transform.Rotate(0, rotatedThisTime, 0);
                _alreadyRotated += rotatedThisTime;
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, _oldRotation + _rotationGoal, transform.rotation.z);
                _isTurning = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            bool isStandingOnAGround = collision.gameObject.layer == _groundLayer;
            bool isStandingOnAScenery = false; //TODO
            if (isStandingOnAGround || isStandingOnAScenery)
                _isGrounded = true; //может повлиять на запрыгивание на scenery, решить вопросец
        }
        private void OnCollisionExit(Collision collision)
        {
            bool isLeavingGround = collision.gameObject.layer == _groundLayer;
            bool isLeavingScenery = false; //TODO
            if (isLeavingGround || isLeavingScenery)
                _isGrounded = false;
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

        private void SetRotationGoal(float degree)
        {
            if (_isTurning)
                return; //one turning at a time!
            _rotationGoal = degree;
            _oldRotation = transform.rotation.eulerAngles.y;
            _alreadyRotated = 0;
            _isTurning = true;
        }

        public void OnBack()
        {
            SetRotationGoal(180);
        }
        
        public void OnRight()
        {
            SetRotationGoal(90);
        }
        
        public void OnLeft()
        {
            SetRotationGoal(-90);
        }

        public void OnRun(InputValue value)
        {
            _currentSpeed = value.isPressed ? _runningSpeed : _walkingSpeed;
        }

        public void OnJump()
        {
            if (_isGrounded)
                _rigidbody.AddForce(Vector3.up * _playerParameters.JumpForce, ForceMode.Impulse);
        }
        #endregion
    }
}
