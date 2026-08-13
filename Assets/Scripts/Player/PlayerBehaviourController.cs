using Assets.Scripts.Weapons;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
    public class PlayerBehaviourController : Character
    {
        [Inject]
        PlayerParameters _playerParameters; 
        public GameObject _weaponPrefab; //пока это тест
        public Weapon _currentWeapon;

        public void OnEnable()
        { 
            _currentWeapon = Instantiate(_weaponPrefab).GetComponent<Weapon>();
            Transform handle = _currentWeapon.Handle;

            if (handle != null)
            {
                Vector3 positionOffset = _rightSocket.position - handle.position;
                Quaternion rotationOffset = _rightSocket.rotation * Quaternion.Inverse(handle.rotation);

                _currentWeapon.transform.SetPositionAndRotation(_currentWeapon.transform.position + positionOffset, 
                                                                rotationOffset * _currentWeapon.transform.rotation);
            }
            else
                Debug.Log("No handle found!");
            _currentWeapon.transform.SetParent(_rightSocket);
        }

        public override void SetParameters()
        {
            _health = _playerParameters.MaxHealth;
            _maxHealth = _playerParameters.MaxHealth;

        }

        public void RangedAttack()
        {

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
        public void OnDisable()
        {
            if (_currentWeapon != null)
            {
                Destroy(_currentWeapon.gameObject);
                _currentWeapon = null;
            }
        }
    }
}
