using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class Pistol : Weapon, IRangedWeapon
    {
        public override void Initialize()
        {

        }

        void IRangedWeapon.Attack(Vector3 direction)
        {

        }

        public override void Equip(Character character)
        {
            
        }
    }
}
