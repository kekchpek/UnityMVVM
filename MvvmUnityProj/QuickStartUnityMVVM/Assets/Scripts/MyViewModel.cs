using System;
using AsyncReactAwait.Bindable;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.QuickStart
{
    public class MyViewModel : ViewModel, IMyViewModel
    {
        
        // Private accessor to mutate a string value.
        private readonly IMutable<string> _text = new Mutable<string>();
        
        // Public bind/read only access to string value.
        public IBindable<string> Text => _text;
        
        public void OnTextChangeClick()
        {
            // Set new random text.
            _text.Set(Guid.NewGuid().ToString());
        }
    }
}