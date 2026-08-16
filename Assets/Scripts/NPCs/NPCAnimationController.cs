using System;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public abstract class NPCAnimationController : IDisposable
    {
        protected NPCSignalBus _signalBus;
        protected Animator _animator;

        public NPCAnimationController(Animator animator, NPCSignalBus signalBus)
        {
            _signalBus = signalBus;
            _animator = animator;
        }
        public abstract void Dispose();
    }
}
