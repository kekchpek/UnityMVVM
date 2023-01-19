using System;

namespace UnityMVVM.ViewModelCore.Bindable
{

    /// <summary>
    /// Value, changes of which could be handled with binded handlers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindable<T>
    {

        /// <summary>
        /// Current value.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Bind a handler for value changing.
        /// </summary>
        /// <param name="handler">Value changing handler.</param>
        /// <param name="callImmediately">Calls handler instantly when it is set.</param>
        /// <exception cref="ArgumentNullException">Handler is null.</exception>
        void Bind(Action<T> handler, bool callImmediately = true);

        /// <inheritdoc cref="Bind(Action{T}, bool)"/>
        void Bind(Action handler, bool callImmediately = true);

        /// <summary>
        /// Unbinds the value changing handler.
        /// </summary>
        /// <param name="handler">Value changing handler.</param>
        void Unbind(Action<T> handler);

        /// <inheritdoc cref="Unbind(Action{T})"/>
        void Unbind(Action handler);

    }
}
