using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts.Player
{
    internal class PlayerAnimationController
    {
        Animator _animator;
        [Inject]
        PlayerSignalBus _signalBus;

        public PlayerAnimationController(Animator animator)
        {

        }
    }
}
