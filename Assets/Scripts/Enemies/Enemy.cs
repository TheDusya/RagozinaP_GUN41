using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

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
        public void MoveTo()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            throw new System.NotImplementedException();
        }
    }
}
