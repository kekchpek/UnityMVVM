using UnityEngine;

namespace UnityMVVM.Pool
{
    /// <summary>
    /// The interface for poolable views, that are not instantiating, but being taken from a pool.
    /// </summary>
    public interface IPoolableView
    {
        /// <summary>
        /// Calls, when the view is being taken from pool.
        /// </summary>
        void OnTakenFromPool();
        
        /// <summary>
        /// Calls, when the view is returning to the pool.
        /// </summary>
        void OnReturnToPool();
    }
}