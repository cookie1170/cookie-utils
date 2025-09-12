using UnityEngine;

namespace CookieUtils
{
    /// <summary>
    /// A globally accessible in-scene MonoBehaviour which there is only one of 
    /// </summary>
    /// <typeparam name="T">The type of the singleton</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instCached;
        
        public static T Inst {
            get {
                if (_instCached) return _instCached;
                
                GameObject obj = new($"Singleton_{typeof(T).Name}");
                _instCached = obj.AddComponent<T>();
                return _instCached;
            }
        }
    }
}
