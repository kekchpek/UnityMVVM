namespace UnityMVVM.ViewModelCore
{
    public interface IBindable<T>
    {

        T Value { get; }

        void Bind(Action<T> handler, bool callImmediately = true);

        void Unbind(Action<T> handler);

    }
}
