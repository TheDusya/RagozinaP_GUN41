using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Character : DestructableGameObject
    {
        protected RangedWeapon _rangedWeapon = null;
        protected MeleeWeapon _meleeWeapon = null;
        protected float _walkingSpeed;
        public abstract void Attack();
        public override abstract void Die();
    }
}
