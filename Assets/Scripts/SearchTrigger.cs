using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    class SearchTrigger : MonoBehaviour
    {
        public Action<GameObject> OnTriggerEnterEvent;
        private void OnTriggerEnter(Collider other) =>  OnTriggerEnterEvent?.Invoke(other.gameObject);
    }
}
