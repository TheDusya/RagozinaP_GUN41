using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Character : DestructableGameObject
    {
        protected IRangedWeapon _rangedWeapon = null;
        protected IMeleeWeapon _meleeWeapon = null;
        [SerializeField, AllowsNull]
        protected Transform _rightSocket;
        [SerializeField, AllowsNull]
        protected Transform _leftSocket;

        public override abstract void Die();
    }
}
