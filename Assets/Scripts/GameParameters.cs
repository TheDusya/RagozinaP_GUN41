using System;
using UnityEngine;

namespace Assets.Scripts
{
    internal class GameParameters : MonoBehaviour
    {
        public float PunchPower;
        [Space(10)]
        public BallType BallType;

        //not the prettiest decision, but better than nothing
        [SerializeField, Space(10)]
        Material _lightMaterial;
        [SerializeField]
        Material _mediumMaterial;
        [SerializeField]
        Material _heavyMaterial;
        [SerializeField, Space(5)]
        float _lightMass;
        [SerializeField]
        float _mediumMass;
        [SerializeField]
        float _heavyMass;

        public Material GetMaterialByType(BallType ballType) =>
            ballType switch
            {
                BallType.Light => _lightMaterial,
                BallType.Medium => _mediumMaterial,
                BallType.Heavy => _heavyMaterial,
                _ => throw new NotImplementedException()
            };

        public float GetMassByType(BallType ballType) =>
            ballType switch
            {
                BallType.Light => _lightMass,
                BallType.Medium => _mediumMass,
                BallType.Heavy => _heavyMass,
                _ => throw new NotImplementedException()
            };
    }
}
