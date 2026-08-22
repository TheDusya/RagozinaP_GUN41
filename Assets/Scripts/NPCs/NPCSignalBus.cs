using System;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class NPCSignalBus : MonoBehaviour
    {
        public event Action<Transform> TargetDetected;
        public event Action TargetLost;
        public event Action Attack;
        public event Action AttackMoment;
        public event Action<float> GetHit;
        public event Action<bool> Look;
        public event Action Walk;
        public event Action Stop;
        public event Action BackOnTrack;
        public event Action Die;

        public void OnTargetDetected(Transform target) => TargetDetected?.Invoke(target);
        public void OnTargetLost() => TargetLost?.Invoke();
        public void OnAttack() => Attack?.Invoke();
        public void OnAttackMoment() => AttackMoment?.Invoke();
        public void OnGetHit(float damage) => GetHit?.Invoke(damage);
        public void OnLook(bool isLooking) => Look?.Invoke(isLooking);
        public void OnWalk() => Walk?.Invoke();
        public void OnStop() => Stop?.Invoke();
        public void OnBackOnTrack() => BackOnTrack?.Invoke();
        public void OnDie() => Die?.Invoke();

    }
}
