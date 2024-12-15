using AsyncReactAwait.Bindable;

namespace CCG.MVVM.MainScreen.Subviews.TextView
{
    public class TextViewModel : UnityMVVM.ViewModelCore.ViewModel, ITextViewModel
    {

        private readonly Mutable<string> _text = new();

        public IBindable<string> Text => _text;

        public TextViewModel(TextViewPayload payload)
        {
            _text.Value = payload.text;
        }
        
    }
}