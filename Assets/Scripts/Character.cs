using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Character : DestructableGameObject
    {
        protected IRangedWeapon _rangedWeapon = null;
        protected IMeleeWeapon _meleeWeapon = null;
        protected float _walkingSpeed;
        public abstract void Attack();
        public override abstract void Die();
    }
}
