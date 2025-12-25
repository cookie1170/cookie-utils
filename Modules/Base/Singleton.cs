using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    /// <summary>
    ///     A globally accessible in-scene MonoBehaviour which there is only one of
    /// </summary>
    /// <typeparam name="T">The type of the singleton</typeparam>
    [PublicAPI]
    public class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T _instanceCached;

        public static T Instance
        {
            get
            {
                if (_instanceCached)
                    return _instanceCached;

                _instanceCached = FindFirstObjectByType<T>();

                if (_instanceCached)
                    return _instanceCached;

                GameObject obj = new($"Singleton_{typeof(T).Name}");
                _instanceCached = obj.AddComponent<T>();

                return _instanceCached;
            }
        }

        protected virtual void Awake()
        {
            if (!_instanceCached)
            {
                _instanceCached = this as T;
            }
            else if (_instanceCached != this)
            {
                if (gameObject.GetComponentCount() > 1 || transform.childCount > 0)
                    Destroy(this);
                else
                    Destroy(gameObject);
            }
        }
    }
}
