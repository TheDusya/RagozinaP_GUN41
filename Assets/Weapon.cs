using Unity.VisualScripting;
using UnityEngine;

namespace Assets
{
    public abstract class Weapon : MonoBehaviour, IInitializable
    {
        public abstract void Initialize();
        public abstract void Attack(); //unsure if its here
    }
}
