using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.ObjectPooling
{
    [PublicAPI]
    public static class PoolExtensions
    {
        public static T Get<T>(this T prefab)
            where T : Component => Get(prefab, Vector3.zero, Quaternion.identity);

        public static T Get<T>(this T prefab, Vector3 position, Quaternion rotation)
            where T : Component =>
            PoolManager.Inst.GetObject(prefab.gameObject, position, rotation).GetComponent<T>();

        public static T Get<T>(this T prefab, Vector3 position)
            where T : Component =>
            PoolManager
                .Inst.GetObject(prefab.gameObject, position, Quaternion.identity)
                .GetComponent<T>();

        public static GameObject GetObj<T>(this T prefab, Vector3 position, Quaternion rotation)
            where T : Component =>
            PoolManager.Inst.GetObject(prefab.gameObject, position, rotation);

        public static GameObject GetObj<T>(this T prefab, Vector3 position)
            where T : Component =>
            PoolManager.Inst.GetObject(prefab.gameObject, position, Quaternion.identity);

        public static GameObject Get(this GameObject prefab) =>
            Get(prefab, Vector3.zero, Quaternion.identity);

        public static GameObject Get(
            this GameObject prefab,
            Vector3 position,
            Quaternion rotation
        ) => PoolManager.Inst.GetObject(prefab, position, rotation);

        public static GameObject Get(this GameObject prefab, Vector3 position) =>
            PoolManager.Inst.GetObject(prefab, position, Quaternion.identity);

        public static bool Release(this GameObject obj) =>
            PoolManager.Inst && PoolManager.Inst.Release(obj);

        public static bool Release<T>(this T obj)
            where T : Component => PoolManager.Inst && PoolManager.Inst.Release(obj.gameObject);

        public static void ReleaseOrDestroy(this GameObject obj)
        {
            if (obj.Release())
                return;

            Object.Destroy(obj);
        }

        public static void ReleaseOrDestroy<T>(this T obj)
            where T : Component => obj.gameObject.ReleaseOrDestroy();
    }
}
