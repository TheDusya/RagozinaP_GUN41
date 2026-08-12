using UnityEngine;

namespace Assets.Scripts.Parameters
{
    [CreateAssetMenu(fileName = "AimingParameters")]
    public class AimingParameters : ScriptableObject
    {
        [SerializeField]
        private float _turningSpeed = 2;
        public float TurningSpeed { get => _turningSpeed; }
        [SerializeField]
        private float _mouseDeadZone = 5f;
        public float MouseDeadZone { get => _mouseDeadZone; }

        [SerializeField]
        private float _aimFOVShrinkCoeff = 0.5f;
        public float AimFOVShrinkCoeff { get => _aimFOVShrinkCoeff; }

        [SerializeField]
        private float _mouseSensitivity = 20;
        public float MouseSensitivity { get => _mouseSensitivity; }

        [SerializeField]
        private float _usualShoulderOffset = 1;
        public float UsualShoulderOffset { get => _usualShoulderOffset; }

        [SerializeField]
        private float _aimingShoulderOffset = 0;
        public float AimingShoulderOffset { get => _aimingShoulderOffset; }

    }
}
