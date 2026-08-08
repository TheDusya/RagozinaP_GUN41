using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerSignalBus : MonoBehaviour
    {
        public event Action<MovementDirection> Move;
        public event Action<MovementDirection> StopMoving;
        public event Action Jump;
        public event Action Land;
        public event Action Run;
        public event Action StopRunning;

        #region PlayerInput

        public void OnForward(InputValue value) => (value.isPressed ? Move : StopMoving)?.Invoke(MovementDirection.Forward);

        public void OnBack(InputValue value) => (value.isPressed ? Move : StopMoving)?.Invoke(MovementDirection.Backward);

        public void OnRight(InputValue value) => (value.isPressed ? Move : StopMoving)?.Invoke(MovementDirection.Right);

        public void OnLeft(InputValue value) => (value.isPressed ? Move : StopMoving)?.Invoke(MovementDirection.Left);

        public void OnRun(InputValue value) => (value.isPressed ? Run : StopRunning)?.Invoke();

        public void OnJump() => Jump?.Invoke();
        public void OnLand() => Land?.Invoke();
        #endregion
    }
}
