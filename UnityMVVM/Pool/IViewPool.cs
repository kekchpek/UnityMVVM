namespace UnityMVVM.Pool
{
    /// <summary>
    /// The pool for views.
    /// </summary>
    public interface IViewPool
    {
        
        /// <summary>
        /// Return a view to the pool.
        /// </summary>
        /// <param name="poolableView">The view to return to the pool.</param>
        void Push(IPoolableView poolableView);

        /// <summary>
        /// Gets a view from the pool.
        /// </summary>
        bool TryPop(out IPoolableView? poolableView);

    }
}