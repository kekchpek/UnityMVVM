using UnityMVVM.ViewModelCore;

namespace CCG.MVVM.MainMenu
{
    public interface IMainMenuPayloadUi : IPayload
    {
        IMainMenu3dController Controller { get; }
    }
}