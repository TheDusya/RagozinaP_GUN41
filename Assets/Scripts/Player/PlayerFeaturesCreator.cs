using Assets.Scripts.Parameters;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    internal class PlayerFeaturesCreator : MonoBehaviour
    {
        [Inject]
        PlayerSignalBus _playerSignalBus;

        [Inject]
        private void OnEnable()
        {
            var animator = GetComponent<Animator>();
            new PlayerAnimationController(animator, _playerSignalBus);
        }
    }
}
