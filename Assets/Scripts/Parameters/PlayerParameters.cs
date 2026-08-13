using Cinemachine;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "PlayerParameters")]
    public class PlayerParameters : ScriptableObject
    {
        [Header("Body parameters")]
        [SerializeField]
        private int _maxHealth = 100;
        public int MaxHealth { get => _maxHealth; }

        [SerializeField]
        private float _walkingSpeed = 5;
        public float WalkingSpeed { get => _walkingSpeed; }

        [SerializeField]
        private float _runningSpeed = 10;
        public float RunningSpeed { get => _runningSpeed; }

        [SerializeField]
        private float _strikePower = 10;
        public float StrikePower { get => _strikePower; }

        [SerializeField]
        private float _jumpForce = 2;
        public float JumpForce { get => _jumpForce; }

        [Header("Controls parameters")]
        [SerializeField]
        private float _turningSpeed = 2;
        public float TurningSpeed { get => _turningSpeed; }

        [SerializeField]
        private float _turningThreshold = 0.001f;
        public float TurningThreshold { get => _turningThreshold; }

        [SerializeField]
        private float _mouseUsualDeadZone = 10f;
        public float MouseDeadZone { get => _mouseUsualDeadZone; }

        [SerializeField]
        private float _raycastGroundDetectionDist = 0.05f;
        public float RaycastGroundDetectionDist { get => _raycastGroundDetectionDist; }

        [SerializeField]
        private float _feetOffset = 0.01f;
        public float FeetOffset { get => _feetOffset; }

        [SerializeField]
        private float _crouchColliderShrinkCoeff = 0.5f;
        public float CrouchColliderShrinkCoeff { get => _crouchColliderShrinkCoeff; }

    }
}
