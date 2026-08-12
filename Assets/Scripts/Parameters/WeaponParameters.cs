using UnityEngine;

namespace Assets.Scripts.Parameters
{
    [CreateAssetMenu(fileName = "WeaponParameters")]
    public class WeaponParameters : ScriptableObject
    {
        [Header("Pistol")]
        [SerializeField]
        private int _maxCapacity = 6;
        public int MaxCapacity { get => _maxCapacity; }
        [SerializeField]
        private float _reloadTime = 1;
        public float ReloadTime { get => _reloadTime; }

    }
}
