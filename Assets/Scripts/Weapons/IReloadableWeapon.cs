namespace Assets.Scripts.Weapons
{
    public interface IReloadableWeapon
    {
        public void Reload();
        public void MakeEmptySound();
        public void MakeReloadSound();
    }
}
