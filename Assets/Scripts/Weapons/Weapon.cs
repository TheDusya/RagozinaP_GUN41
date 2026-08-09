using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract void Equip(Character wielder);
    }
}
