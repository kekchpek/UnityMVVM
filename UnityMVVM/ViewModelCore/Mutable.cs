namespace UnityMVVM.ViewModelCore
{
    public class Mutable<T> : IMutable<T>
    {

        private T _value;

        private event Action<T> _onChange;

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

        public Mutable(T initialValue = default)
        {
            _value = initialValue;
        }

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

        public void Unbind(Action<T> handler)
        {
            _onChange -= handler;
        }
    }
}
