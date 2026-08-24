using Assets.Scripts.NPCs.NPCStateMachines;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.NPCs
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NPCSignalBus))]
    public abstract class Enemy : Character, IAttacker
    {
        [SerializeField]
        protected NavMeshAgent _agent;
        [SerializeField]
        protected Path _path;
        protected Animator _animator;
        protected NPCAnimationController _animationController;
        protected NPCSignalBus _signalBus;
        protected StateMachine _stateMachine;
        protected float _strikePower;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _signalBus = GetComponent<NPCSignalBus>();
        }
        public void MoveTo(Vector3 goal)
        { 
            //navmesh might be used here
            transform.position = goal;
        }
        private void OnTriggerEnter(Collider other)
        {
            _signalBus.OnAttackMoment();
        }
        public float GetAttackNum() => _strikePower;
    }
}
