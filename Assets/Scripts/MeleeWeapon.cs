namespace Assets.Scripts
{
    public abstract class MeleeWeapon : Weapon
    {
        private float _attackMultiplier;
        public float AttackMultiplier { get => _attackMultiplier; }
    }
}
