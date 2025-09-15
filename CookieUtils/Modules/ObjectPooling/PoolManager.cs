using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace CookieUtils.Runtime.ObjectPooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Inst;
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> _prefabToPool = new();
        private readonly Dictionary<GameObject, Transform> _poolContainers = new();
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> _instToPool = new();

        private void Awake()
        {
            Inst = this;
        }

        private GameObject GetFromPool(GameObject prefab)
        {
            if (!_prefabToPool.ContainsKey(prefab))
            {
                _poolContainers.Add(prefab, Instantiate(new GameObject($"{prefab.name}Container"), transform).transform);
                ObjectPool<GameObject> pool = new(() => Instantiate(prefab, _poolContainers[prefab]),
                    obj => obj.SetActive(true),
                    obj => obj.SetActive(false),
                    obj =>
                    {
                        _instToPool.Remove(obj);
                        Destroy(obj);
                    });
                _prefabToPool.Add(prefab, pool);
            }

            GameObject obj = _prefabToPool[prefab].Get();
            _instToPool.TryAdd(obj, _prefabToPool[prefab]);
            return obj;
        }

        public GameObject GetObject(GameObject obj, Vector3 position, Quaternion rotation)
        {
            GameObject spawnedObj = GetFromPool(obj);
            spawnedObj.transform.position = position;
            spawnedObj.transform.rotation = rotation;
            return spawnedObj;
        }

        public bool Release(GameObject obj)
        {
            if (!_instToPool.TryGetValue(obj, out ObjectPool<GameObject> pool)) return false;
            if (obj.activeSelf) pool.Release(obj);
            return true;
        }
    }
}
