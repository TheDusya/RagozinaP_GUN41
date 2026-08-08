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
            _signalBus.Land += OnLand;
            _signalBus.Move += OnMove;
            _signalBus.StopMoving += OnStopMoving;
            _signalBus.Run += OnRun;
            _signalBus.StopRunning += OnStopRunning;
        }

        public void Dispose()
        {
            _signalBus.Jump -= OnJump;
            _signalBus.Land -= OnLand;
            _signalBus.Move -= OnMove;
            _signalBus.StopMoving -= OnStopMoving;
            _signalBus.Run -= OnRun;
            _signalBus.StopRunning -= OnStopRunning;

        }

        public void OnJump()
        {
            //_isJumping = true;
            _animator.SetBool(NameConstants.AnimatorParametersNames.IsJumpingParameter, true);
        }

        public void OnLand()
        {
            //_isJumping = false;
            _animator.SetBool(NameConstants.AnimatorParametersNames.IsJumpingParameter, false);
        }
        public void OnMove(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Forward:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveZParameter, 1);
                    _zMovementIsZero = false;
                    break;
                case MovementDirection.Backward:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveZParameter, -1);
                    _zMovementIsZero = false;
                    break;
                case MovementDirection.Right:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveXParameter, 1);
                    _xMovementIsZero = false;
                    break;
                case MovementDirection.Left:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveXParameter, -1);
                    _xMovementIsZero = false;
                    break;
                default:
                    Debug.LogError("Unexpected MovementDirection value");
                    break;
            }
            _animator.SetBool(NameConstants.AnimatorParametersNames.IsWalkingParameter, true);
        }
        public void OnStopMoving(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Forward or MovementDirection.Backward:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveZParameter, 0);
                    _zMovementIsZero = true;
                    break;
                case MovementDirection.Right or MovementDirection.Left:
                    _animator.SetFloat(NameConstants.AnimatorParametersNames.MoveXParameter, 0);
                    _xMovementIsZero = true;
                    break;
                default:
                    Debug.LogError("Unexpected MovementDirection value");
                    break;
            }
            _animator.SetBool(NameConstants.AnimatorParametersNames.IsWalkingParameter, !_zMovementIsZero || !_xMovementIsZero);
        }
        public void OnRun() => _animator.SetBool(NameConstants.AnimatorParametersNames.IsRunningParameter, true);
        public void OnStopRunning() => _animator.SetBool(NameConstants.AnimatorParametersNames.IsRunningParameter, false);
    }
}
