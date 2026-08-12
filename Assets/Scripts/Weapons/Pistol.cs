using Assets.Scripts.Parameters;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Weapons
{
    public class Pistol : Weapon, IRangedWeapon, IReloadableWeapon
    {
        [SerializeField]
        Transform _barrelEnd;
        int _maxBulletsNum;
        int _currentBulletsNum;
        /*public void OnEnable()
        {
            _maxBulletsNum = _weaponParameters.MaxCapacity;
            _currentBulletsNum = _maxBulletsNum; //Let's not overcomplicate things
        }*/

        void IRangedWeapon.Attack(Vector3 direction)
        {
            if (_currentBulletsNum == 0)
                MakeEmptySound();
            else
            {

                _currentBulletsNum--;
            }
        }

        public override void Equip(Character character)
        {
            
        }
        public void Reload()
        {
            _currentBulletsNum = _maxBulletsNum;
            MakeReloadSound();
        }

        public void MakeShotSound()
        {
            throw new System.NotImplementedException();
        }

        public void MakeShotEffect()
        {
            throw new System.NotImplementedException();
        }

        public void MakeEmptySound()
        {
            throw new System.NotImplementedException();
        }

        public void MakeReloadSound()
        {
            throw new System.NotImplementedException();
        }
    }
}
