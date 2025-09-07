using UnityEngine;

namespace CookieUtils.Health
{
    public class AttackData : ScriptableObject
    {
        /// <summary>
        /// The mask used for hit detection<br/>
        /// Set to int.MaxValue to pass all checks
        /// </summary>
        public int mask;
        /// <summary>
        /// The damage dealt by the Hitbox
        /// </summary>
        public int damage = 20;
        /// <summary>
        /// The I-Frames invoked by the hitbox
        /// </summary>
        public float iframes = 0.2f;

        public bool hasPierce;
        public int pierce;
    }
}