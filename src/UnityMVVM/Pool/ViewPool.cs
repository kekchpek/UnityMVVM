using System.Collections.Generic;

namespace UnityMVVM.Pool
{
    /// <summary>
    /// A pool for the views.
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ViewPool<T> : IViewPool where T : IPoolableView
    {

        private readonly Stack<IPoolableView> _poolCollection = new();

        /// <inheritdoc />
        public void Push(IPoolableView poolableView)
        {
            poolableView.OnReturnToPool();
            _poolCollection.Push(poolableView);
            OnViewReturnToPool((T)poolableView);
        }

        /// <summary>
        /// Called after a view is placed in the pool.
        /// </summary>
        /// <param name="poolableView"></param>
        protected virtual void OnViewReturnToPool(T poolableView)
        {
        }

        /// <inheritdoc />
        public bool TryPop(out IPoolableView? poolableView)
        {
            if (_poolCollection.Count > 0)
            {
                var view = _poolCollection.Pop();
                OnViewTakenFromPool((T)view);
                view.OnTakenFromPool();
                poolableView = view;
                return true;
            }

            poolableView = default;
            return false;
        }
        
        /// <summary>
        /// Called before a view is taken from pool.
        /// </summary>
        /// <param name="poolableView"></param>
        protected virtual void OnViewTakenFromPool(T poolableView)
        {
        }
    }
}