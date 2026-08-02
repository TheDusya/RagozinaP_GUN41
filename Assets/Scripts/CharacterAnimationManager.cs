using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
    internal class CharacterAnimationManager : MonoBehaviour
    {
        Animator _animator;
        bool _isCorrect = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            if (!_animator.parameters.Where(par => par.name == GameParameters.IsCharacterMovingParameterName).Any())
                Debug.Log($"Speed parameter {GameParameters.IsCharacterMovingParameterName} not found!");
            else
            {
                _isCorrect = true;
                InGameEventManager.PathWasChosen += StartMoving;
                InGameEventManager.PathWasFinished += EndMoving;
            }

        }
        void StartMoving(Path path) => _animator.SetBool(GameParameters.IsCharacterMovingParameterName, true);
        void EndMoving() => _animator.SetBool(GameParameters.IsCharacterMovingParameterName, false);
        private void OnDestroy()
        {
            if (!_isCorrect)
                return;
            InGameEventManager.PathWasChosen -= StartMoving;
            InGameEventManager.PathWasFinished -= EndMoving;
        }
    }
}
