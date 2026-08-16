using Assets.Scripts;
using Assets.Scripts.Parameters;
using UnityEngine;
using Zenject;

namespace Assets
{
    public abstract class Weapon : MonoBehaviour
    {
        [Inject]
        protected WeaponParameters _weaponParameters;
        //[Inject]
        //protected AudioManager _audioManager;
        [SerializeField]
        Transform _handle;

        public Transform Handle { get => _handle; }
        public abstract void Equip(Character wielder);
        public void MakeSound(AudioClip clip)
        {
            //audioManager.
        }
    }
}
