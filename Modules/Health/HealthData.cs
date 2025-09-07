using UnityEngine;

namespace CookieUtils.Health
{
    [CreateAssetMenu(fileName = "HealthData", menuName = "Scriptable Objects/HealthData")]
    public class HealthData : ScriptableObject
    {
        /// <summary>
        /// The bitwise mask used for hit comparison<br/>
        /// Set to int.MaxValue to pass all checks
        /// </summary>
        public int mask;
        public int maxHealth;
        public int startHealth;
        public AnimationCurve regenCurve;
        public Health.IframeTypes iFramesType;
    }
}
