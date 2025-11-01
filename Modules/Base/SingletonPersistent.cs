using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    /// <summary>
    ///     A globally accessible MonoBehaviour which there is only one of that does not get destroyed on load
    /// </summary>
    /// <typeparam name="T">The type of the singleton</typeparam>
    [PublicAPI]
    public class SingletonPersistent<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instCached;

        public static T Inst {
            get {
                if (_instCached) return _instCached;
                _instCached = FindFirstObjectByType<T>();

                if (_instCached) return _instCached;
                GameObject obj = new($"Singleton_{typeof(T).Name}");
                _instCached = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);

                return _instCached;
            }
        }


        protected virtual void Awake() {
            if (!_instCached)
                _instCached = this as T;
            else if (_instCached != this)
                Destroy(this);
        }
    }
}