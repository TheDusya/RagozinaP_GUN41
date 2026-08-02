using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
    internal class CharacterAnimationManager : MonoBehaviour
    {
        Animator _animator;
        bool _isCorrect = true;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            if (!_animator.parameters.Where(par => par.name == GameParameters.IsCharacterMovingParameterName).Any())
                Debug.Log($"Speed parameter {GameParameters.IsCharacterMovingParameterName} not found!");
            else
                _isCorrect = true;
        }
        void Update()
        {   
            if (!_isCorrect)
                return;
            bool isMoving = DOTween.IsTweening(transform);
            _animator.SetBool(GameParameters.IsCharacterMovingParameterName, isMoving);
        }
    }
}
