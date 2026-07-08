using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts
{
    internal class GameParameters : MonoBehaviour
    {
        [Inject(Id = "Ball")]
        GameObject _ball;
        private Renderer _ballRenderer;
        private Rigidbody _ballRigidbody;

        [SerializeField]
        private BallType _ballType;
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

        [Inject]
        private void AfterInject()
        {
            if (_ball == null)
                throw new Exception("Ball not found!");
            _ballRenderer = _ball.GetComponent<Renderer>();
            _ballRigidbody = _ball.GetComponent<Rigidbody>();
             _ballRenderer.SetMaterials(new List<Material>() { GetMaterialByType(_ballType), _ballRenderer.materials[1] });
            _ballRigidbody.mass = GetMassByType(_ballType);
        }

        private Material GetMaterialByType(BallType ballType) =>
            ballType switch
            {
                BallType.Light => _lightMaterial,
                BallType.Medium => _mediumMaterial,
                BallType.Heavy => _heavyMaterial,
                _ => throw new NotImplementedException()
            };

        private float GetMassByType(BallType ballType) =>
            ballType switch
            {
                BallType.Light => _lightMass,
                BallType.Medium => _mediumMass,
                BallType.Heavy => _heavyMass,
                _ => throw new NotImplementedException()
            };
    }
}
