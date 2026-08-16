using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NPCSignalBus))]
    public abstract class Enemy : Character
    {
        [SerializeField]
        protected NavMeshAgent _agent;
        [SerializeField]
        protected Path _path;
        protected Animator _animator;
        protected NPCSignalBus _signalBus;
        protected StateMachine _stateMachine;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _signalBus = GetComponent<NPCSignalBus>();
        }
        public void MoveTo(Vector3 goal)
        { //navmesh might be used here
            transform.position = goal;
        }
        private void OnTriggerEnter(Collider other)
        {
            throw new System.NotImplementedException();
        }
    }
}
