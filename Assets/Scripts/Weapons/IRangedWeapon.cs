using UnityEngine;

namespace Assets.Scripts
{
    public interface IRangedWeapon
    {
        public void Attack(Vector3 direction);
        public void MakeShotSound();
        public void MakeShotEffect();
    }
}
