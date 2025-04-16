using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainScreen.Subviews.TextView
{
    public struct TextViewPayload : IPayload
    {
        public readonly string text;

        public TextViewPayload(string text)
        {
            this.text = text;
        }
    }
}