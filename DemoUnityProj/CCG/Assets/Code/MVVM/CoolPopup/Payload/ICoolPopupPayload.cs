using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.CoolPopup.Payload
{
    public interface ICoolPopupPayload : IPayload
    {
        bool ThrowError { get; }
    }
}