using JetBrains.Annotations;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UnityMVVM.ViewModelCore.Bindable
{

    /// <inheritdoc cref="IMutable{T}"/>
    public class Mutable<T> : IMutable<T>
    {

        [CanBeNull, AllowNull]
        private T _value;

        private event Action<T> _onChange;

        /// <inheritdoc cref="IMutable{T}.Value"/>
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                _onChange?.Invoke(_value);
            }
        }

        T IBindable<T>.Value => _value;

        /// <summary>
        /// Default constructor to create changable mutable value.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        public Mutable([CanBeNull, AllowNull] T initialValue = default)
        {
            _value = initialValue;
        }

        /// <inheritdoc cref="IBindable{T}.Bind(Action{T}, bool)"/>
        /// <exception cref="ArgumentNullException"></exception>
        public void Bind(Action<T> handler, bool callImmediately = true)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (callImmediately)
            {
                handler.Invoke(_value);
            }
            _onChange += handler;
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T})"/>
        public void Unbind(Action<T> handler)
        {
            _onChange -= handler;
        }
    }
}
