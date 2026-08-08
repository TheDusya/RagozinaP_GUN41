using System;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "PlayerParameters")]
    internal class PlayerParameters : ScriptableObject
    {
        public float MaxHealth = 100;
        public float WalkingSpeed = 5;
        public float RunningSpeed = 10;
        public float StrikePower = 10;
        public float JumpForce = 2;
        public float TurningSpeed = 2;
        public float TurningThreshold = 0.001f;
        public float MouseDeadZone = 10f;
        public float RaycastGroundDetectionDist = 0.05f;
        public float FeetOffset = 0.01f;
        public float CrouchColliderShlinkCoeff = 0.5f;

        public WeaponType StartingWeaponType;
    }
}
