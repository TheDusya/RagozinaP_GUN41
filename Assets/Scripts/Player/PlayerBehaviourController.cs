using Assets.Scripts.Weapons;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    internal class PlayerBehaviourController : Character
    {
        [Inject]
        PlayerParameters _playerParameters; 
        public GameObject _weaponPrefab; //пока это тест

        public void OnEnable()
        { 
            Weapon weapon = Instantiate(_weaponPrefab).GetComponent<Weapon>();
            Transform handle = weapon.Handle;

            if (handle != null)
            {
                Vector3 positionOffset = _rightSocket.position - handle.position;
                Quaternion rotationOffset = _rightSocket.rotation * Quaternion.Inverse(handle.rotation);

                weapon.transform.SetPositionAndRotation(weapon.transform.position + positionOffset, 
                                                                rotationOffset * weapon.transform.rotation);
            }
            else
                Debug.Log("No handle found!");
            weapon.transform.SetParent(_rightSocket);
        }
        public override void SetParameters()
        {
            _health = _playerParameters.MaxHealth;
            _maxHealth = _playerParameters.MaxHealth;

        }

        public override void Attack()
        {
            throw new System.NotImplementedException();
        }

        public override void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }

        public override void Die()
        {
            throw new System.NotImplementedException();
        }
        public Weapon GetWeapon<Weapon>()
        {
            throw new System.NotImplementedException("GetWeaponOfType");
        }
    }
}
