using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.QuickStart
{
    public interface IMyViewModel : IViewModel
    {
        // The bindable string value.
        IBindable<string> Text { get; }
        
        // The handler for ui button click event.
        void OnTextChangeClick();
    }
}