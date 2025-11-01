using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace CookieUtils.ObjectPooling
{
    public class PoolManager : Singleton<PoolManager>
    {
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> _instToPool = new();
        private readonly Dictionary<GameObject, Transform> _poolContainers = new();
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> _prefabToPool = new();

        // creating it in the outside scope and then passing it into GetComponentsInChildren function to not create a new list every time
        private readonly List<IPoolCallbackReceiver> _receivers = new();

        private GameObject GetFromPool(GameObject prefab) {
            if (!_prefabToPool.ContainsKey(prefab)) {
                Transform container = new GameObject($"{prefab.name}_Container").transform;
                container.parent = transform;
                _poolContainers.Add(prefab, container);

                ObjectPool<GameObject> pool = new(
                    () => Instantiate(prefab, _poolContainers[prefab]),
                    PoolOnGet,
                    PoolOnRelease,
                    PoolOnDestroy
                );
                _prefabToPool.Add(prefab, pool);
            }

            GameObject obj = _prefabToPool[prefab].Get();
            _instToPool.TryAdd(obj, _prefabToPool[prefab]);

            return obj;
        }

        private void PoolOnDestroy(GameObject obj) {
            _instToPool.Remove(obj);
            Destroy(obj);
        }

        private void PoolOnRelease(GameObject obj) {
            obj.SetActive(false);
            obj.GetComponentsInChildren(_receivers);
            foreach (IPoolCallbackReceiver receiver in _receivers) receiver.OnRelease();
        }

        private void PoolOnGet(GameObject obj) {
            obj.SetActive(true);
            obj.GetComponentsInChildren(_receivers);
            foreach (IPoolCallbackReceiver receiver in _receivers) receiver.OnGet();
        }

        public GameObject GetObject(GameObject obj, Vector3 position, Quaternion rotation) {
            GameObject spawnedObj = GetFromPool(obj);
            spawnedObj.transform.SetPositionAndRotation(position, rotation);

            return spawnedObj;
        }

        public bool Release(GameObject obj) {
            if (!_instToPool.TryGetValue(obj, out ObjectPool<GameObject> pool)) return false;
            if (obj.activeSelf) pool.Release(obj);

            return true;
        }
    }
}