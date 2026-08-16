using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class NPCSignalBus : MonoBehaviour
    {
        public event Action<Transform> TargetDetected;
        public event Action TargetLost;
        public event Action Attack;
        public event Action<float> GetHit;
        public event Action<bool> Look;
        public event Action Walk;
        public event Action Stop;

        public void OnTargetDetected(Transform target) => TargetDetected?.Invoke(target);
        public void OnTargetLost() => TargetLost?.Invoke();
        public void OnLook(bool isLooking) => Look?.Invoke(isLooking);
        public void OnWalk() => Walk?.Invoke();
        public void OnStop() => Stop.Invoke();
        public void OnGetHit(float damage) => GetHit?.Invoke(damage);

    }
}
