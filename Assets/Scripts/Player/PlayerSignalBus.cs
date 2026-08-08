using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerSignalBus : MonoBehaviour
    {
        public event Action<MovementDirection, bool> Move;
        public event Action<bool> Jump;
        public event Action<bool> Run;
        public event Action<bool> Crouch;

        public void OnForward(InputValue value) => Move?.Invoke(MovementDirection.Forward, value.isPressed);

        public void OnBack(InputValue value) => Move?.Invoke(MovementDirection.Backward, value.isPressed);

        public void OnRight(InputValue value) => Move?.Invoke(MovementDirection.Right, value.isPressed);

        public void OnLeft(InputValue value) => Move?.Invoke(MovementDirection.Left, value.isPressed);

        public void OnRun(InputValue value) => Run?.Invoke(value.isPressed);

        public void OnCrouch(InputValue value) => Crouch?.Invoke(value.isPressed);

        public void OnJump() => Jump?.Invoke(true);
        public void OnLand() => Jump?.Invoke(false);
    }
}
