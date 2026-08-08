using Assets.Scripts.Parameters;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts.Player
{
    internal class PlayerAnimationController : IDisposable
    {
        Animator _animator;
        PlayerSignalBus _signalBus;
        bool _xMovementIsZero = true;
        bool _zMovementIsZero = false;
        //Implementing all the states is crazy

        public PlayerAnimationController(Animator animator, PlayerSignalBus signalBus)
        {
            _animator = animator;
            _signalBus = signalBus;

            _signalBus.Jump += OnJump;
            _signalBus.Move += OnMove;
            _signalBus.Run += OnRun;
            _signalBus.Crouch += OnCrouch;
        }

        public void Dispose()
        {
            _signalBus.Jump -= OnJump;
            _signalBus.Move -= OnMove;
            _signalBus.Run -= OnRun;
            _signalBus.Crouch -= OnCrouch;
        }

        public void OnJump(bool isStarted) => _animator.SetBool(NameConstants.AnimatorParametersNames.IsJumpingParameter, isStarted);
        public void OnRun(bool isStarted) => _animator.SetBool(NameConstants.AnimatorParametersNames.IsRunningParameter, isStarted);
        public void OnCrouch(bool isStarted) => _animator.SetBool(NameConstants.AnimatorParametersNames.IsCrouchingParameter, isStarted);

        public void OnMove(MovementDirection direction, bool isStarted)
        {
            switch (direction)
            {
                case MovementDirection.Forward:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveZParameter, isStarted ? 1 : 0);
                    _zMovementIsZero = !isStarted;
                    break;
                case MovementDirection.Backward:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveZParameter, isStarted ? -1 : 0);
                    _zMovementIsZero = !isStarted;
                    break;
                case MovementDirection.Right:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveXParameter, isStarted ? 1 : 0);
                    _xMovementIsZero = !isStarted;
                    break;
                case MovementDirection.Left:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveXParameter, isStarted ? -1 : 0);
                    _xMovementIsZero = !isStarted;
                    break;
                default:
                    Debug.LogError("Unexpected MovementDirection value");
                    break;
            }
            _animator.SetBool(NameConstants.AnimatorParametersNames.IsWalkingParameter, !_zMovementIsZero || !_xMovementIsZero);
        }
    }
}
