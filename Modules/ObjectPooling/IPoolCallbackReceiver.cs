using JetBrains.Annotations;

namespace CookieUtils.ObjectPooling
{
    /// <summary>
    ///     Implement this interface to receive callbacks when the object is retrieved or returned to the object pool
    /// </summary>
    [PublicAPI]
    public interface IPoolCallbackReceiver
    {
        /// <summary>
        ///     Called when the object is retrieved from the object pool
        /// </summary>
        void OnGet();

        /// <summary>
        ///     Called when the object is released to the object pool
        /// </summary>
        void OnRelease();
    }
}
