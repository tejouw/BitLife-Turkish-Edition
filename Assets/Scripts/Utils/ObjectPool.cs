using UnityEngine;
using System.Collections.Generic;

namespace BitLifeTR.Utils
{
    /// <summary>
    /// Generic object pooling system for performance optimization.
    /// </summary>
    /// <typeparam name="T">Type of object to pool</typeparam>
    public class ObjectPool<T> where T : Component
    {
        private readonly Queue<T> pool = new Queue<T>();
        private readonly T prefab;
        private readonly Transform parent;
        private readonly int initialSize;

        /// <summary>
        /// Create a new object pool.
        /// </summary>
        public ObjectPool(T prefab, Transform parent, int initialSize = 10)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.initialSize = initialSize;

            // Pre-populate pool
            for (int i = 0; i < initialSize; i++)
            {
                var obj = CreateNew();
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        /// <summary>
        /// Get an object from the pool.
        /// </summary>
        public T Get()
        {
            T obj;

            if (pool.Count > 0)
            {
                obj = pool.Dequeue();
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = CreateNew();
            }

            return obj;
        }

        /// <summary>
        /// Return an object to the pool.
        /// </summary>
        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        /// <summary>
        /// Clear all objects in the pool.
        /// </summary>
        public void Clear()
        {
            while (pool.Count > 0)
            {
                var obj = pool.Dequeue();
                Object.Destroy(obj.gameObject);
            }
        }

        private T CreateNew()
        {
            return Object.Instantiate(prefab, parent);
        }
    }

    /// <summary>
    /// Simple object pool for GameObjects.
    /// </summary>
    public class GameObjectPool
    {
        private readonly Queue<GameObject> pool = new Queue<GameObject>();
        private readonly Transform parent;
        private readonly string name;

        /// <summary>
        /// Create a new GameObject pool.
        /// </summary>
        public GameObjectPool(Transform parent, string name = "PooledObject", int initialSize = 10)
        {
            this.parent = parent;
            this.name = name;

            // Pre-populate
            for (int i = 0; i < initialSize; i++)
            {
                var obj = CreateNew();
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        /// <summary>
        /// Get an object from the pool.
        /// </summary>
        public GameObject Get()
        {
            GameObject obj;

            if (pool.Count > 0)
            {
                obj = pool.Dequeue();
                obj.SetActive(true);
            }
            else
            {
                obj = CreateNew();
            }

            return obj;
        }

        /// <summary>
        /// Return an object to the pool.
        /// </summary>
        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        /// <summary>
        /// Clear all objects.
        /// </summary>
        public void Clear()
        {
            while (pool.Count > 0)
            {
                var obj = pool.Dequeue();
                Object.Destroy(obj);
            }
        }

        private GameObject CreateNew()
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);
            return obj;
        }
    }
}
