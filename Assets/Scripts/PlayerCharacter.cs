using Assets.Scripts.Parameters;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

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
        int _groundLayer;

        float _currentSpeed;
        float _oldRotation;
        float _rotationGoal;
        float _alreadyRotated;

        bool _isMovingHorisontally = false;
        bool _isGrounded = true;
        bool _isTurning = false;
        Vector3 _horisontalMovingVector = Vector3.zero;

        [Inject]
        public void OnEnable()
        {
            _groundLayer = LayerMask.NameToLayer(NameConstants.LayerNames.GroundLayerName);
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
        private void DoHorisontalMovement()
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 movement = (cameraForward * _horisontalMovingVector.z +
                                cameraRight * _horisontalMovingVector.x) * _currentSpeed;

            Vector3 velocity = _rigidbody.velocity;
            velocity.x = movement.x;
            velocity.z = movement.z;
            _rigidbody.velocity = velocity;
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
                _horisontalMovingVector.z = 0;
            else
                _horisontalMovingVector.z = 1;
            _isMovingHorisontally |= value.isPressed;
        }

        public void OnBack(InputValue value)
        {
            bool stopPressing = !value.isPressed;
            if (stopPressing)
                _horisontalMovingVector.z = 0;
            else
                _horisontalMovingVector.z = -1;
            _isMovingHorisontally |= value.isPressed;
        }
        
        public void OnRight(InputValue value)
        {
            bool stopPressing = !value.isPressed;
            if (stopPressing)
                _horisontalMovingVector.x = 0;
            else
                _horisontalMovingVector.x = 1;
            _isMovingHorisontally |= value.isPressed;
        }
        
        public void OnLeft(InputValue value)
        {
            bool stopPressing = !value.isPressed;
            if (stopPressing)
                _horisontalMovingVector.x= 0;
            else
                _horisontalMovingVector.x = -1;
            _isMovingHorisontally |= value.isPressed;
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
