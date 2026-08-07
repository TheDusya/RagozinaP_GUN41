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

        public WeaponType StartingWeaponType;
    }
}
