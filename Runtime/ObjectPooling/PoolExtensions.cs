using UnityEngine;

namespace CookieUtils.Runtime.ObjectPooling
{
	public static class PoolExtensions
	{
		public static T Get<T>(this T prefab) where T : Component
		{
			return Get(prefab, Vector3.zero, Quaternion.identity);
		}
		
		public static T Get<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
		{
			return PoolManager.Inst.GetObject(prefab.gameObject, position, rotation).GetComponent<T>();
		}
		
		public static GameObject GetObj<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
		{
			return PoolManager.Inst.GetObject(prefab.gameObject, position, rotation);
		}
		
		public static GameObject Get(this GameObject prefab)
		{
			return Get(prefab, Vector3.zero, Quaternion.identity);
		}
        
		public static GameObject Get(this GameObject prefab, Vector3 position, Quaternion rotation)
		{
			return PoolManager.Inst.GetObject(prefab.gameObject, position, rotation);
		}

		public static bool Release(this GameObject obj)
		{
			return PoolManager.Inst.Release(obj);
		}

		public static bool Release<T>(this T obj) where T : Component
		{
			return PoolManager.Inst.Release(obj.gameObject);
		}
	}
}
