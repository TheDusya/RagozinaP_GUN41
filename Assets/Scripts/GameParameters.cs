using System;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "GameParameters")]
    internal class GameParameters : ScriptableObject
    {
        public int PinAmount = 10;
        public int SpareBonus;
        public int StrikeBonus;
        [Space(10)]
        public float PunchPower;
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
