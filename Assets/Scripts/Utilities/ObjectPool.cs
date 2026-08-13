using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class ObjectPool<T> where T : Component
    {
        int _poolSize;
        Queue<T> _poolQueue;
        T _prefab;
        public ObjectPool(T prefab, int poolSize)
        {
            _prefab = prefab;
            _poolSize = poolSize;
            _poolQueue = new Queue<T>(poolSize);

            for (int i = 0; i < poolSize; i++)
            {
                T obj = Object.Instantiate(_prefab);
                obj.gameObject.SetActive(false);
                _poolQueue.Enqueue(obj);
            }
        }

        public T Take()
        {
            if (!_poolQueue.Any())
            {
                Debug.LogError("Empty pool!");
                return null;
            }
            T obj = _poolQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void PutBack(T obj)
        {
            obj.gameObject.SetActive(false);
            _poolQueue.Enqueue(obj);
        }
    }
}
