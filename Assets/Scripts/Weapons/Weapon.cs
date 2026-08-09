using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        Transform _handle;
        public Transform Handle { get => _handle; }
        public abstract void Initialize();
        public abstract void Equip(Character wielder);
    }
}
